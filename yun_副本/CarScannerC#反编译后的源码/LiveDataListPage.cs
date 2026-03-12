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
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020000DE RID: 222
	[XamlFilePath("Pages\\LiveDataListPage.xaml")]
	public class LiveDataListPage : ContentPage
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x0003E6E8 File Offset: 0x0003C8E8
		public LiveDataListPage()
		{
			this.UpdateOnlyVisible = SharedSettings.Current.LiveDataListPageUpdateOnlyVisible;
			this.InitializeComponent();
			if (!SharedSettings.Current.DashboardAnimation)
			{
				this.popupView.AnimationMode = 6;
			}
			if (Device.Idiom == 2)
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["tabletTemplate"];
			}
			else
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["mobileTemplate"];
			}
			if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidUseFullscreen)
			{
				try
				{
					PlatformHelper.DroidService.Window_SetFullscreenOn();
				}
				catch (Exception)
				{
				}
			}
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			if (!SharedSettings.Current.InfoShowed_AllSensors)
			{
				SharedSettings.Current.InfoShowed_AllSensors = true;
				this.btnInfo_Clicked(null, null);
			}
			base.BindingContext = this;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0003E81C File Offset: 0x0003CA1C
		private async void btnShowSettings_Clicked(object sender, EventArgs e)
		{
			try
			{
				this.pageSettingsPopupLayout.Show(false);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0003E854 File Offset: 0x0003CA54
		private async void Lv_ItemDisappearing(object sender, ItemDisappearingEventArgs e)
		{
			LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
			if (liveDataPIDModel != null)
			{
				this.VisiblePIDs.Remove(liveDataPIDModel);
			}
			if (this.UpdateOnlyVisible)
			{
				List<LiveDataPIDModel> visibleCopy = new List<LiveDataPIDModel>(this.VisiblePIDs);
				await Task.Delay(500);
				if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, this.VisiblePIDs))
				{
					this.UpdateRequests();
				}
				visibleCopy = null;
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0003E894 File Offset: 0x0003CA94
		private async void Lv_ItemAppearing(object sender, ItemAppearingEventArgs e)
		{
			LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
			if (liveDataPIDModel != null)
			{
				this.VisiblePIDs.Add(liveDataPIDModel);
			}
			if (this.UpdateOnlyVisible)
			{
				List<LiveDataPIDModel> visibleCopy = new List<LiveDataPIDModel>(this.VisiblePIDs);
				await Task.Delay(500);
				if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, this.VisiblePIDs))
				{
					this.UpdateRequests();
				}
				visibleCopy = null;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0003E8D3 File Offset: 0x0003CAD3
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0003E8DB File Offset: 0x0003CADB
		public bool UpdateOnlyVisible
		{
			get
			{
				return this._UpdateOnlyVisible;
			}
			set
			{
				this.SetUpdateOnlyVisible(value);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0003E8E4 File Offset: 0x0003CAE4
		private async void SetUpdateOnlyVisible(bool value)
		{
			if (this._UpdateOnlyVisible != value)
			{
				this._UpdateOnlyVisible = value;
				SharedSettings.Current.LiveDataListPageUpdateOnlyVisible = value;
				this.OnPropertyChanged("UpdateOnlyVisible");
				await Task.Delay(500);
				if (this.UpdateOnlyVisible == value)
				{
					this.UpdateRequests();
				}
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0003E924 File Offset: 0x0003CB24
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			try
			{
				this.goBackRequested = true;
				await App.OBDReader.ClearRequestQueue();
				if (SharedSettings.Current.AndroidUseFullscreen && SharedSettings.Current.AndroidUseFullscreen)
				{
					try
					{
						PlatformHelper.DroidService.Window_SetFullscreenOff();
					}
					catch (Exception)
					{
					}
				}
				foreach (LiveDataPIDModel liveDataPIDModel in this.ModelsCollection)
				{
					liveDataPIDModel.Unsubscribe();
				}
				await base.Navigation.PopAsync();
			}
			catch
			{
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0003E95B File Offset: 0x0003CB5B
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnBack_Clicked(this, null);
			}
			return true;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0003E988 File Offset: 0x0003CB88
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0003E990 File Offset: 0x0003CB90
		public LiveDataListPage.BaseListFilters BaseListFilter
		{
			get
			{
				return this._BaseListFilter;
			}
			set
			{
				this.SetBaseListFilterValue(value);
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0003E99C File Offset: 0x0003CB9C
		private async void SetBaseListFilterValue(LiveDataListPage.BaseListFilters value)
		{
			if (this._BaseListFilter != value)
			{
				this._BaseListFilter = value;
				this.OnPropertyChanged("BaseListFilter");
				await Task.Delay(500);
				if (this._BaseListFilter == value)
				{
					this.UpdateRequests();
				}
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000027D4 File Offset: 0x000009D4
		private void pickerBaseList_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0003E9DC File Offset: 0x0003CBDC
		private async void Page_Appearing(object sender, EventArgs e)
		{
			if (!SharedSettings.Current.InfoShowed_AllSensors)
			{
				SharedSettings.Current.InfoShowed_AllSensors = true;
				this.btnInfo_Clicked(null, null);
			}
			await this.InitializeModels();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0003EA14 File Offset: 0x0003CC14
		private void Page_Disappearing(object sender, EventArgs e)
		{
			foreach (LiveDataPIDModel liveDataPIDModel in this.ModelsCollection)
			{
				liveDataPIDModel.Unsubscribe();
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0003EA60 File Offset: 0x0003CC60
		public ObservableCollection<LiveDataPIDModel> ModelsCollection
		{
			get
			{
				return this._ModelsCollection;
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0003EA68 File Offset: 0x0003CC68
		private async Task InitializeModels()
		{
			try
			{
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !OBDReaderSimulator.Current.IsActive)
				{
					await App.OBDReader.ClearRequestQueue();
				}
				if (this.ModelsCollection.Count == 0)
				{
					this.lv.BatchBegin();
					string @string = Translate.GetString("ios_LoadingSensorsList");
					int num = LiveDataPIDModel._PIDCollection.Count - 1;
					List<LiveDataPIDModel> list = new List<LiveDataPIDModel>(num);
					this.activityFrame.Text = string.Format(@string, 0, num);
					new List<OBDRequest>(num);
					for (int i = 0; i < num; i++)
					{
						if (this.goBackRequested)
						{
							return;
						}
						try
						{
							LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
							list.Add(liveDataPIDModel);
							liveDataPIDModel.SelectedPID = LiveDataPIDModel._PIDCollection[i + 1];
							this.activityFrame.Text = string.Format(@string, i, num);
						}
						catch (Exception)
						{
						}
					}
					foreach (LiveDataPIDModel liveDataPIDModel2 in list)
					{
						this.ModelsCollection.Add(liveDataPIDModel2);
					}
					if (!this.UpdateOnlyVisible)
					{
						this.UpdateRequests();
					}
					await Task.Delay(500);
					this.lv.BatchCommit();
					this.activityFrame.IsVisible = false;
				}
				else
				{
					foreach (LiveDataPIDModel liveDataPIDModel3 in this.ModelsCollection)
					{
						liveDataPIDModel3.Subscribe();
					}
					this.UpdateRequests();
					if (this.goBackRequested)
					{
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0003EAAC File Offset: 0x0003CCAC
		private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.filterText = this.searchBar.Text;
			await Task.Delay(1000);
			string text = this.searchBar.Text;
			if (this.filterText == text)
			{
				try
				{
					IEnumerable<LiveDataPIDModel> filteredModels = this.GetFilteredModels(this.filterText);
					this.lv.ItemsSource = filteredModels;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0003EAE4 File Offset: 0x0003CCE4
		private IEnumerable<LiveDataPIDModel> GetFilteredModels(string filter)
		{
			IEnumerable<LiveDataPIDModel> enumerable;
			switch (this.BaseListFilter)
			{
			case LiveDataListPage.BaseListFilters.OBDII:
				enumerable = this.ModelsCollection.Where((LiveDataPIDModel x) => x.SelectedPID != null && !CustomPIDViewModel.CurrentCustom.PidCollection.Contains(x.SelectedPID) && !CustomPIDViewModel.CurrentProfile.PidCollection.Contains(x.SelectedPID)).ToList<LiveDataPIDModel>();
				goto IL_00BF;
			case LiveDataListPage.BaseListFilters.Profile:
				enumerable = this.ModelsCollection.Where((LiveDataPIDModel x) => x.SelectedPID != null && CustomPIDViewModel.CurrentProfile.PidCollection.Contains(x.SelectedPID)).ToList<LiveDataPIDModel>();
				goto IL_00BF;
			case LiveDataListPage.BaseListFilters.Custom:
				enumerable = this.ModelsCollection.Where((LiveDataPIDModel x) => x.SelectedPID != null && CustomPIDViewModel.CurrentCustom.PidCollection.Contains(x.SelectedPID)).ToList<LiveDataPIDModel>();
				goto IL_00BF;
			}
			enumerable = this.ModelsCollection;
			IL_00BF:
			if (filter == null)
			{
				return enumerable;
			}
			filter = filter.Trim();
			if (string.IsNullOrEmpty(filter))
			{
				return enumerable;
			}
			string[] array = filter.Split(new char[] { ' ' });
			for (int i = 0; i < array.Length; i++)
			{
				string word = array[i];
				enumerable = enumerable.Where((LiveDataPIDModel x) => x != null && x.SelectedPID != null && ((!string.IsNullOrEmpty(x.SelectedPID.Name) && x.SelectedPID.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.SelectedPID.ShortName) && x.SelectedPID.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)));
			}
			if (SharedSettings.Current.AlwaysRecordFuelConsumption)
			{
				LiveDataPIDModel liveDataPIDModel = this.ModelsCollection.FirstOrDefault((LiveDataPIDModel x) => x.SelectedPID is PID_CalculatedAVGFuelConsumption);
				if (liveDataPIDModel != null && !enumerable.Contains(liveDataPIDModel))
				{
					enumerable = enumerable.Concat(new LiveDataPIDModel[] { liveDataPIDModel });
				}
			}
			return enumerable;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0003EC65 File Offset: 0x0003CE65
		private void btnFilter_Clicked(object sender, EventArgs e)
		{
			this.panelFilter.IsVisible = !this.panelFilter.IsVisible;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0003EC80 File Offset: 0x0003CE80
		private async void listView_ItemSelected(object sender, EventArgs e)
		{
			LiveDataPIDModel liveDataPIDModel = (sender as Grid).BindingContext as LiveDataPIDModel;
			IPID selectedPID = liveDataPIDModel.SelectedPID;
			if (selectedPID != null && selectedPID is CustomPID && (selectedPID as CustomPID).IsAction)
			{
				liveDataPIDModel.Action.Execute(null);
			}
			else
			{
				SharedSettings.Current.LiveDataPIDId0 = liveDataPIDModel.SelectedPID.Id;
				SharedSettings.Current.ChartsVisible = 1;
				base.Navigation.PushAsync(new LiveDataChartPage());
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0003ECBF File Offset: 0x0003CEBF
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_MainPage_TileAllSensors"), Translate.GetString("ios_AllSensorsListInfoText"), "OK");
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0003ECE4 File Offset: 0x0003CEE4
		private async void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			LiveDataPIDModel model = e.ItemData as LiveDataPIDModel;
			this.lv.SelectedItem = null;
			IPID selectedPID = model.SelectedPID;
			if (selectedPID != null && selectedPID is CustomPID && (selectedPID as CustomPID).IsAction)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Execute " + selectedPID.Name + "?", "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					model.Action.Execute(null);
				}
			}
			else
			{
				SharedSettings.Current.LiveDataPIDId0 = model.SelectedPID.Id;
				SharedSettings.Current.ChartsVisible = 1;
				base.Navigation.PushAsync(new LiveDataChartPage());
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0003ED24 File Offset: 0x0003CF24
		private async void MenuItemDIsablePID_Clicked(object sender, EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			LiveDataPIDModel model = (LiveDataPIDModel)menuItem.BindingContext;
			IPID selectedPID = model.SelectedPID;
			CustomPID cpid = selectedPID as CustomPID;
			if (cpid != null && CustomPIDViewModel.CurrentProfile.PidCollection.Contains(cpid))
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(string.Format(Translate.GetString("ios_RemoveSensorFromList"), cpid.Name), string.Format("You can manage active sensors in Settings -> {0} -> {1} -> {2}", Translate.GetString("Settings_Control_tbConnection.Text"), Translate.GetString("ios_AdvancedConnectionSettings"), Translate.GetString("ios_ManageProfilePids")), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					CustomPIDViewModel.CurrentProfile.PidCollection.Remove(cpid);
					if (!CustomPIDViewModel.DisabledProfile.Loaded)
					{
						await Task.Run(delegate
						{
							CustomPIDViewModel.DisabledProfile.Load();
						});
					}
					CustomPIDViewModel.DisabledProfile.PidCollection.Add(cpid);
					CustomPIDViewModel.CurrentProfile.Save();
					CustomPIDViewModel.DisabledProfile.Save();
					model.Unsubscribe();
					this.ModelsCollection.Remove(model);
					this.searchBar_TextChanged(this.searchBar, new TextChangedEventArgs(this.searchBar.Text, this.searchBar.Text));
				}
			}
			else
			{
				await base.DisplayAlert(Translate.GetString("ios_CantRemoveSensor"), "", "OK");
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0003ED63 File Offset: 0x0003CF63
		private void UpdateRequests()
		{
			MainAppRequestProducer.Delegate = new AddRequestsDelegate(this.AddRequestsDelegate);
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0003ED7C File Offset: 0x0003CF7C
		private void AddRequestsDelegate(List<OBDRequest> requests)
		{
			IEnumerable<LiveDataPIDModel> enumerable;
			if (this.UpdateOnlyVisible)
			{
				enumerable = new List<LiveDataPIDModel>(this.VisiblePIDs);
			}
			else
			{
				enumerable = this.ModelsCollection;
			}
			foreach (LiveDataPIDModel liveDataPIDModel in enumerable)
			{
				LiveDataPIDModel.GetRequests(liveDataPIDModel.SelectedPID, requests, null, "");
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0003EDEC File Offset: 0x0003CFEC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LiveDataListPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/LiveDataListPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 14);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 14);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 21);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 21);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 30);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 30);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 26);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 18);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 21);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 21);
			On on5;
			VisualDiagnostics.RegisterSourceInfo(on5 = new On(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 30);
			On on6;
			VisualDiagnostics.RegisterSourceInfo(on6 = new On(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 30);
			OnPlatform<Thickness> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<Thickness>(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 26);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 18);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 17);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 21);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 21);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 21);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 18);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 21);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 21);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 21);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 21);
			CheckBoxWithLabel checkBoxWithLabel;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel = new CheckBoxWithLabel(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 18);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 14);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 291, 17);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 282, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 307, 25);
			PopupView popupView;
			VisualDiagnostics.RegisterSourceInfo(popupView = new PopupView(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 302, 22);
			SfPopupLayout sfPopupLayout;
			VisualDiagnostics.RegisterSourceInfo(sfPopupLayout = new SfPopupLayout(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 317, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 323, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("btnFilter", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "btnFilter";
			}
			nameScope.RegisterName("panelFilter", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "panelFilter";
			}
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("cbUpdateOnlyVisible", checkBoxWithLabel);
			if (checkBoxWithLabel.StyleId == null)
			{
				checkBoxWithLabel.StyleId = "cbUpdateOnlyVisible";
			}
			nameScope.RegisterName("lv", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lv";
			}
			nameScope.RegisterName("pageSettingsPopupLayout", sfPopupLayout);
			if (sfPopupLayout.StyleId == null)
			{
				sfPopupLayout.StyleId = "pageSettingsPopupLayout";
			}
			nameScope.RegisterName("popupView", popupView);
			if (popupView.StyleId == null)
			{
				popupView.StyleId = "popupView";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.page = this;
			this.btnFilter = linkButton2;
			this.panelFilter = stackLayout2;
			this.searchBar = entry;
			this.cbUpdateOnlyVisible = checkBoxWithLabel;
			this.lv = sfListView;
			this.pageSettingsPopupLayout = sfPopupLayout;
			this.popupView = popupView;
			this.activityFrame = activityFrame;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			IDataTemplate dataTemplate4 = dataTemplate;
			LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_59 <InitializeComponent>_anonXamlCDataTemplate_ = new LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_59();
			object[] array = new object[0 + 3];
			array[0] = dataTemplate;
			array[1] = resourceDictionary;
			array[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary.Add("settingsPopupTemplate", dataTemplate);
			IDataTemplate dataTemplate5 = dataTemplate2;
			LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_60 <InitializeComponent>_anonXamlCDataTemplate_2 = new LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_60();
			object[] array2 = new object[0 + 3];
			array2[0] = dataTemplate2;
			array2[1] = resourceDictionary;
			array2[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			resourceDictionary.Add("mobileTemplate", dataTemplate2);
			IDataTemplate dataTemplate6 = dataTemplate3;
			LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_61 <InitializeComponent>_anonXamlCDataTemplate_3 = new LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_61();
			object[] array3 = new object[0 + 3];
			array3[0] = dataTemplate3;
			array3[1] = resourceDictionary;
			array3[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			resourceDictionary.Add("tabletTemplate", dataTemplate3);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Page_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 1];
			array4[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			on.Platform = new List<string>(1) { "Android" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "5,0,5,0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
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
			object[] array5 = new object[0 + 3];
			array5[0] = linkButton;
			array5[1] = grid;
			array5[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array5, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Back";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = linkButton;
			array6[1] = grid;
			array6[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension3.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = nonScalableLabel;
			array7[1] = grid;
			array7[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array7, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension4.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = nonScalableLabel;
			array8[1] = grid;
			array8[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array8, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 17)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			translate2.Text = "ios_MainPage_TileAllSensors";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = nonScalableLabel;
			array9[1] = grid;
			array9[2] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 17)));
			object obj8 = markupExtension6.ProvideValue(xamlServiceProvider6);
			nonScalableLabel.Text = obj8;
			nonScalableLabel.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(nonScalableLabel);
			stackLayout.SetValue(Grid.ColumnProperty, 2);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnFilter_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension5.Key = "NB_filter";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = linkButton2;
			array10[1] = stackLayout;
			array10[2] = grid;
			array10[3] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array10, Button.ImageProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(199, 21)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource5.Key);
			dynamicResourceExtension6.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = linkButton2;
			array11[1] = stackLayout;
			array11[2] = grid;
			array11[3] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array11, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(200, 21)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource6.Key);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "0";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on4);
			linkButton2.SetValue(View.MarginProperty, onPlatform2);
			stackLayout.Children.Add(linkButton2);
			linkButton3.SetValue(Grid.ColumnProperty, 2);
			linkButton3.Clicked += this.btnInfo_Clicked;
			linkButton3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension7.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = linkButton3;
			array12[1] = stackLayout;
			array12[2] = grid;
			array12[3] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array12, Button.ImageProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(212, 21)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			linkButton3.SetDynamicResource(Button.ImageProperty, dynamicResource7.Key);
			dynamicResourceExtension8.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = linkButton3;
			array13[1] = stackLayout;
			array13[2] = grid;
			array13[3] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array13, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 21)));
			DynamicResource dynamicResource8 = markupExtension10.ProvideValue(xamlServiceProvider10);
			linkButton3.SetDynamicResource(VisualElement.StyleProperty, dynamicResource8.Key);
			linkButton3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			on5.Platform = new List<string>(1) { "iOS" };
			on5.Value = "0";
			onPlatform3.Platforms.Add(on5);
			on6.Platform = new List<string>(1) { "Android" };
			on6.Value = "0,0,5,0";
			onPlatform3.Platforms.Add(on6);
			linkButton3.SetValue(View.MarginProperty, onPlatform3);
			stackLayout.Children.Add(linkButton3);
			grid.Children.Add(stackLayout);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid2.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			stackLayout2.SetValue(Grid.RowProperty, 0);
			dynamicResourceExtension9.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = stackLayout2;
			array14[1] = grid2;
			array14[2] = this;
			object obj13;
			xamlServiceProvider11.Add(typeFromHandle21, obj13 = new SimpleValueTargetProvider(array14, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(245, 17)));
			DynamicResource dynamicResource9 = markupExtension11.ProvideValue(xamlServiceProvider11);
			stackLayout2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource9.Key);
			stackLayout2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(View.MarginProperty, new Thickness(10.0, 0.0));
			dynamicResourceExtension10.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = entry;
			array15[1] = stackLayout2;
			array15[2] = grid2;
			array15[3] = this;
			object obj14;
			xamlServiceProvider12.Add(typeFromHandle23, obj14 = new SimpleValueTargetProvider(array15, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(252, 21)));
			DynamicResource dynamicResource10 = markupExtension12.ProvideValue(xamlServiceProvider12);
			entry.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource10.Key);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			dynamicResourceExtension11.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = entry;
			array16[1] = stackLayout2;
			array16[2] = grid2;
			array16[3] = this;
			object obj15;
			xamlServiceProvider13.Add(typeFromHandle25, obj15 = new SimpleValueTargetProvider(array16, Entry.PlaceholderColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(256, 21)));
			DynamicResource dynamicResource11 = markupExtension13.ProvideValue(xamlServiceProvider13);
			entry.SetDynamicResource(Entry.PlaceholderColorProperty, dynamicResource11.Key);
			entry.TextChanged += this.searchBar_TextChanged;
			dynamicResourceExtension12.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = entry;
			array17[1] = stackLayout2;
			array17[2] = grid2;
			array17[3] = this;
			object obj16;
			xamlServiceProvider14.Add(typeFromHandle27, obj16 = new SimpleValueTargetProvider(array17, Entry.TextColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 21)));
			DynamicResource dynamicResource12 = markupExtension14.ProvideValue(xamlServiceProvider14);
			entry.SetDynamicResource(Entry.TextColorProperty, dynamicResource12.Key);
			stackLayout2.Children.Add(entry);
			checkBoxWithLabel.SetValue(Grid.RowProperty, 1);
			checkBoxWithLabel.SetValue(View.MarginProperty, new Thickness(10.0, 0.0));
			dynamicResourceExtension13.Key = "ButtonRedColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = checkBoxWithLabel;
			array18[1] = stackLayout2;
			array18[2] = grid2;
			array18[3] = this;
			object obj17;
			xamlServiceProvider15.Add(typeFromHandle29, obj17 = new SimpleValueTargetProvider(array18, CheckBoxWithLabel.CheckColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(264, 21)));
			DynamicResource dynamicResource13 = markupExtension15.ProvideValue(xamlServiceProvider15);
			checkBoxWithLabel.SetDynamicResource(CheckBoxWithLabel.CheckColorProperty, dynamicResource13.Key);
			checkBoxWithLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			bindingExtension.Mode = 1;
			bindingExtension.Path = "UpdateOnlyVisible";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			checkBoxWithLabel.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase);
			translate3.Text = "settings_LiveDataListPageUpdateOnlyVisible";
			IMarkupExtension markupExtension16 = translate3;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = checkBoxWithLabel;
			array19[1] = stackLayout2;
			array19[2] = grid2;
			array19[3] = this;
			object obj18;
			xamlServiceProvider16.Add(typeFromHandle31, obj18 = new SimpleValueTargetProvider(array19, CheckBoxWithLabel.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(267, 21)));
			object obj19 = markupExtension16.ProvideValue(xamlServiceProvider16);
			checkBoxWithLabel.Text = obj19;
			dynamicResourceExtension14.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = checkBoxWithLabel;
			array20[1] = stackLayout2;
			array20[2] = grid2;
			array20[3] = this;
			object obj20;
			xamlServiceProvider17.Add(typeFromHandle33, obj20 = new SimpleValueTargetProvider(array20, CheckBoxWithLabel.TextColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(268, 21)));
			DynamicResource dynamicResource14 = markupExtension17.ProvideValue(xamlServiceProvider17);
			checkBoxWithLabel.SetDynamicResource(CheckBoxWithLabel.TextColorProperty, dynamicResource14.Key);
			stackLayout2.Children.Add(checkBoxWithLabel);
			grid2.Children.Add(stackLayout2);
			sfListView.SetValue(Grid.RowProperty, 2);
			sfListView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfListView.ItemAppearing += new ItemAppearingEventHandler(this.Lv_ItemAppearing);
			sfListView.ItemDisappearing += new ItemDisappearingEventHandler(this.Lv_ItemDisappearing);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.Lv_ItemTapped);
			bindingExtension2.Path = "ModelsCollection";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			sfListView.SetBinding(SfListView.ItemsSourceProperty, bindingBase2);
			sfListView.SetValue(SfListView.SelectionBackgroundColorProperty, Color.Transparent);
			sfListView.SetValue(SfListView.SelectionModeProperty, 0);
			grid2.Children.Add(sfListView);
			sfPopupLayout.SetValue(SfPopupLayout.IsOpenProperty, false);
			sfPopupLayout.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			popupView.SetValue(PopupView.AnimationModeProperty, 4);
			popupView.SetValue(PopupView.AutoSizeModeProperty, 2);
			popupView.SetValue(VisualElement.BackgroundColorProperty, new Color(0.7529411911964417, 0.7529411911964417, 0.7529411911964417, 1.0));
			staticResourceExtension.Key = "settingsPopupTemplate";
			IMarkupExtension markupExtension18 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = popupView;
			array21[1] = sfPopupLayout;
			array21[2] = grid2;
			array21[3] = this;
			object obj21;
			xamlServiceProvider18.Add(typeFromHandle35, obj21 = new SimpleValueTargetProvider(array21, PopupView.ContentTemplateProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(LiveDataListPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(307, 25)));
			object obj22 = markupExtension18.ProvideValue(xamlServiceProvider18);
			popupView.ContentTemplate = obj22;
			popupView.SetValue(PopupView.HeaderTitleProperty, "");
			popupView.SetValue(PopupView.ShowCloseButtonProperty, true);
			popupView.SetValue(PopupView.ShowFooterProperty, false);
			popupView.SetValue(PopupView.ShowHeaderProperty, false);
			sfPopupLayout.SetValue(SfPopupLayout.PopupViewProperty, popupView);
			grid2.Children.Add(sfPopupLayout);
			activityFrame.SetValue(Grid.RowProperty, 2);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			complexAdView.SetValue(Grid.RowProperty, 3);
			complexAdView.SetValue(View.MarginProperty, new Thickness(-5.0, 0.0, -5.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid2.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00041BB0 File Offset: 0x0003FDB0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LiveDataListPage>(this, typeof(LiveDataListPage));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.btnFilter = NameScopeExtensions.FindByName<LinkButton>(this, "btnFilter");
			this.panelFilter = NameScopeExtensions.FindByName<StackLayout>(this, "panelFilter");
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.cbUpdateOnlyVisible = NameScopeExtensions.FindByName<CheckBoxWithLabel>(this, "cbUpdateOnlyVisible");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
			this.pageSettingsPopupLayout = NameScopeExtensions.FindByName<SfPopupLayout>(this, "pageSettingsPopupLayout");
			this.popupView = NameScopeExtensions.FindByName<PopupView>(this, "popupView");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0400031F RID: 799
		private List<LiveDataPIDModel> VisiblePIDs = new List<LiveDataPIDModel>();

		// Token: 0x04000320 RID: 800
		private bool _UpdateOnlyVisible = true;

		// Token: 0x04000321 RID: 801
		private volatile bool goBackRequested;

		// Token: 0x04000322 RID: 802
		private LiveDataListPage.BaseListFilters _BaseListFilter;

		// Token: 0x04000323 RID: 803
		private ObservableCollection<LiveDataPIDModel> _ModelsCollection = new ObservableCollection<LiveDataPIDModel>();

		// Token: 0x04000324 RID: 804
		private string filterText = "";

		// Token: 0x04000325 RID: 805
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04000326 RID: 806
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnFilter;

		// Token: 0x04000327 RID: 807
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelFilter;

		// Token: 0x04000328 RID: 808
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04000329 RID: 809
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CheckBoxWithLabel cbUpdateOnlyVisible;

		// Token: 0x0400032A RID: 810
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x0400032B RID: 811
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfPopupLayout pageSettingsPopupLayout;

		// Token: 0x0400032C RID: 812
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private PopupView popupView;

		// Token: 0x0400032D RID: 813
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0400032E RID: 814
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x020000DF RID: 223
		public enum BaseListFilters
		{
			// Token: 0x04000330 RID: 816
			All,
			// Token: 0x04000331 RID: 817
			OBDII,
			// Token: 0x04000332 RID: 818
			Profile,
			// Token: 0x04000333 RID: 819
			Custom
		}

		// Token: 0x020000E0 RID: 224
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06000470 RID: 1136 RVA: 0x00041C78 File Offset: 0x0003FE78
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06000471 RID: 1137 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06000472 RID: 1138 RVA: 0x00041C84 File Offset: 0x0003FE84
			internal bool <GetFilteredModels>b__28_0(LiveDataPIDModel x)
			{
				return x.SelectedPID != null && CustomPIDViewModel.CurrentProfile.PidCollection.Contains(x.SelectedPID);
			}

			// Token: 0x06000473 RID: 1139 RVA: 0x00041CA5 File Offset: 0x0003FEA5
			internal bool <GetFilteredModels>b__28_1(LiveDataPIDModel x)
			{
				return x.SelectedPID != null && CustomPIDViewModel.CurrentCustom.PidCollection.Contains(x.SelectedPID);
			}

			// Token: 0x06000474 RID: 1140 RVA: 0x00041CC6 File Offset: 0x0003FEC6
			internal bool <GetFilteredModels>b__28_2(LiveDataPIDModel x)
			{
				return x.SelectedPID != null && !CustomPIDViewModel.CurrentCustom.PidCollection.Contains(x.SelectedPID) && !CustomPIDViewModel.CurrentProfile.PidCollection.Contains(x.SelectedPID);
			}

			// Token: 0x06000475 RID: 1141 RVA: 0x00041D01 File Offset: 0x0003FF01
			internal bool <GetFilteredModels>b__28_4(LiveDataPIDModel x)
			{
				return x.SelectedPID is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x00041D11 File Offset: 0x0003FF11
			internal void <MenuItemDIsablePID_Clicked>b__33_0()
			{
				CustomPIDViewModel.DisabledProfile.Load();
			}

			// Token: 0x04000334 RID: 820
			public static readonly LiveDataListPage.<>c <>9 = new LiveDataListPage.<>c();

			// Token: 0x04000335 RID: 821
			public static Func<LiveDataPIDModel, bool> <>9__28_0;

			// Token: 0x04000336 RID: 822
			public static Func<LiveDataPIDModel, bool> <>9__28_1;

			// Token: 0x04000337 RID: 823
			public static Func<LiveDataPIDModel, bool> <>9__28_2;

			// Token: 0x04000338 RID: 824
			public static Func<LiveDataPIDModel, bool> <>9__28_4;

			// Token: 0x04000339 RID: 825
			public static Action <>9__33_0;
		}

		// Token: 0x020000E1 RID: 225
		[CompilerGenerated]
		private sealed class <>c__DisplayClass28_0
		{
			// Token: 0x06000477 RID: 1143 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass28_0()
			{
			}

			// Token: 0x06000478 RID: 1144 RVA: 0x00041D20 File Offset: 0x0003FF20
			internal bool <GetFilteredModels>b__3(LiveDataPIDModel x)
			{
				return x != null && x.SelectedPID != null && ((!string.IsNullOrEmpty(x.SelectedPID.Name) && x.SelectedPID.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.SelectedPID.ShortName) && x.SelectedPID.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0));
			}

			// Token: 0x0400033A RID: 826
			public string word;
		}

		// Token: 0x020000E2 RID: 226
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <InitializeModels>d__25 : IAsyncStateMachine
		{
			// Token: 0x06000479 RID: 1145 RVA: 0x00041D9C File Offset: 0x0003FF9C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
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
								num = (num2 = -1);
								goto IL_0206;
							}
							if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU || OBDReaderSimulator.Current.IsActive)
							{
								goto IL_0095;
							}
							taskAwaiter = App.OBDReader.ClearRequestQueue().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<InitializeModels>d__25>(ref taskAwaiter, ref this);
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
						IL_0095:
						if (liveDataListPage.ModelsCollection.Count == 0)
						{
							liveDataListPage.lv.BatchBegin();
							string @string = Translate.GetString("ios_LoadingSensorsList");
							int num3 = LiveDataPIDModel._PIDCollection.Count - 1;
							List<LiveDataPIDModel> list = new List<LiveDataPIDModel>(num3);
							liveDataListPage.activityFrame.Text = string.Format(@string, 0, num3);
							new List<OBDRequest>(num3);
							for (int i = 0; i < num3; i++)
							{
								if (liveDataListPage.goBackRequested)
								{
									goto IL_028E;
								}
								try
								{
									LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
									list.Add(liveDataPIDModel);
									liveDataPIDModel.SelectedPID = LiveDataPIDModel._PIDCollection[i + 1];
									liveDataListPage.activityFrame.Text = string.Format(@string, i, num3);
								}
								catch (Exception)
								{
								}
							}
							List<LiveDataPIDModel>.Enumerator enumerator = list.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									LiveDataPIDModel liveDataPIDModel2 = enumerator.Current;
									liveDataListPage.ModelsCollection.Add(liveDataPIDModel2);
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							if (!liveDataListPage.UpdateOnlyVisible)
							{
								liveDataListPage.UpdateRequests();
							}
							taskAwaiter = Task.Delay(500).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<InitializeModels>d__25>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							IEnumerator<LiveDataPIDModel> enumerator2 = liveDataListPage.ModelsCollection.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									LiveDataPIDModel liveDataPIDModel3 = enumerator2.Current;
									liveDataPIDModel3.Subscribe();
								}
							}
							finally
							{
								if (num < 0 && enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
							liveDataListPage.UpdateRequests();
							if (liveDataListPage.goBackRequested)
							{
								goto IL_028E;
							}
							goto IL_026E;
						}
						IL_0206:
						taskAwaiter.GetResult();
						liveDataListPage.lv.BatchCommit();
						liveDataListPage.activityFrame.IsVisible = false;
						IL_026E:;
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
				IL_028E:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600047A RID: 1146 RVA: 0x000420C8 File Offset: 0x000402C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400033B RID: 827
			public int <>1__state;

			// Token: 0x0400033C RID: 828
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400033D RID: 829
			public LiveDataListPage <>4__this;

			// Token: 0x0400033E RID: 830
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000E3 RID: 227
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemAppearing>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600047B RID: 1147 RVA: 0x000420D8 File Offset: 0x000402D8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
						if (liveDataPIDModel != null)
						{
							liveDataListPage.VisiblePIDs.Add(liveDataPIDModel);
						}
						if (!liveDataListPage.UpdateOnlyVisible)
						{
							goto IL_00C9;
						}
						visibleCopy = new List<LiveDataPIDModel>(liveDataListPage.VisiblePIDs);
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<Lv_ItemAppearing>d__4>(ref taskAwaiter, ref this);
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
					if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, liveDataListPage.VisiblePIDs))
					{
						liveDataListPage.UpdateRequests();
					}
					visibleCopy = null;
					IL_00C9:;
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

			// Token: 0x0600047C RID: 1148 RVA: 0x000421EC File Offset: 0x000403EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400033F RID: 831
			public int <>1__state;

			// Token: 0x04000340 RID: 832
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000341 RID: 833
			public ItemAppearingEventArgs e;

			// Token: 0x04000342 RID: 834
			public LiveDataListPage <>4__this;

			// Token: 0x04000343 RID: 835
			private List<LiveDataPIDModel> <visibleCopy>5__2;

			// Token: 0x04000344 RID: 836
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000E4 RID: 228
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemDisappearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600047D RID: 1149 RVA: 0x000421FC File Offset: 0x000403FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
						if (liveDataPIDModel != null)
						{
							liveDataListPage.VisiblePIDs.Remove(liveDataPIDModel);
						}
						if (!liveDataListPage.UpdateOnlyVisible)
						{
							goto IL_00CA;
						}
						visibleCopy = new List<LiveDataPIDModel>(liveDataListPage.VisiblePIDs);
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<Lv_ItemDisappearing>d__3>(ref taskAwaiter, ref this);
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
					if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, liveDataListPage.VisiblePIDs))
					{
						liveDataListPage.UpdateRequests();
					}
					visibleCopy = null;
					IL_00CA:;
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

			// Token: 0x0600047E RID: 1150 RVA: 0x00042314 File Offset: 0x00040514
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000345 RID: 837
			public int <>1__state;

			// Token: 0x04000346 RID: 838
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000347 RID: 839
			public ItemDisappearingEventArgs e;

			// Token: 0x04000348 RID: 840
			public LiveDataListPage <>4__this;

			// Token: 0x04000349 RID: 841
			private List<LiveDataPIDModel> <visibleCopy>5__2;

			// Token: 0x0400034A RID: 842
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000E5 RID: 229
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemTapped>d__32 : IAsyncStateMachine
		{
			// Token: 0x0600047F RID: 1151 RVA: 0x00042324 File Offset: 0x00040524
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						model = e.ItemData as LiveDataPIDModel;
						liveDataListPage.lv.SelectedItem = null;
						IPID selectedPID = model.SelectedPID;
						if (selectedPID == null || !(selectedPID is CustomPID) || !(selectedPID as CustomPID).IsAction)
						{
							SharedSettings.Current.LiveDataPIDId0 = model.SelectedPID.Id;
							SharedSettings.Current.ChartsVisible = 1;
							liveDataListPage.Navigation.PushAsync(new LiveDataChartPage());
							goto IL_0132;
						}
						taskAwaiter3 = liveDataListPage.DisplayAlert("Execute " + selectedPID.Name + "?", "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, LiveDataListPage.<Lv_ItemTapped>d__32>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult())
					{
						model.Action.Execute(null);
					}
					IL_0132:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					model = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				model = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000480 RID: 1152 RVA: 0x000424BC File Offset: 0x000406BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400034B RID: 843
			public int <>1__state;

			// Token: 0x0400034C RID: 844
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400034D RID: 845
			public ItemTappedEventArgs e;

			// Token: 0x0400034E RID: 846
			public LiveDataListPage <>4__this;

			// Token: 0x0400034F RID: 847
			private LiveDataPIDModel <model>5__2;

			// Token: 0x04000350 RID: 848
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020000E6 RID: 230
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <MenuItemDIsablePID_Clicked>d__33 : IAsyncStateMachine
		{
			// Token: 0x06000481 RID: 1153 RVA: 0x000424CC File Offset: 0x000406CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01C5;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02A3;
					}
					default:
					{
						MenuItem menuItem = (MenuItem)sender;
						model = (LiveDataPIDModel)menuItem.BindingContext;
						IPID selectedPID = model.SelectedPID;
						cpid = selectedPID as CustomPID;
						if (cpid != null && CustomPIDViewModel.CurrentProfile.PidCollection.Contains(cpid))
						{
							taskAwaiter3 = liveDataListPage.DisplayAlert(string.Format(Translate.GetString("ios_RemoveSensorFromList"), cpid.Name), string.Format("You can manage active sensors in Settings -> {0} -> {1} -> {2}", Translate.GetString("Settings_Control_tbConnection.Text"), Translate.GetString("ios_AdvancedConnectionSettings"), Translate.GetString("ios_ManageProfilePids")), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, LiveDataListPage.<MenuItemDIsablePID_Clicked>d__33>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter4 = liveDataListPage.DisplayAlert(Translate.GetString("ios_CantRemoveSensor"), "", "OK").GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<MenuItemDIsablePID_Clicked>d__33>(ref taskAwaiter4, ref this);
								return;
							}
							goto IL_02A3;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_02AA;
					}
					CustomPIDViewModel.CurrentProfile.PidCollection.Remove(cpid);
					if (CustomPIDViewModel.DisabledProfile.Loaded)
					{
						goto IL_01CC;
					}
					taskAwaiter4 = Task.Run(delegate
					{
						CustomPIDViewModel.DisabledProfile.Load();
					}).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<MenuItemDIsablePID_Clicked>d__33>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01C5:
					taskAwaiter4.GetResult();
					IL_01CC:
					CustomPIDViewModel.DisabledProfile.PidCollection.Add(cpid);
					CustomPIDViewModel.CurrentProfile.Save();
					CustomPIDViewModel.DisabledProfile.Save();
					model.Unsubscribe();
					liveDataListPage.ModelsCollection.Remove(model);
					liveDataListPage.searchBar_TextChanged(liveDataListPage.searchBar, new TextChangedEventArgs(liveDataListPage.searchBar.Text, liveDataListPage.searchBar.Text));
					goto IL_02AA;
					IL_02A3:
					taskAwaiter4.GetResult();
					IL_02AA:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					model = null;
					cpid = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				model = null;
				cpid = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000482 RID: 1154 RVA: 0x000427EC File Offset: 0x000409EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000351 RID: 849
			public int <>1__state;

			// Token: 0x04000352 RID: 850
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000353 RID: 851
			public object sender;

			// Token: 0x04000354 RID: 852
			public LiveDataListPage <>4__this;

			// Token: 0x04000355 RID: 853
			private LiveDataPIDModel <model>5__2;

			// Token: 0x04000356 RID: 854
			private CustomPID <cpid>5__3;

			// Token: 0x04000357 RID: 855
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000358 RID: 856
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020000E7 RID: 231
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Page_Appearing>d__20 : IAsyncStateMachine
		{
			// Token: 0x06000483 RID: 1155 RVA: 0x000427FC File Offset: 0x000409FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!SharedSettings.Current.InfoShowed_AllSensors)
						{
							SharedSettings.Current.InfoShowed_AllSensors = true;
							liveDataListPage.btnInfo_Clicked(null, null);
						}
						taskAwaiter = liveDataListPage.InitializeModels().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<Page_Appearing>d__20>(ref taskAwaiter, ref this);
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

			// Token: 0x06000484 RID: 1156 RVA: 0x000428D0 File Offset: 0x00040AD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000359 RID: 857
			public int <>1__state;

			// Token: 0x0400035A RID: 858
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400035B RID: 859
			public LiveDataListPage <>4__this;

			// Token: 0x0400035C RID: 860
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000E8 RID: 232
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetBaseListFilterValue>d__17 : IAsyncStateMachine
		{
			// Token: 0x06000485 RID: 1157 RVA: 0x000428E0 File Offset: 0x00040AE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					LiveDataListPage.BaseListFilters tempValue;
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (liveDataListPage._BaseListFilter == value)
						{
							goto IL_00D0;
						}
						liveDataListPage._BaseListFilter = value;
						tempValue = value;
						liveDataListPage.OnPropertyChanged("BaseListFilter");
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<SetBaseListFilterValue>d__17>(ref taskAwaiter, ref this);
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
					if (liveDataListPage._BaseListFilter == tempValue)
					{
						liveDataListPage.UpdateRequests();
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00D0:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000486 RID: 1158 RVA: 0x000429E0 File Offset: 0x00040BE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400035D RID: 861
			public int <>1__state;

			// Token: 0x0400035E RID: 862
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400035F RID: 863
			public LiveDataListPage <>4__this;

			// Token: 0x04000360 RID: 864
			public LiveDataListPage.BaseListFilters value;

			// Token: 0x04000361 RID: 865
			private LiveDataListPage.BaseListFilters <tempValue>5__2;

			// Token: 0x04000362 RID: 866
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000E9 RID: 233
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetUpdateOnlyVisible>d__9 : IAsyncStateMachine
		{
			// Token: 0x06000487 RID: 1159 RVA: 0x000429F0 File Offset: 0x00040BF0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (liveDataListPage._UpdateOnlyVisible == value)
						{
							goto IL_00D4;
						}
						liveDataListPage._UpdateOnlyVisible = value;
						SharedSettings.Current.LiveDataListPageUpdateOnlyVisible = value;
						liveDataListPage.OnPropertyChanged("UpdateOnlyVisible");
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<SetUpdateOnlyVisible>d__9>(ref taskAwaiter, ref this);
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
					if (liveDataListPage.UpdateOnlyVisible == value)
					{
						liveDataListPage.UpdateRequests();
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00D4:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00042AF4 File Offset: 0x00040CF4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000363 RID: 867
			public int <>1__state;

			// Token: 0x04000364 RID: 868
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000365 RID: 869
			public LiveDataListPage <>4__this;

			// Token: 0x04000366 RID: 870
			public bool value;

			// Token: 0x04000367 RID: 871
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000EA RID: 234
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x06000489 RID: 1161 RVA: 0x00042B04 File Offset: 0x00040D04
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<Page> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<Page>);
								num = (num2 = -1);
								goto IL_0136;
							}
							liveDataListPage.goBackRequested = true;
							taskAwaiter3 = App.OBDReader.ClearRequestQueue().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<btnBack_Clicked>d__10>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter3.GetResult();
						if (SharedSettings.Current.AndroidUseFullscreen && SharedSettings.Current.AndroidUseFullscreen)
						{
							try
							{
								PlatformHelper.DroidService.Window_SetFullscreenOff();
							}
							catch (Exception)
							{
							}
						}
						IEnumerator<LiveDataPIDModel> enumerator = liveDataListPage.ModelsCollection.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								LiveDataPIDModel liveDataPIDModel = enumerator.Current;
								liveDataPIDModel.Unsubscribe();
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						taskAwaiter = liveDataListPage.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, LiveDataListPage.<btnBack_Clicked>d__10>(ref taskAwaiter, ref this);
							return;
						}
						IL_0136:
						taskAwaiter.GetResult();
					}
					catch
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

			// Token: 0x0600048A RID: 1162 RVA: 0x00042CE8 File Offset: 0x00040EE8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000368 RID: 872
			public int <>1__state;

			// Token: 0x04000369 RID: 873
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400036A RID: 874
			public LiveDataListPage <>4__this;

			// Token: 0x0400036B RID: 875
			private TaskAwaiter <>u__1;

			// Token: 0x0400036C RID: 876
			private TaskAwaiter<Page> <>u__2;
		}

		// Token: 0x020000EB RID: 235
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnShowSettings_Clicked>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600048B RID: 1163 RVA: 0x00042CF8 File Offset: 0x00040EF8
			void IAsyncStateMachine.MoveNext()
			{
				LiveDataListPage liveDataListPage = this;
				try
				{
					try
					{
						liveDataListPage.pageSettingsPopupLayout.Show(false);
					}
					catch (Exception)
					{
					}
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

			// Token: 0x0600048C RID: 1164 RVA: 0x00042D68 File Offset: 0x00040F68
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400036D RID: 877
			public int <>1__state;

			// Token: 0x0400036E RID: 878
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400036F RID: 879
			public LiveDataListPage <>4__this;
		}

		// Token: 0x020000EC RID: 236
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <listView_ItemSelected>d__30 : IAsyncStateMachine
		{
			// Token: 0x0600048D RID: 1165 RVA: 0x00042D78 File Offset: 0x00040F78
			void IAsyncStateMachine.MoveNext()
			{
				LiveDataListPage liveDataListPage = this;
				try
				{
					LiveDataPIDModel liveDataPIDModel = (sender as Grid).BindingContext as LiveDataPIDModel;
					IPID selectedPID = liveDataPIDModel.SelectedPID;
					if (selectedPID != null && selectedPID is CustomPID && (selectedPID as CustomPID).IsAction)
					{
						liveDataPIDModel.Action.Execute(null);
					}
					else
					{
						SharedSettings.Current.LiveDataPIDId0 = liveDataPIDModel.SelectedPID.Id;
						SharedSettings.Current.ChartsVisible = 1;
						liveDataListPage.Navigation.PushAsync(new LiveDataChartPage());
					}
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

			// Token: 0x0600048E RID: 1166 RVA: 0x00042E3C File Offset: 0x0004103C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000370 RID: 880
			public int <>1__state;

			// Token: 0x04000371 RID: 881
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000372 RID: 882
			public object sender;

			// Token: 0x04000373 RID: 883
			public LiveDataListPage <>4__this;
		}

		// Token: 0x020000ED RID: 237
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__27 : IAsyncStateMachine
		{
			// Token: 0x0600048F RID: 1167 RVA: 0x00042E4C File Offset: 0x0004104C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataListPage liveDataListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						liveDataListPage.filterText = liveDataListPage.searchBar.Text;
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataListPage.<searchBar_TextChanged>d__27>(ref taskAwaiter, ref this);
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
					string text = liveDataListPage.searchBar.Text;
					if (liveDataListPage.filterText == text)
					{
						try
						{
							IEnumerable<LiveDataPIDModel> filteredModels = liveDataListPage.GetFilteredModels(liveDataListPage.filterText);
							liveDataListPage.lv.ItemsSource = filteredModels;
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

			// Token: 0x06000490 RID: 1168 RVA: 0x00042F60 File Offset: 0x00041160
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000374 RID: 884
			public int <>1__state;

			// Token: 0x04000375 RID: 885
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000376 RID: 886
			public LiveDataListPage <>4__this;

			// Token: 0x04000377 RID: 887
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000EE RID: 238
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_59
		{
			// Token: 0x06000491 RID: 1169 RVA: 0x00042F70 File Offset: 0x00041170
			public <InitializeComponent>_anonXamlCDataTemplate_59()
			{
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x00042F84 File Offset: 0x00041184
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 25);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 26);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 29);
				RadioButton radioButton;
				VisualDiagnostics.RegisterSourceInfo(radioButton = new RadioButton(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 26);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 29);
				RadioButton radioButton2;
				VisualDiagnostics.RegisterSourceInfo(radioButton2 = new RadioButton(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 26);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 22);
				ScrollView scrollView;
				VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(scrollView, nameScope);
				nameScope.RegisterName("rbAll", radioButton);
				if (radioButton.StyleId == null)
				{
					radioButton.StyleId = "rbAll";
				}
				nameScope.RegisterName("rbVisible", radioButton2);
				if (radioButton2.StyleId == null)
				{
					radioButton2.StyleId = "rbVisible";
				}
				scrollView.SetValue(ScrollView.OrientationProperty, 0);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				stackLayout.SetValue(RadioButtonGroup.GroupNameProperty, "UpdateSettings");
				bindingExtension.Mode = 1;
				bindingExtension.Path = "UpdateOnlyVisible";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				stackLayout.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase);
				label.SetValue(Label.TextProperty, "Update readings:");
				stackLayout.Children.Add(label);
				radioButton.SetValue(RadioButton.ContentProperty, "All");
				radioButton.SetValue(RadioButton.GroupNameProperty, "UpdateSettings");
				staticResourceExtension.Key = "FalseValue";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = radioButton;
				array2[1] = stackLayout;
				array2[2] = scrollView;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, RadioButton.ValueProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_59).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(40, 29)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				radioButton.SetValue(RadioButton.ValueProperty, obj2);
				stackLayout.Children.Add(radioButton);
				radioButton2.SetValue(RadioButton.ContentProperty, "Visible");
				radioButton2.SetValue(RadioButton.GroupNameProperty, "UpdateSettings");
				staticResourceExtension2.Key = "TrueValue";
				IMarkupExtension markupExtension2 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = radioButton2;
				array4[1] = stackLayout;
				array4[2] = scrollView;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, RadioButton.ValueProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_59).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 29)));
				object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
				radioButton2.SetValue(RadioButton.ValueProperty, obj4);
				stackLayout.Children.Add(radioButton2);
				scrollView.Content = stackLayout;
				return scrollView;
			}

			// Token: 0x04000378 RID: 888
			internal object[] parentValues;

			// Token: 0x04000379 RID: 889
			internal LiveDataListPage root;
		}

		// Token: 0x020000EF RID: 239
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_60
		{
			// Token: 0x06000493 RID: 1171 RVA: 0x000434D0 File Offset: 0x000416D0
			public <InitializeComponent>_anonXamlCDataTemplate_60()
			{
			}

			// Token: 0x06000494 RID: 1172 RVA: 0x000434E4 File Offset: 0x000416E4
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 26);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 26);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 26);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 22);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 22);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 25);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 25);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 39);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 34);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 34);
				FormattedString formattedString2;
				VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 30);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 22);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 25);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				label.SetValue(Grid.RowProperty, 0);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "SelectedPID.Name";
				bindingExtension.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						IPID selectedPID = A_0.SelectedPID;
						if (selectedPID != null)
						{
							return new ValueTuple<string, bool>(selectedPID.Name, true);
						}
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "SelectedPID"),
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0.SelectedPID, "Name")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.RowProperty, 1);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "TextValue";
				bindingExtension2.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.TextValue, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "TextValue")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span2);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Units";
				bindingExtension3.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Units, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Units")
				});
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span3);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label2);
				label3.SetValue(Grid.RowProperty, 0);
				label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize---";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label3;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_60).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 25)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				bindingExtension4.Mode = 2;
				bindingExtension4.Path = "Settings.ShowPing";
				bindingExtension4.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						SharedSettings settings = A_0.Settings;
						if (settings != null)
						{
							return new ValueTuple<bool, bool>(settings.ShowPing, true);
						}
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Settings"),
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0.Settings, "ShowPing")
				});
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
				label3.SetValue(Label.TextColorProperty, Color.Red);
				label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				bindingExtension5.Mode = 2;
				bindingExtension5.Path = "Ping";
				bindingExtension5.TypedBinding = new TypedBinding<LiveDataPIDModel, long>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<long, bool>(A_0.Ping, true);
					}
					return default(ValueTuple<long, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Ping")
				});
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				span4.SetBinding(Span.TextProperty, bindingBase5);
				formattedString2.Spans.Add(span4);
				span5.SetValue(Span.TextProperty, "ms");
				formattedString2.Spans.Add(span5);
				label3.SetValue(Label.FormattedTextProperty, formattedString2);
				grid.Children.Add(label3);
				frame.SetValue(Grid.RowProperty, 2);
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				dynamicResourceExtension2.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = frame;
				array4[1] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_60).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 25)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource2.Key);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x06000495 RID: 1173 RVA: 0x00043FEC File Offset: 0x000421EC
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1356(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					IPID selectedPID = A_0.SelectedPID;
					if (selectedPID != null)
					{
						return new ValueTuple<string, bool>(selectedPID.Name, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06000496 RID: 1174 RVA: 0x00044024 File Offset: 0x00042224
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1357(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x06000497 RID: 1175 RVA: 0x00044034 File Offset: 0x00042234
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1358(LiveDataPIDModel A_0)
			{
				return A_0.SelectedPID;
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x00044048 File Offset: 0x00042248
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1359(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06000499 RID: 1177 RVA: 0x00044078 File Offset: 0x00042278
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1360(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0600049A RID: 1178 RVA: 0x00044088 File Offset: 0x00042288
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1361(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Units, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x0600049B RID: 1179 RVA: 0x000440B8 File Offset: 0x000422B8
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1362(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0600049C RID: 1180 RVA: 0x000440C8 File Offset: 0x000422C8
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1363(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					SharedSettings settings = A_0.Settings;
					if (settings != null)
					{
						return new ValueTuple<bool, bool>(settings.ShowPing, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x0600049D RID: 1181 RVA: 0x00044100 File Offset: 0x00042300
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1364(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0600049E RID: 1182 RVA: 0x00044110 File Offset: 0x00042310
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1365(LiveDataPIDModel A_0)
			{
				return A_0.Settings;
			}

			// Token: 0x0600049F RID: 1183 RVA: 0x00044124 File Offset: 0x00042324
			[CompilerGenerated]
			private static ValueTuple<long, bool> <LoadDataTemplate>typedBindingsM__1366(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<long, bool>(A_0.Ping, true);
				}
				return default(ValueTuple<long, bool>);
			}

			// Token: 0x060004A0 RID: 1184 RVA: 0x00044154 File Offset: 0x00042354
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1367(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0400037A RID: 890
			internal object[] parentValues;

			// Token: 0x0400037B RID: 891
			internal LiveDataListPage root;
		}

		// Token: 0x020000F0 RID: 240
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_61
		{
			// Token: 0x060004A1 RID: 1185 RVA: 0x00044164 File Offset: 0x00042364
			public <InitializeComponent>_anonXamlCDataTemplate_61()
			{
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x00044178 File Offset: 0x00042378
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 30);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 30);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 30);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 29);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 29);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 26);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 29);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 43);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 38);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 38);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 43);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 38);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 34);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 26);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 29);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\LiveDataListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("1"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension.Key = "BaseFontSize+";
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
				xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_61).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "SelectedPID.Name";
				bindingExtension.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						IPID selectedPID = A_0.SelectedPID;
						if (selectedPID != null)
						{
							return new ValueTuple<string, bool>(selectedPID.Name, true);
						}
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "SelectedPID"),
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0.SelectedPID, "Name")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_61).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 29)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "TextValue";
				bindingExtension2.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.TextValue, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "TextValue")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span2);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Units";
				bindingExtension3.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Units, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
				{
					new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Units")
				});
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span3);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label2);
				frame.SetValue(Grid.RowProperty, 1);
				frame.SetValue(Grid.ColumnProperty, 0);
				frame.SetValue(Grid.ColumnSpanProperty, 2);
				frame.SetValue(View.MarginProperty, new Thickness(0.0, 5.0));
				dynamicResourceExtension3.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array5, 3, num3);
				object[] array6 = array5;
				array6[0] = frame;
				array6[1] = grid;
				array6[2] = viewCell;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Frame.BorderColorProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveDataListPage.<InitializeComponent>_anonXamlCDataTemplate_61).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 29)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource3.Key);
				frame.SetValue(Frame.HasShadowProperty, true);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				grid.Children.Add(frame);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x060004A3 RID: 1187 RVA: 0x00044C50 File Offset: 0x00042E50
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1368(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					IPID selectedPID = A_0.SelectedPID;
					if (selectedPID != null)
					{
						return new ValueTuple<string, bool>(selectedPID.Name, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x060004A4 RID: 1188 RVA: 0x00044C88 File Offset: 0x00042E88
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1369(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x060004A5 RID: 1189 RVA: 0x00044C98 File Offset: 0x00042E98
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1370(LiveDataPIDModel A_0)
			{
				return A_0.SelectedPID;
			}

			// Token: 0x060004A6 RID: 1190 RVA: 0x00044CAC File Offset: 0x00042EAC
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1371(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x060004A7 RID: 1191 RVA: 0x00044CDC File Offset: 0x00042EDC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1372(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x060004A8 RID: 1192 RVA: 0x00044CEC File Offset: 0x00042EEC
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1373(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Units, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x060004A9 RID: 1193 RVA: 0x00044D1C File Offset: 0x00042F1C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1374(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0400037C RID: 892
			internal object[] parentValues;

			// Token: 0x0400037D RID: 893
			internal LiveDataListPage root;
		}
	}
}
