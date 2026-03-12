using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.ListView.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000654 RID: 1620
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\PIDSelectorV3.xaml")]
	public class PIDSelectorV3 : ContentPage, IPIDSelector
	{
		// Token: 0x060037F5 RID: 14325 RVA: 0x002A2BE0 File Offset: 0x002A0DE0
		private PIDSelectorV3(IEnumerable<PID> pids, Func<PID, bool> additionalFilter, IPID preselectedPID = null)
		{
			this.InitializeComponent();
			base.Appearing += this.PIDSelectorV3_Appearing;
			base.Disappearing += this.PIDSelectorV3_Disappearing;
			if (pids == null)
			{
				this.allPids = new List<PID>(App.OBDReader.CurrentCarData.LiveDataPIDs.Count + CustomPIDViewModel.CurrentProfile.PidCollection.Count + CustomPIDViewModel.CurrentCustom.PidCollection.Count);
				this.allPids.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs);
				this.allPids.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
				this.allPids.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
			}
			else
			{
				this.allPids = new List<PID>(pids);
			}
			if (additionalFilter != null)
			{
				this.allPids = new List<PID>(this.allPids.Where((PID x) => additionalFilter(x)));
			}
			this.allPids = this.allPids.OrderBy((PID x) => x.Id).ToList<PID>();
			this.lv.ItemsSourceChangeCachingStrategy = 0;
			DataTemplate dataTemplate;
			DataTemplate dataTemplate2;
			if (Device.Idiom == 2)
			{
				dataTemplate = (DataTemplate)base.Resources["tabletTemplate"];
				dataTemplate2 = (DataTemplate)base.Resources["tabletSelectedTemplate"];
			}
			else
			{
				dataTemplate = (DataTemplate)base.Resources["mobileTemplate"];
				dataTemplate2 = (DataTemplate)base.Resources["mobileSelectedTemplate"];
			}
			this.lv.ItemTemplate = dataTemplate;
			this.lv.SelectedItemTemplate = dataTemplate2;
			this.preselectedPID = preselectedPID;
			base.BindingContext = this;
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x002A2DE2 File Offset: 0x002A0FE2
		private void UpdateQueueDelegate(IEnumerable<LiveDataPIDModel> models)
		{
			PIDSelectorRequestProducer.Delegate = delegate(List<OBDRequest> requests)
			{
				foreach (LiveDataPIDModel liveDataPIDModel in models)
				{
					liveDataPIDModel.GetRequests(requests, null, "");
				}
			};
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x002A2E08 File Offset: 0x002A1008
		private void PIDSelectorV3_Disappearing(object sender, EventArgs e)
		{
			foreach (LiveDataPIDModel liveDataPIDModel in this.ModelsCollection)
			{
				liveDataPIDModel.Unsubscribe();
			}
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x002A2E54 File Offset: 0x002A1054
		private void UpdateFilteredRequests()
		{
			List<LiveDataPIDModel> visiblePIDs = this.VisiblePIDs;
			this.UpdateQueueDelegate(visiblePIDs);
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x002A2E70 File Offset: 0x002A1070
		private void InitializeModels()
		{
			try
			{
				if (this.ModelsCollection.Count == 0)
				{
					this.lv.BatchBegin();
					int count = this.allPids.Count;
					List<LiveDataPIDModel> list = new List<LiveDataPIDModel>(count);
					new List<OBDRequest>(count);
					for (int i = 0; i < count; i++)
					{
						if (this.goBackRequested)
						{
							return;
						}
						try
						{
							LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
							list.Add(liveDataPIDModel);
							liveDataPIDModel.SelectedPID = this.allPids[i];
						}
						catch (Exception)
						{
						}
					}
					foreach (LiveDataPIDModel liveDataPIDModel2 in list)
					{
						this.ModelsCollection.Add(liveDataPIDModel2);
					}
					this.lv.BatchCommit();
				}
				else
				{
					foreach (LiveDataPIDModel liveDataPIDModel3 in this.ModelsCollection)
					{
						liveDataPIDModel3.Subscribe();
					}
					this.UpdateFilteredRequests();
					if (this.goBackRequested)
					{
						return;
					}
				}
			}
			catch (Exception)
			{
			}
			this.activityFrame.IsVisible = false;
			if (this.preselectedPID != null)
			{
				MainThread.BeginInvokeOnMainThread(async delegate
				{
					LiveDataPIDModel liveDataPIDModel4 = (this.lv.ItemsSource as IEnumerable<LiveDataPIDModel>).FirstOrDefault((LiveDataPIDModel x) => x.SelectedPID == this.preselectedPID);
					if (liveDataPIDModel4 != null)
					{
						this.lv.ScrollTo(liveDataPIDModel4, 2, true);
						this.lv.SelectedItem = liveDataPIDModel4;
					}
				});
			}
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x002A300C File Offset: 0x002A120C
		private async void PIDSelectorV3_Appearing(object sender, EventArgs e)
		{
			this.InitializeModels();
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x060037FB RID: 14331 RVA: 0x002A3043 File Offset: 0x002A1243
		public ObservableCollection<LiveDataPIDModel> ModelsCollection
		{
			get
			{
				return this._ModelsCollection;
			}
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x002A304C File Offset: 0x002A124C
		private async void Lv_ItemDisappearing(object sender, ItemDisappearingEventArgs e)
		{
			LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
			if (liveDataPIDModel != null)
			{
				this.VisiblePIDs.Remove(liveDataPIDModel);
			}
			List<LiveDataPIDModel> visibleCopy = new List<LiveDataPIDModel>(this.VisiblePIDs);
			await Task.Delay(500);
			if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, this.VisiblePIDs))
			{
				this.UpdateFilteredRequests();
			}
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x002A308C File Offset: 0x002A128C
		private async void Lv_ItemAppearing(object sender, ItemAppearingEventArgs e)
		{
			LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
			if (liveDataPIDModel != null)
			{
				this.VisiblePIDs.Add(liveDataPIDModel);
			}
			List<LiveDataPIDModel> visibleCopy = new List<LiveDataPIDModel>(this.VisiblePIDs);
			await Task.Delay(500);
			if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, this.VisiblePIDs))
			{
				this.UpdateFilteredRequests();
			}
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x002A30CC File Offset: 0x002A12CC
		private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.UpdateFilter();
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x002A3104 File Offset: 0x002A1304
		private async void UpdateFilter()
		{
			this.filterText = this.searchBar.Text;
			await Task.Delay(1000);
			string text = this.searchBar.Text;
			if (this.filterText == text)
			{
				try
				{
					IEnumerable<LiveDataPIDModel> filteredModels = this.GetFilteredModels(this.filterText);
					this.UpdateQueueDelegate(filteredModels);
					this.lv.ItemsSource = filteredModels;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x002A313C File Offset: 0x002A133C
		private IEnumerable<LiveDataPIDModel> GetFilteredModels(string filter)
		{
			IEnumerable<LiveDataPIDModel> enumerable = this.ModelsCollection;
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

		// Token: 0x06003801 RID: 14337 RVA: 0x002A3204 File Offset: 0x002A1404
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

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06003802 RID: 14338 RVA: 0x002A3262 File Offset: 0x002A1462
		// (set) Token: 0x06003803 RID: 14339 RVA: 0x002A326A File Offset: 0x002A146A
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

		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06003804 RID: 14340 RVA: 0x00017A6F File Offset: 0x00015C6F
		IPID IPIDSelector.SelectedPID
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x002A3274 File Offset: 0x002A1474
		private async void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			(sender as View).IsEnabled = false;
			IPID selectedPID = (e.ItemData as LiveDataPIDModel).SelectedPID;
			this.SelectedPID = selectedPID;
			await base.Navigation.PopAsync(true);
			(sender as View).IsEnabled = true;
			this.ReleaseSemaphore();
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x002A32BC File Offset: 0x002A14BC
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			(sender as VisualElement).IsEnabled = false;
			this.SelectedPID = null;
			await base.Navigation.PopAsync(true);
			this.ReleaseSemaphore();
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x002A32FC File Offset: 0x002A14FC
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

		// Token: 0x06003808 RID: 14344 RVA: 0x002A332C File Offset: 0x002A152C
		private void BeforeGoingBack()
		{
			try
			{
				foreach (LiveDataPIDModel liveDataPIDModel in this.ModelsCollection)
				{
					liveDataPIDModel.Unsubscribe();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x002A3388 File Offset: 0x002A1588
		protected override bool OnBackButtonPressed()
		{
			this.btnCancel_Clicked(this, EventArgs.Empty);
			return true;
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x002A3397 File Offset: 0x002A1597
		public Task WaitSemaphoreAsync()
		{
			return this.semaphore.WaitAsync();
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x002A33A4 File Offset: 0x002A15A4
		public static async Task<IPID> SelectPIDAsync(IPID preSelectedPid = null, Func<PID, bool> additionalFilter = null)
		{
			App.OBDReader.GetQueueCopy();
			PIDSelectorV3 selector;
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				selector = new PIDSelectorV3(null, additionalFilter, preSelectedPid);
			}
			else
			{
				selector = new PIDSelectorV3(LiveDataPIDModel._PIDCollection, additionalFilter, preSelectedPid);
			}
			await App.GetCurrentPage().Navigation.PushAsync(selector);
			await selector.WaitSemaphoreAsync();
			IPID selectedPID = selector.SelectedPID;
			PIDSelectorRequestProducer.Delegate = null;
			RequestProducerStatic.UpdateOBDReaderRequests();
			return selectedPID;
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x002A33F0 File Offset: 0x002A15F0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(PIDSelectorV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/PIDSelectorV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 10);
			OBDReaderConnectedToECUStatusToTrue obdreaderConnectedToECUStatusToTrue;
			VisualDiagnostics.RegisterSourceInfo(obdreaderConnectedToECUStatusToTrue = new OBDReaderConnectedToECUStatusToTrue(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 10);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 10);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 10);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 10);
			DataTemplate dataTemplate4;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate4 = new DataTemplate(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 18);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 17);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 17);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 14);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 17);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 17);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 17);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
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
			nameScope.RegisterName("lv", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lv";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.page = this;
			this.gridButtons = grid;
			this.searchBar = entry;
			this.lv = sfListView;
			this.activityFrame = activityFrame;
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
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
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
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
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 17)));
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
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			nonScalableLabel.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(nonScalableLabel);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			this.Resources.Add("OBDReaderConnectedToECUStatusToTrue", obdreaderConnectedToECUStatusToTrue);
			IDataTemplate dataTemplate5 = dataTemplate;
			PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_72 <InitializeComponent>_anonXamlCDataTemplate_ = new PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_72();
			object[] array6 = new object[0 + 2];
			array6[0] = dataTemplate;
			array6[1] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array6;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			this.Resources.Add("mobileTemplate", dataTemplate);
			IDataTemplate dataTemplate6 = dataTemplate2;
			PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_73 <InitializeComponent>_anonXamlCDataTemplate_2 = new PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_73();
			object[] array7 = new object[0 + 2];
			array7[0] = dataTemplate2;
			array7[1] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array7;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			this.Resources.Add("mobileSelectedTemplate", dataTemplate2);
			IDataTemplate dataTemplate7 = dataTemplate3;
			PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_74 <InitializeComponent>_anonXamlCDataTemplate_3 = new PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_74();
			object[] array8 = new object[0 + 2];
			array8[0] = dataTemplate3;
			array8[1] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array8;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate7.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			this.Resources.Add("tabletTemplate", dataTemplate3);
			IDataTemplate dataTemplate8 = dataTemplate4;
			PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_75 <InitializeComponent>_anonXamlCDataTemplate_4 = new PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_75();
			object[] array9 = new object[0 + 2];
			array9[0] = dataTemplate4;
			array9[1] = this;
			<InitializeComponent>_anonXamlCDataTemplate_4.parentValues = array9;
			<InitializeComponent>_anonXamlCDataTemplate_4.root = this;
			dataTemplate8.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_4.LoadDataTemplate);
			this.Resources.Add("tabletSelectedTemplate", dataTemplate4);
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
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(View.MarginProperty, new Thickness(0.0, 0.0));
			dynamicResourceExtension4.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = entry;
			array10[1] = grid2;
			array10[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array10, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 17)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			entry.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource4.Key);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			dynamicResourceExtension5.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = entry;
			array11[1] = grid2;
			array11[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array11, Entry.PlaceholderColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(262, 17)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			entry.SetDynamicResource(Entry.PlaceholderColorProperty, dynamicResource5.Key);
			entry.TextChanged += this.searchBar_TextChanged;
			dynamicResourceExtension6.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = entry;
			array12[1] = grid2;
			array12[2] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array12, Entry.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(264, 17)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			entry.SetDynamicResource(Entry.TextColorProperty, dynamicResource6.Key);
			grid2.Children.Add(entry);
			sfListView.SetValue(Grid.RowProperty, 2);
			sfListView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.ItemAppearing += new ItemAppearingEventHandler(this.Lv_ItemAppearing);
			sfListView.ItemDisappearing += new ItemDisappearingEventHandler(this.Lv_ItemDisappearing);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.Lv_ItemTapped);
			bindingExtension.Path = "ModelsCollection";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			sfListView.SetBinding(SfListView.ItemsSourceProperty, bindingBase);
			staticResourceExtension.Key = "mobileSelectedTemplate";
			IMarkupExtension markupExtension9 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = sfListView;
			array13[1] = grid2;
			array13[2] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array13, SfListView.SelectedItemTemplateProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(275, 17)));
			object obj12 = markupExtension9.ProvideValue(xamlServiceProvider9);
			sfListView.SelectedItemTemplate = obj12;
			dynamicResourceExtension7.Key = "GreenTextColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = sfListView;
			array14[1] = grid2;
			array14[2] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array14, SfListView.SelectionBackgroundColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(PIDSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(276, 17)));
			DynamicResource dynamicResource7 = markupExtension10.ProvideValue(xamlServiceProvider10);
			sfListView.SetDynamicResource(SfListView.SelectionBackgroundColorProperty, dynamicResource7.Key);
			sfListView.SetValue(SfListView.SelectionModeProperty, 0);
			grid2.Children.Add(sfListView);
			activityFrame.SetValue(Grid.RowProperty, 2);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x002A4FD4 File Offset: 0x002A31D4
		[CompilerGenerated]
		private async void <InitializeModels>b__5_0()
		{
			LiveDataPIDModel liveDataPIDModel = (this.lv.ItemsSource as IEnumerable<LiveDataPIDModel>).FirstOrDefault((LiveDataPIDModel x) => x.SelectedPID == this.preselectedPID);
			if (liveDataPIDModel != null)
			{
				this.lv.ScrollTo(liveDataPIDModel, 2, true);
				this.lv.SelectedItem = liveDataPIDModel;
			}
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x002A500B File Offset: 0x002A320B
		[CompilerGenerated]
		private bool <InitializeModels>b__5_1(LiveDataPIDModel x)
		{
			return x.SelectedPID == this.preselectedPID;
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x002A501C File Offset: 0x002A321C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<PIDSelectorV3>(this, typeof(PIDSelectorV3));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x040021F4 RID: 8692
		private IPID preselectedPID;

		// Token: 0x040021F5 RID: 8693
		private ObservableCollection<LiveDataPIDModel> _ModelsCollection = new ObservableCollection<LiveDataPIDModel>();

		// Token: 0x040021F6 RID: 8694
		private List<LiveDataPIDModel> VisiblePIDs = new List<LiveDataPIDModel>();

		// Token: 0x040021F7 RID: 8695
		private List<PID> allPids;

		// Token: 0x040021F8 RID: 8696
		private string filterText = "";

		// Token: 0x040021F9 RID: 8697
		[CompilerGenerated]
		private IPID <SelectedPID>k__BackingField;

		// Token: 0x040021FA RID: 8698
		private volatile bool goBackRequested;

		// Token: 0x040021FB RID: 8699
		private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

		// Token: 0x040021FC RID: 8700
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x040021FD RID: 8701
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x040021FE RID: 8702
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x040021FF RID: 8703
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x04002200 RID: 8704
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000655 RID: 1621
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<InitializeModels>b__5_0>d : IAsyncStateMachine
		{
			// Token: 0x06003810 RID: 14352 RVA: 0x002A5090 File Offset: 0x002A3290
			void IAsyncStateMachine.MoveNext()
			{
				PIDSelectorV3 pidselectorV = this;
				try
				{
					LiveDataPIDModel liveDataPIDModel = (pidselectorV.lv.ItemsSource as IEnumerable<LiveDataPIDModel>).FirstOrDefault((LiveDataPIDModel x) => x.SelectedPID == pidselectorV.preselectedPID);
					if (liveDataPIDModel != null)
					{
						pidselectorV.lv.ScrollTo(liveDataPIDModel, 2, true);
						pidselectorV.lv.SelectedItem = liveDataPIDModel;
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

			// Token: 0x06003811 RID: 14353 RVA: 0x002A5120 File Offset: 0x002A3320
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002201 RID: 8705
			public int <>1__state;

			// Token: 0x04002202 RID: 8706
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002203 RID: 8707
			public PIDSelectorV3 <>4__this;
		}

		// Token: 0x02000656 RID: 1622
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003812 RID: 14354 RVA: 0x002A512E File Offset: 0x002A332E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003813 RID: 14355 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003814 RID: 14356 RVA: 0x002A211D File Offset: 0x002A031D
			internal int <.ctor>b__0_1(PID x)
			{
				return x.Id;
			}

			// Token: 0x06003815 RID: 14357 RVA: 0x00041D01 File Offset: 0x0003FF01
			internal bool <GetFilteredModels>b__17_1(LiveDataPIDModel x)
			{
				return x.SelectedPID is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x04002204 RID: 8708
			public static readonly PIDSelectorV3.<>c <>9 = new PIDSelectorV3.<>c();

			// Token: 0x04002205 RID: 8709
			public static Func<PID, int> <>9__0_1;

			// Token: 0x04002206 RID: 8710
			public static Func<LiveDataPIDModel, bool> <>9__17_1;
		}

		// Token: 0x02000657 RID: 1623
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06003816 RID: 14358 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06003817 RID: 14359 RVA: 0x002A513A File Offset: 0x002A333A
			internal bool <.ctor>b__0(PID x)
			{
				return this.additionalFilter(x);
			}

			// Token: 0x04002207 RID: 8711
			public Func<PID, bool> additionalFilter;
		}

		// Token: 0x02000658 RID: 1624
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x06003818 RID: 14360 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x06003819 RID: 14361 RVA: 0x002A5148 File Offset: 0x002A3348
			internal bool <GetFilteredModels>b__0(LiveDataPIDModel x)
			{
				return x != null && x.SelectedPID != null && ((!string.IsNullOrEmpty(x.SelectedPID.Name) && x.SelectedPID.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.SelectedPID.ShortName) && x.SelectedPID.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0));
			}

			// Token: 0x04002208 RID: 8712
			public string word;
		}

		// Token: 0x02000659 RID: 1625
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600381A RID: 14362 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x0600381B RID: 14363 RVA: 0x002A51C4 File Offset: 0x002A33C4
			internal void <UpdateQueueDelegate>b__0(List<OBDRequest> requests)
			{
				foreach (LiveDataPIDModel liveDataPIDModel in this.models)
				{
					liveDataPIDModel.GetRequests(requests, null, "");
				}
			}

			// Token: 0x04002209 RID: 8713
			public IEnumerable<LiveDataPIDModel> models;
		}

		// Token: 0x0200065A RID: 1626
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemAppearing>d__13 : IAsyncStateMachine
		{
			// Token: 0x0600381C RID: 14364 RVA: 0x002A5218 File Offset: 0x002A3418
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV3 pidselectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
						if (liveDataPIDModel != null)
						{
							pidselectorV.VisiblePIDs.Add(liveDataPIDModel);
						}
						visibleCopy = new List<LiveDataPIDModel>(pidselectorV.VisiblePIDs);
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV3.<Lv_ItemAppearing>d__13>(ref taskAwaiter, ref this);
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
					if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, pidselectorV.VisiblePIDs))
					{
						pidselectorV.UpdateFilteredRequests();
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					visibleCopy = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				visibleCopy = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600381D RID: 14365 RVA: 0x002A5328 File Offset: 0x002A3528
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400220A RID: 8714
			public int <>1__state;

			// Token: 0x0400220B RID: 8715
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400220C RID: 8716
			public ItemAppearingEventArgs e;

			// Token: 0x0400220D RID: 8717
			public PIDSelectorV3 <>4__this;

			// Token: 0x0400220E RID: 8718
			private List<LiveDataPIDModel> <visibleCopy>5__2;

			// Token: 0x0400220F RID: 8719
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200065B RID: 1627
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemDisappearing>d__12 : IAsyncStateMachine
		{
			// Token: 0x0600381E RID: 14366 RVA: 0x002A5338 File Offset: 0x002A3538
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV3 pidselectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						LiveDataPIDModel liveDataPIDModel = e.ItemData as LiveDataPIDModel;
						if (liveDataPIDModel != null)
						{
							pidselectorV.VisiblePIDs.Remove(liveDataPIDModel);
						}
						visibleCopy = new List<LiveDataPIDModel>(pidselectorV.VisiblePIDs);
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV3.<Lv_ItemDisappearing>d__12>(ref taskAwaiter, ref this);
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
					if (ArrayHelpers.ListEquals<LiveDataPIDModel>(visibleCopy, pidselectorV.VisiblePIDs))
					{
						pidselectorV.UpdateFilteredRequests();
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					visibleCopy = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				visibleCopy = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600381F RID: 14367 RVA: 0x002A544C File Offset: 0x002A364C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002210 RID: 8720
			public int <>1__state;

			// Token: 0x04002211 RID: 8721
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002212 RID: 8722
			public ItemDisappearingEventArgs e;

			// Token: 0x04002213 RID: 8723
			public PIDSelectorV3 <>4__this;

			// Token: 0x04002214 RID: 8724
			private List<LiveDataPIDModel> <visibleCopy>5__2;

			// Token: 0x04002215 RID: 8725
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200065C RID: 1628
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemTapped>d__25 : IAsyncStateMachine
		{
			// Token: 0x06003820 RID: 14368 RVA: 0x002A545C File Offset: 0x002A365C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV3 pidselectorV = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						(sender as View).IsEnabled = false;
						IPID selectedPID = (e.ItemData as LiveDataPIDModel).SelectedPID;
						pidselectorV.SelectedPID = selectedPID;
						taskAwaiter = pidselectorV.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, PIDSelectorV3.<Lv_ItemTapped>d__25>(ref taskAwaiter, ref this);
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

			// Token: 0x06003821 RID: 14369 RVA: 0x002A555C File Offset: 0x002A375C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002216 RID: 8726
			public int <>1__state;

			// Token: 0x04002217 RID: 8727
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002218 RID: 8728
			public object sender;

			// Token: 0x04002219 RID: 8729
			public ItemTappedEventArgs e;

			// Token: 0x0400221A RID: 8730
			public PIDSelectorV3 <>4__this;

			// Token: 0x0400221B RID: 8731
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x0200065D RID: 1629
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PIDSelectorV3_Appearing>d__6 : IAsyncStateMachine
		{
			// Token: 0x06003822 RID: 14370 RVA: 0x002A556C File Offset: 0x002A376C
			void IAsyncStateMachine.MoveNext()
			{
				PIDSelectorV3 pidselectorV = this;
				try
				{
					pidselectorV.InitializeModels();
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

			// Token: 0x06003823 RID: 14371 RVA: 0x002A55C4 File Offset: 0x002A37C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400221C RID: 8732
			public int <>1__state;

			// Token: 0x0400221D RID: 8733
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400221E RID: 8734
			public PIDSelectorV3 <>4__this;
		}

		// Token: 0x0200065E RID: 1630
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectPIDAsync>d__33 : IAsyncStateMachine
		{
			// Token: 0x06003824 RID: 14372 RVA: 0x002A55D4 File Offset: 0x002A37D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				IPID ipid;
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
							goto IL_0121;
						}
						App.OBDReader.GetQueueCopy();
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
						{
							selector = new PIDSelectorV3(null, additionalFilter, preSelectedPid);
						}
						else
						{
							selector = new PIDSelectorV3(LiveDataPIDModel._PIDCollection, additionalFilter, preSelectedPid);
						}
						taskAwaiter = App.GetCurrentPage().Navigation.PushAsync(selector).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV3.<SelectPIDAsync>d__33>(ref taskAwaiter, ref this);
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
					taskAwaiter = selector.WaitSemaphoreAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV3.<SelectPIDAsync>d__33>(ref taskAwaiter, ref this);
						return;
					}
					IL_0121:
					taskAwaiter.GetResult();
					IPID selectedPID = selector.SelectedPID;
					PIDSelectorRequestProducer.Delegate = null;
					RequestProducerStatic.UpdateOBDReaderRequests();
					ipid = selectedPID;
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
				this.<>t__builder.SetResult(ipid);
			}

			// Token: 0x06003825 RID: 14373 RVA: 0x002A5778 File Offset: 0x002A3978
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400221F RID: 8735
			public int <>1__state;

			// Token: 0x04002220 RID: 8736
			public AsyncTaskMethodBuilder<IPID> <>t__builder;

			// Token: 0x04002221 RID: 8737
			public Func<PID, bool> additionalFilter;

			// Token: 0x04002222 RID: 8738
			public IPID preSelectedPid;

			// Token: 0x04002223 RID: 8739
			private PIDSelectorV3 <selector>5__2;

			// Token: 0x04002224 RID: 8740
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200065F RID: 1631
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateFilter>d__16 : IAsyncStateMachine
		{
			// Token: 0x06003826 RID: 14374 RVA: 0x002A5788 File Offset: 0x002A3988
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV3 pidselectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						pidselectorV.filterText = pidselectorV.searchBar.Text;
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV3.<UpdateFilter>d__16>(ref taskAwaiter, ref this);
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
					string text = pidselectorV.searchBar.Text;
					if (pidselectorV.filterText == text)
					{
						try
						{
							IEnumerable<LiveDataPIDModel> filteredModels = pidselectorV.GetFilteredModels(pidselectorV.filterText);
							pidselectorV.UpdateQueueDelegate(filteredModels);
							pidselectorV.lv.ItemsSource = filteredModels;
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

			// Token: 0x06003827 RID: 14375 RVA: 0x002A58A4 File Offset: 0x002A3AA4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002225 RID: 8741
			public int <>1__state;

			// Token: 0x04002226 RID: 8742
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002227 RID: 8743
			public PIDSelectorV3 <>4__this;

			// Token: 0x04002228 RID: 8744
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000660 RID: 1632
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__26 : IAsyncStateMachine
		{
			// Token: 0x06003828 RID: 14376 RVA: 0x002A58B4 File Offset: 0x002A3AB4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV3 pidselectorV = this;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, PIDSelectorV3.<btnCancel_Clicked>d__26>(ref taskAwaiter, ref this);
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

			// Token: 0x06003829 RID: 14377 RVA: 0x002A598C File Offset: 0x002A3B8C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002229 RID: 8745
			public int <>1__state;

			// Token: 0x0400222A RID: 8746
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400222B RID: 8747
			public object sender;

			// Token: 0x0400222C RID: 8748
			public PIDSelectorV3 <>4__this;

			// Token: 0x0400222D RID: 8749
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000661 RID: 1633
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__15 : IAsyncStateMachine
		{
			// Token: 0x0600382A RID: 14378 RVA: 0x002A599C File Offset: 0x002A3B9C
			void IAsyncStateMachine.MoveNext()
			{
				PIDSelectorV3 pidselectorV = this;
				try
				{
					pidselectorV.UpdateFilter();
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

			// Token: 0x0600382B RID: 14379 RVA: 0x002A59F4 File Offset: 0x002A3BF4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400222E RID: 8750
			public int <>1__state;

			// Token: 0x0400222F RID: 8751
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002230 RID: 8752
			public PIDSelectorV3 <>4__this;
		}

		// Token: 0x02000662 RID: 1634
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_72
		{
			// Token: 0x0600382C RID: 14380 RVA: 0x002A5A04 File Offset: 0x002A3C04
			public <InitializeComponent>_anonXamlCDataTemplate_72()
			{
			}

			// Token: 0x0600382D RID: 14381 RVA: 0x002A5A18 File Offset: 0x002A3C18
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 22);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 22);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 21);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 21);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 18);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 21);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 35);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 30);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 30);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 35);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 30);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 26);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 29);
				OBDDataReader obdreader;
				VisualDiagnostics.RegisterSourceInfo(obdreader = App.OBDReader, new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 29);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 26);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 21);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 18);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 14);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				grid.SetValue(Grid.RowSpacingProperty, 0.0);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				label.SetValue(Grid.RowProperty, 0);
				dynamicResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_72).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 21)));
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
				label2.SetValue(Grid.RowProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_72).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 21)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.MaxLinesProperty, 2);
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
				staticResourceExtension.Key = "OBDReaderConnectedToECUStatusToTrue";
				IMarkupExtension markupExtension3 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array5, 3, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension4;
				array6[1] = label2;
				array6[2] = grid;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_72).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 29)));
				object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension4.Converter = obj4;
				bindingExtension4.Mode = 4;
				bindingExtension4.Path = "CurrentStatus";
				bindingExtension4.Source = obdreader;
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
				grid.Children.Add(label2);
				frame.SetValue(Grid.RowProperty, 2);
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				dynamicResourceExtension3.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array7, 2, num4);
				object[] array8 = array7;
				array8[0] = frame;
				array8[1] = grid;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver4.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_72).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 21)));
				DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource3.Key);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x0600382E RID: 14382 RVA: 0x002A66B4 File Offset: 0x002A48B4
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1382(LiveDataPIDModel A_0)
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

			// Token: 0x0600382F RID: 14383 RVA: 0x002A66EC File Offset: 0x002A48EC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1383(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x06003830 RID: 14384 RVA: 0x002A66FC File Offset: 0x002A48FC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1384(LiveDataPIDModel A_0)
			{
				return A_0.SelectedPID;
			}

			// Token: 0x06003831 RID: 14385 RVA: 0x002A6710 File Offset: 0x002A4910
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1385(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06003832 RID: 14386 RVA: 0x002A6740 File Offset: 0x002A4940
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1386(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x06003833 RID: 14387 RVA: 0x002A6750 File Offset: 0x002A4950
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1387(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Units, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06003834 RID: 14388 RVA: 0x002A6780 File Offset: 0x002A4980
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1388(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x04002231 RID: 8753
			internal object[] parentValues;

			// Token: 0x04002232 RID: 8754
			internal PIDSelectorV3 root;
		}

		// Token: 0x02000663 RID: 1635
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_73
		{
			// Token: 0x06003835 RID: 14389 RVA: 0x002A6790 File Offset: 0x002A4990
			public <InitializeComponent>_anonXamlCDataTemplate_73()
			{
			}

			// Token: 0x06003836 RID: 14390 RVA: 0x002A67A4 File Offset: 0x002A49A4
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 19);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 22);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 22);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 22);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 21);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 21);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 18);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 21);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 35);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 30);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 30);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 35);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 30);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 26);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 29);
				OBDDataReader obdreader;
				VisualDiagnostics.RegisterSourceInfo(obdreader = App.OBDReader, new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 29);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 26);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 18);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 21);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 18);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 14);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				dynamicResourceExtension.Key = "ButtonGreenColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_73).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 19)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				grid.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
				grid.SetValue(Grid.RowSpacingProperty, 0.0);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				label.SetValue(Grid.RowProperty, 0);
				dynamicResourceExtension2.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_73).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 21)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
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
				label.SetValue(Label.TextColorProperty, Color.White);
				grid.Children.Add(label);
				label2.SetValue(Grid.RowProperty, 1);
				dynamicResourceExtension3.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array5, 2, num3);
				object[] array6 = array5;
				array6[0] = label2;
				array6[1] = grid;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_73).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(124, 21)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.MaxLinesProperty, 2);
				label2.SetValue(Label.TextColorProperty, Color.White);
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
				staticResourceExtension.Key = "OBDReaderConnectedToECUStatusToTrue";
				IMarkupExtension markupExtension4 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array7, 3, num4);
				object[] array8 = array7;
				array8[0] = bindingExtension4;
				array8[1] = label2;
				array8[2] = grid;
				object obj4;
				xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver4.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_73).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 29)));
				object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
				bindingExtension4.Converter = obj5;
				bindingExtension4.Mode = 4;
				bindingExtension4.Path = "CurrentStatus";
				bindingExtension4.Source = obdreader;
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
				grid.Children.Add(label2);
				frame.SetValue(Grid.RowProperty, 2);
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				dynamicResourceExtension4.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array9, 2, num5);
				object[] array10 = array9;
				array10[0] = frame;
				array10[1] = grid;
				object obj6;
				xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array10, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver5.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_73).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 21)));
				DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource4.Key);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x06003837 RID: 14391 RVA: 0x002A7634 File Offset: 0x002A5834
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1389(LiveDataPIDModel A_0)
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

			// Token: 0x06003838 RID: 14392 RVA: 0x002A766C File Offset: 0x002A586C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1390(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x06003839 RID: 14393 RVA: 0x002A767C File Offset: 0x002A587C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1391(LiveDataPIDModel A_0)
			{
				return A_0.SelectedPID;
			}

			// Token: 0x0600383A RID: 14394 RVA: 0x002A7690 File Offset: 0x002A5890
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1392(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x0600383B RID: 14395 RVA: 0x002A76C0 File Offset: 0x002A58C0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1393(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0600383C RID: 14396 RVA: 0x002A76D0 File Offset: 0x002A58D0
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1394(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Units, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x0600383D RID: 14397 RVA: 0x002A7700 File Offset: 0x002A5900
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1395(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x04002233 RID: 8755
			internal object[] parentValues;

			// Token: 0x04002234 RID: 8756
			internal PIDSelectorV3 root;
		}

		// Token: 0x02000664 RID: 1636
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_74
		{
			// Token: 0x0600383E RID: 14398 RVA: 0x002A7710 File Offset: 0x002A5910
			public <InitializeComponent>_anonXamlCDataTemplate_74()
			{
			}

			// Token: 0x0600383F RID: 14399 RVA: 0x002A7724 File Offset: 0x002A5924
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 26);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 26);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 26);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 26);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 25);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 25);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 22);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 25);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 22);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 25);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 18);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 14);
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
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_74).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 25)));
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
				dynamicResourceExtension2.Key = "BaseFontSize";
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
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_74).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 25)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				label2.SetValue(Label.MaxLinesProperty, 2);
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
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_74).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 25)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource3.Key);
				frame.SetValue(Frame.HasShadowProperty, true);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				grid.Children.Add(frame);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x06003840 RID: 14400 RVA: 0x002A8280 File Offset: 0x002A6480
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1396(LiveDataPIDModel A_0)
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

			// Token: 0x06003841 RID: 14401 RVA: 0x002A82B8 File Offset: 0x002A64B8
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1397(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x06003842 RID: 14402 RVA: 0x002A82C8 File Offset: 0x002A64C8
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1398(LiveDataPIDModel A_0)
			{
				return A_0.SelectedPID;
			}

			// Token: 0x06003843 RID: 14403 RVA: 0x002A82DC File Offset: 0x002A64DC
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1399(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06003844 RID: 14404 RVA: 0x002A830C File Offset: 0x002A650C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1400(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x06003845 RID: 14405 RVA: 0x002A831C File Offset: 0x002A651C
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1401(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Units, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06003846 RID: 14406 RVA: 0x002A834C File Offset: 0x002A654C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1402(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x04002235 RID: 8757
			internal object[] parentValues;

			// Token: 0x04002236 RID: 8758
			internal PIDSelectorV3 root;
		}

		// Token: 0x02000665 RID: 1637
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_75
		{
			// Token: 0x06003847 RID: 14407 RVA: 0x002A835C File Offset: 0x002A655C
			public <InitializeComponent>_anonXamlCDataTemplate_75()
			{
			}

			// Token: 0x06003848 RID: 14408 RVA: 0x002A8370 File Offset: 0x002A6570
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 23);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 26);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 26);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 26);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 26);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 25);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 25);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 22);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 25);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 223, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 22);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 25);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 18);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\PIDSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 14);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				dynamicResourceExtension.Key = "ButtonGreenColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = grid;
				array2[1] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_75).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(198, 23)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				grid.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("1"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension2.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_75).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(209, 25)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
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
				label.SetValue(Label.TextColorProperty, Color.White);
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 1);
				dynamicResourceExtension3.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array5, 3, num3);
				object[] array6 = array5;
				array6[0] = label2;
				array6[1] = grid;
				array6[2] = viewCell;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_75).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(214, 25)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				label2.SetValue(Label.MaxLinesProperty, 2);
				label2.SetValue(Label.TextColorProperty, Color.White);
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
				dynamicResourceExtension4.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array7, 3, num4);
				object[] array8 = array7;
				array8[0] = frame;
				array8[1] = grid;
				array8[2] = viewCell;
				object obj4;
				xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array8, Frame.BorderColorProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver4.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(PIDSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_75).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(233, 25)));
				DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
				frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource4.Key);
				frame.SetValue(Frame.HasShadowProperty, true);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				grid.Children.Add(frame);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x06003849 RID: 14409 RVA: 0x002A90A4 File Offset: 0x002A72A4
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1403(LiveDataPIDModel A_0)
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

			// Token: 0x0600384A RID: 14410 RVA: 0x002A90DC File Offset: 0x002A72DC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1404(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0600384B RID: 14411 RVA: 0x002A90EC File Offset: 0x002A72EC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1405(LiveDataPIDModel A_0)
			{
				return A_0.SelectedPID;
			}

			// Token: 0x0600384C RID: 14412 RVA: 0x002A9100 File Offset: 0x002A7300
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1406(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x0600384D RID: 14413 RVA: 0x002A9130 File Offset: 0x002A7330
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1407(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x0600384E RID: 14414 RVA: 0x002A9140 File Offset: 0x002A7340
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1408(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Units, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x0600384F RID: 14415 RVA: 0x002A9170 File Offset: 0x002A7370
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1409(LiveDataPIDModel A_0)
			{
				return A_0;
			}

			// Token: 0x04002237 RID: 8759
			internal object[] parentValues;

			// Token: 0x04002238 RID: 8760
			internal PIDSelectorV3 root;
		}
	}
}
