using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.Settings.SettingsV3;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000604 RID: 1540
	[XamlFilePath("DriveCycles\\FuelStatisticsPage.xaml")]
	public class FuelStatisticsPage : ContentPage
	{
		// Token: 0x06003670 RID: 13936 RVA: 0x00279658 File Offset: 0x00277858
		public FuelStatisticsPage()
		{
			this.InitializeComponent();
			if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidChartRenderingSafeMode)
			{
				this.seriesAvgFuel.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
				this.seriesAvgSpeed.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
				this.seriesDistance.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
				this.seriesFuelPrice.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
				this.seriesFuelUsed.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
				this.seriesMotorHours.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
			}
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_delete"], delegate
			{
				this.ResetModelQuestion();
			}, 0, 0));
			ToolbarItem toolbarItem = new ToolbarItem("", (string)Application.Current.Resources["NB_settings"], delegate
			{
				if (PlatformHelper.IsiOS && App.UseLegacyUI)
				{
					base.Navigation.PushAsync(new SettingsFuelRatePage());
					return;
				}
				base.Navigation.PushAsync(new SettingsFuelRateV3());
			}, 0, 0);
			base.ToolbarItems.Add(toolbarItem);
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["InfoImageNavigationBarTextColor"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			this.gridInfoRow1.BindingContext = this.model;
			this.gridInfoRow2.BindingContext = this.model;
			this.panelDays.BindingContext = this.model;
			this.chart.BindingContext = null;
			this.FullRefreshModel();
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x00279854 File Offset: 0x00277A54
		private async void StartUpdateCycles()
		{
			if (!this.updater_started)
			{
				this.updater_started = true;
				while (Application.Current.MainPage.Navigation.NavigationStack.Contains(this))
				{
					await Task.Delay(TimeSpan.FromSeconds(5.0));
					DriveCycleViewModel.Current.RefreshPeriodForStatisticsScreenWithoutRebuildingDays();
				}
				this.updater_started = false;
			}
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x0027988C File Offset: 0x00277A8C
		private void btnDays_Clicked(object sender, EventArgs e)
		{
			foreach (ChartSeries chartSeries in this.chart.Series)
			{
				chartSeries.ItemsSource = this.model.DayCycles;
			}
			this.xaxis.IntervalType = 1;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x002798F4 File Offset: 0x00277AF4
		private void btnHours_Clicked(object sender, EventArgs e)
		{
			foreach (ChartSeries chartSeries in this.chart.Series)
			{
				chartSeries.ItemsSource = this.model.DriveCycles;
			}
			this.xaxis.IntervalType = 2;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_ActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
		{
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x0027995C File Offset: 0x00277B5C
		private async void ResetModelQuestion()
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ResetStatsTitle"), "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					this.model.Reset();
					this.model.PeriodAllTime.Execute(null);
				}
			}
			else
			{
				base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK");
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x00279993 File Offset: 0x00277B93
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_FuelStatisticsPage.Text"), Translate.GetString("ios_FuelStatisticsInfo"), "OK");
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x002799B8 File Offset: 0x00277BB8
		private async void btnCustomPeriod_Clicked(object sender, EventArgs e)
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					await base.Navigation.PushAsync(InAppManager.GetInAppPage());
				}
			}
			else
			{
				FuelStatisticsDatePeriodSelectorPage fuelStatisticsDatePeriodSelectorPage = new FuelStatisticsDatePeriodSelectorPage(this.model);
				base.Navigation.PushAsync(fuelStatisticsDatePeriodSelectorPage);
			}
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x002799EF File Offset: 0x00277BEF
		private void Handle_Appearing(object sender, EventArgs e)
		{
			FuelStatisticsPage.Instance = this;
			MainAppRequestProducer.Delegate = new AddRequestsDelegate(this.AddRequestsDelegate);
			RequestProducerStatic.UpdateOBDReaderRequests();
			this.StartUpdateCycles();
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x00279A14 File Offset: 0x00277C14
		private void AddRequestsDelegate(List<OBDRequest> requests)
		{
			PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption);
			if (pid != null)
			{
				LiveDataPIDModel.GetRequests(pid, requests, null, "");
			}
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x00279A5C File Offset: 0x00277C5C
		private async void FullRefreshModel()
		{
			this.activityFrame.IsVisible = true;
			this.model.PeriodAllTime.Execute(null);
			this.chart.BindingContext = this.model;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x00279A93 File Offset: 0x00277C93
		private void OnProgressChanged(object sender, KeyValuePair<int, int> e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = string.Format(Translate.GetString("ios_Loading"), e.Key, e.Value);
			});
		}

		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x00279AB8 File Offset: 0x00277CB8
		private DriveCycleViewModel model
		{
			get
			{
				return DriveCycleViewModel.Current;
			}
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x00279ABF File Offset: 0x00277CBF
		private void Page_Disappearing(object sender, EventArgs e)
		{
			FuelStatisticsPage.Instance = null;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x00279AC8 File Offset: 0x00277CC8
		private void boxAvgFuel_Tapped(object sender, EventArgs e)
		{
			BoxView boxView = (BoxView)sender;
			if (boxView == this.boxAvgFuelConsumption)
			{
				this.seriesAvgFuel.IsVisible = !this.seriesAvgFuel.IsVisible;
				return;
			}
			if (boxView == this.boxAvgSpeed)
			{
				this.seriesAvgSpeed.IsVisible = !this.seriesAvgSpeed.IsVisible;
				return;
			}
			if (boxView == this.boxTotalFuelUsed)
			{
				this.seriesFuelUsed.IsVisible = !this.seriesFuelUsed.IsVisible;
				return;
			}
			if (boxView == this.boxTotalDistance)
			{
				this.seriesDistance.IsVisible = !this.seriesDistance.IsVisible;
				return;
			}
			if (boxView == this.boxTotalFuelPrice)
			{
				this.seriesFuelPrice.IsVisible = !this.seriesFuelPrice.IsVisible;
				return;
			}
			if (boxView == this.boxMotorHours)
			{
				this.seriesMotorHours.IsVisible = !this.seriesMotorHours.IsVisible;
			}
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x00279BB0 File Offset: 0x00277DB0
		private async void btnDCList_Clicked(object sender, EventArgs e)
		{
			if (SharedSettings.Current.AdsProductPurchased)
			{
				DriveCyclesListPage driveCyclesListPage = new DriveCyclesListPage(this.model);
				await base.Navigation.PushAsync(driveCyclesListPage);
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					await base.Navigation.PushAsync(InAppManager.GetInAppPage());
				}
			}
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000027D4 File Offset: 0x000009D4
		private void btn_AllTime_Clicked(object sender, EventArgs e)
		{
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x00279BE8 File Offset: 0x00277DE8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(FuelStatisticsPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DriveCycles/FuelStatisticsPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 26);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 26);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 26);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 26);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 26);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 26);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 26);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 30);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 30);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 30);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 30);
			RowDefinition rowDefinition12;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition12 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 30);
			RowDefinition rowDefinition13;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition13 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 30);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 34);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 26);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 29);
			BoxView boxView2;
			VisualDiagnostics.RegisterSourceInfo(boxView2 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 26);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 29);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 26);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 29);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 29);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 26);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 29);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 26);
			TapGestureRecognizer tapGestureRecognizer2;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 34);
			BoxView boxView3;
			VisualDiagnostics.RegisterSourceInfo(boxView3 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 26);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 29);
			BoxView boxView4;
			VisualDiagnostics.RegisterSourceInfo(boxView4 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 26);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 29);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 29);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 26);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 29);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 26);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 29);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 29);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 26);
			TapGestureRecognizer tapGestureRecognizer3;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer3 = new TapGestureRecognizer(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 34);
			BoxView boxView5;
			VisualDiagnostics.RegisterSourceInfo(boxView5 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 223, 26);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 29);
			BoxView boxView6;
			VisualDiagnostics.RegisterSourceInfo(boxView6 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 26);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 29);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 29);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 26);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 29);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 29);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 26);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 29);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 29);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 22);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 282, 30);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 30);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 284, 30);
			RowDefinition rowDefinition14;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition14 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 30);
			RowDefinition rowDefinition15;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition15 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 30);
			RowDefinition rowDefinition16;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition16 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 30);
			TapGestureRecognizer tapGestureRecognizer4;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer4 = new TapGestureRecognizer(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 302, 34);
			BoxView boxView7;
			VisualDiagnostics.RegisterSourceInfo(boxView7 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 293, 26);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 311, 29);
			BoxView boxView8;
			VisualDiagnostics.RegisterSourceInfo(boxView8 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 26);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 29);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 322, 29);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 26);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 328, 29);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 332, 29);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 323, 26);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 29);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 341, 29);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 26);
			TapGestureRecognizer tapGestureRecognizer5;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer5 = new TapGestureRecognizer(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 34);
			BoxView boxView9;
			VisualDiagnostics.RegisterSourceInfo(boxView9 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 26);
			DynamicResourceExtension dynamicResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension18 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 362, 29);
			BoxView boxView10;
			VisualDiagnostics.RegisterSourceInfo(boxView10 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 356, 26);
			DynamicResourceExtension dynamicResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension19 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 369, 29);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 373, 29);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 365, 26);
			DynamicResourceExtension dynamicResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension20 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 379, 29);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 383, 29);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 374, 26);
			DynamicResourceExtension dynamicResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension21 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 388, 29);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 392, 29);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 384, 26);
			TapGestureRecognizer tapGestureRecognizer6;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer6 = new TapGestureRecognizer(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 404, 34);
			BoxView boxView11;
			VisualDiagnostics.RegisterSourceInfo(boxView11 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 395, 26);
			DynamicResourceExtension dynamicResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension22 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 413, 29);
			BoxView boxView12;
			VisualDiagnostics.RegisterSourceInfo(boxView12 = new BoxView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 407, 26);
			DynamicResourceExtension dynamicResourceExtension23;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension23 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 420, 29);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 29);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 416, 26);
			DynamicResourceExtension dynamicResourceExtension24;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension24 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 29);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 434, 29);
			Label label17;
			VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 425, 26);
			DynamicResourceExtension dynamicResourceExtension25;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension25 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 439, 29);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 443, 29);
			Label label18;
			VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 435, 26);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 277, 22);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 469, 30);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 470, 30);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 472, 26);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 477, 26);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 22);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 487, 33);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 488, 33);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 30);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 489, 71);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 489, 30);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 37);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 69);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 30);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 37);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 69);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 30);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 37);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 70);
			Button button7;
			VisualDiagnostics.RegisterSourceInfo(button7 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 30);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 37);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 70);
			Button button8;
			VisualDiagnostics.RegisterSourceInfo(button8 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 30);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 494, 37);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 494, 70);
			Button button9;
			VisualDiagnostics.RegisterSourceInfo(button9 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 494, 30);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 495, 37);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 495, 71);
			Button button10;
			VisualDiagnostics.RegisterSourceInfo(button10 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 495, 30);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 496, 37);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 496, 71);
			Button button11;
			VisualDiagnostics.RegisterSourceInfo(button11 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 496, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 484, 26);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 483, 22);
			DynamicResourceExtension dynamicResourceExtension26;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension26 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 504, 25);
			DynamicResourceExtension dynamicResourceExtension27;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension27 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 505, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 513, 33);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 514, 33);
			DynamicResourceExtension dynamicResourceExtension28;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension28 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 517, 64);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 517, 38);
			DynamicResourceExtension dynamicResourceExtension29;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension29 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 520, 59);
			ChartLineStyle chartLineStyle;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle = new ChartLineStyle(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 520, 38);
			DateTimeAxis dateTimeAxis;
			VisualDiagnostics.RegisterSourceInfo(dateTimeAxis = new DateTimeAxis(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 509, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 527, 33);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 528, 33);
			DynamicResourceExtension dynamicResourceExtension30;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension30 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 531, 64);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 531, 38);
			DynamicResourceExtension dynamicResourceExtension31;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension31 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 534, 59);
			ChartLineStyle chartLineStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle2 = new ChartLineStyle(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 534, 38);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 526, 30);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 554, 33);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 555, 33);
			FastLineSeries fastLineSeries;
			VisualDiagnostics.RegisterSourceInfo(fastLineSeries = new FastLineSeries(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 549, 30);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 575, 33);
			FastLineSeries fastLineSeries2;
			VisualDiagnostics.RegisterSourceInfo(fastLineSeries2 = new FastLineSeries(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 570, 30);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 587, 33);
			FastLineSeries fastLineSeries3;
			VisualDiagnostics.RegisterSourceInfo(fastLineSeries3 = new FastLineSeries(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 582, 30);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 599, 33);
			FastLineSeries fastLineSeries4;
			VisualDiagnostics.RegisterSourceInfo(fastLineSeries4 = new FastLineSeries(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 594, 30);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 611, 33);
			FastLineSeries fastLineSeries5;
			VisualDiagnostics.RegisterSourceInfo(fastLineSeries5 = new FastLineSeries(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 606, 30);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 623, 33);
			FastLineSeries fastLineSeries6;
			VisualDiagnostics.RegisterSourceInfo(fastLineSeries6 = new FastLineSeries(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 618, 30);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 632, 30);
			ChartTrackballBehavior chartTrackballBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartTrackballBehavior = new ChartTrackballBehavior(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 638, 30);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 501, 22);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 654, 25);
			Button button12;
			VisualDiagnostics.RegisterSourceInfo(button12 = new Button(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 649, 22);
			DynamicResourceExtension dynamicResourceExtension32;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension32 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 657, 25);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 658, 25);
			Label label19;
			VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 655, 22);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 659, 22);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 18);
			ScrollView scrollView2;
			VisualDiagnostics.RegisterSourceInfo(scrollView2 = new ScrollView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 687, 14);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DriveCycles\\FuelStatisticsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridInfoRow1", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridInfoRow1";
			}
			nameScope.RegisterName("boxAvgFuelConsumption", boxView);
			if (boxView.StyleId == null)
			{
				boxView.StyleId = "boxAvgFuelConsumption";
			}
			nameScope.RegisterName("boxAvgSpeed", boxView3);
			if (boxView3.StyleId == null)
			{
				boxView3.StyleId = "boxAvgSpeed";
			}
			nameScope.RegisterName("boxMotorHours", boxView5);
			if (boxView5.StyleId == null)
			{
				boxView5.StyleId = "boxMotorHours";
			}
			nameScope.RegisterName("gridInfoRow2", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridInfoRow2";
			}
			nameScope.RegisterName("boxTotalDistance", boxView7);
			if (boxView7.StyleId == null)
			{
				boxView7.StyleId = "boxTotalDistance";
			}
			nameScope.RegisterName("boxTotalFuelUsed", boxView9);
			if (boxView9.StyleId == null)
			{
				boxView9.StyleId = "boxTotalFuelUsed";
			}
			nameScope.RegisterName("boxTotalFuelPrice", boxView11);
			if (boxView11.StyleId == null)
			{
				boxView11.StyleId = "boxTotalFuelPrice";
			}
			nameScope.RegisterName("btnDays", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnDays";
			}
			nameScope.RegisterName("btnHours", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnHours";
			}
			nameScope.RegisterName("panelDays", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelDays";
			}
			nameScope.RegisterName("chart", sfChart);
			if (sfChart.StyleId == null)
			{
				sfChart.StyleId = "chart";
			}
			nameScope.RegisterName("xaxis", dateTimeAxis);
			if (dateTimeAxis.StyleId == null)
			{
				dateTimeAxis.StyleId = "xaxis";
			}
			nameScope.RegisterName("seriesAvgFuel", fastLineSeries);
			if (fastLineSeries.StyleId == null)
			{
				fastLineSeries.StyleId = "seriesAvgFuel";
			}
			nameScope.RegisterName("seriesAvgSpeed", fastLineSeries2);
			if (fastLineSeries2.StyleId == null)
			{
				fastLineSeries2.StyleId = "seriesAvgSpeed";
			}
			nameScope.RegisterName("seriesFuelUsed", fastLineSeries3);
			if (fastLineSeries3.StyleId == null)
			{
				fastLineSeries3.StyleId = "seriesFuelUsed";
			}
			nameScope.RegisterName("seriesDistance", fastLineSeries4);
			if (fastLineSeries4.StyleId == null)
			{
				fastLineSeries4.StyleId = "seriesDistance";
			}
			nameScope.RegisterName("seriesFuelPrice", fastLineSeries5);
			if (fastLineSeries5.StyleId == null)
			{
				fastLineSeries5.StyleId = "seriesFuelPrice";
			}
			nameScope.RegisterName("seriesMotorHours", fastLineSeries6);
			if (fastLineSeries6.StyleId == null)
			{
				fastLineSeries6.StyleId = "seriesMotorHours";
			}
			nameScope.RegisterName("btnDCList", button12);
			if (button12.StyleId == null)
			{
				button12.StyleId = "btnDCList";
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
			this.gridInfoRow1 = grid;
			this.boxAvgFuelConsumption = boxView;
			this.boxAvgSpeed = boxView3;
			this.boxMotorHours = boxView5;
			this.gridInfoRow2 = grid2;
			this.boxTotalDistance = boxView7;
			this.boxTotalFuelUsed = boxView9;
			this.boxTotalFuelPrice = boxView11;
			this.btnDays = button;
			this.btnHours = button2;
			this.panelDays = stackLayout;
			this.chart = sfChart;
			this.xaxis = dateTimeAxis;
			this.seriesAvgFuel = fastLineSeries;
			this.seriesAvgSpeed = fastLineSeries2;
			this.seriesFuelUsed = fastLineSeries3;
			this.seriesDistance = fastLineSeries4;
			this.seriesFuelPrice = fastLineSeries5;
			this.seriesMotorHours = fastLineSeries6;
			this.btnDCList = button12;
			this.activityFrame = activityFrame;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			translate.Text = "ios_FuelStatisticsPage.Text";
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
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
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
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid5.SetValue(View.MarginProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			scrollView2.SetValue(Grid.RowProperty, 0);
			scrollView2.SetValue(ScrollView.OrientationProperty, 0);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(View.MarginProperty, new Thickness(0.0, 2.0, 0.0, 0.0));
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			rowDefinition12.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition12);
			rowDefinition13.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition13);
			boxView.SetValue(Grid.RowProperty, 0);
			boxView.SetValue(Grid.RowSpanProperty, 3);
			boxView.SetValue(Grid.ColumnProperty, 0);
			boxView.SetValue(Grid.ColumnSpanProperty, 1);
			boxView.SetValue(VisualElement.BackgroundColorProperty, new Color(0.3960784375667572, 0.6000000238418579, 1.0, 1.0));
			boxView.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.boxAvgFuel_Tapped;
			boxView.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(boxView);
			boxView2.SetValue(Grid.RowProperty, 0);
			boxView2.SetValue(Grid.RowSpanProperty, 3);
			boxView2.SetValue(Grid.ColumnProperty, 0);
			boxView2.SetValue(Grid.ColumnSpanProperty, 1);
			boxView2.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 6];
			array3[0] = boxView2;
			array3[1] = grid;
			array3[2] = grid4;
			array3[3] = scrollView2;
			array3[4] = grid5;
			array3[5] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			boxView2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			boxView2.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			boxView2.SetValue(VisualElement.InputTransparentProperty, true);
			grid.Children.Add(boxView2);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Grid.ColumnProperty, 0);
			label.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension3.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = label;
			array4[1] = grid;
			array4[2] = grid4;
			array4[3] = scrollView2;
			array4[4] = grid5;
			array4[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 29)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(VisualElement.InputTransparentProperty, true);
			translate2.Text = "PID_AvgFuelConsumption_Short";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = label;
			array5[1] = grid;
			array5[2] = grid4;
			array5[3] = scrollView2;
			array5[4] = grid5;
			array5[5] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(151, 29)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.Text = obj7;
			grid.Children.Add(label);
			label2.SetValue(Grid.RowProperty, 1);
			label2.SetValue(Grid.ColumnProperty, 0);
			label2.SetValue(View.MarginProperty, new Thickness(5.0));
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension4.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = label2;
			array6[1] = grid;
			array6[2] = grid4;
			array6[3] = scrollView2;
			array6[4] = grid5;
			array6[5] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(157, 29)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label2.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension.Path = "PeriodAvgFuelConsumption";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase);
			grid.Children.Add(label2);
			label3.SetValue(Grid.RowProperty, 2);
			label3.SetValue(Grid.ColumnProperty, 0);
			label3.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension5.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = label3;
			array7[1] = grid;
			array7[2] = grid4;
			array7[3] = scrollView2;
			array7[4] = grid5;
			array7[5] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Label.FontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(166, 29)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label3.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension2.Path = "FuelConsumptionsUnits";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase2);
			grid.Children.Add(label3);
			boxView3.SetValue(Grid.RowProperty, 0);
			boxView3.SetValue(Grid.RowSpanProperty, 3);
			boxView3.SetValue(Grid.ColumnProperty, 1);
			boxView3.SetValue(Grid.ColumnSpanProperty, 1);
			boxView3.SetValue(VisualElement.BackgroundColorProperty, new Color(0.6000000238418579, 0.6000000238418579, 0.6000000238418579, 1.0));
			boxView3.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			tapGestureRecognizer2.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer2.Tapped += this.boxAvgFuel_Tapped;
			boxView3.GestureRecognizers.Add(tapGestureRecognizer2);
			grid.Children.Add(boxView3);
			boxView4.SetValue(Grid.RowProperty, 0);
			boxView4.SetValue(Grid.RowSpanProperty, 3);
			boxView4.SetValue(Grid.ColumnProperty, 1);
			boxView4.SetValue(Grid.ColumnSpanProperty, 1);
			boxView4.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension6.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = boxView4;
			array8[1] = grid;
			array8[2] = grid4;
			array8[3] = scrollView2;
			array8[4] = grid5;
			array8[5] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(191, 29)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			boxView4.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			boxView4.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			boxView4.SetValue(VisualElement.InputTransparentProperty, true);
			grid.Children.Add(boxView4);
			label4.SetValue(Grid.RowProperty, 0);
			label4.SetValue(Grid.ColumnProperty, 1);
			label4.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension7.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = label4;
			array9[1] = grid;
			array9[2] = grid4;
			array9[3] = scrollView2;
			array9[4] = grid5;
			array9[5] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(198, 29)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label4.SetValue(VisualElement.InputTransparentProperty, true);
			translate3.Text = "PID_CalculatedAvgSpeed";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = label4;
			array10[1] = grid;
			array10[2] = grid4;
			array10[3] = scrollView2;
			array10[4] = grid5;
			array10[5] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(202, 29)));
			object obj13 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label4.Text = obj13;
			grid.Children.Add(label4);
			label5.SetValue(Grid.RowProperty, 1);
			label5.SetValue(Grid.ColumnProperty, 1);
			label5.SetValue(View.MarginProperty, new Thickness(5.0));
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension8.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = label5;
			array11[1] = grid;
			array11[2] = grid4;
			array11[3] = scrollView2;
			array11[4] = grid5;
			array11[5] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array11, Label.FontSizeProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(208, 29)));
			DynamicResource dynamicResource8 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
			label5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label5.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension3.Path = "PeriodAvgSpeed";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label5.SetBinding(Label.TextProperty, bindingBase3);
			grid.Children.Add(label5);
			label6.SetValue(Grid.RowProperty, 2);
			label6.SetValue(Grid.ColumnProperty, 1);
			label6.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension9.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = label6;
			array12[1] = grid;
			array12[2] = grid4;
			array12[3] = scrollView2;
			array12[4] = grid5;
			array12[5] = this;
			object obj15;
			xamlServiceProvider12.Add(typeFromHandle23, obj15 = new SimpleValueTargetProvider(array12, Label.FontSizeProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(217, 29)));
			DynamicResource dynamicResource9 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource9.Key);
			label6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label6.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label6.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension4.Path = "SpeedUnits";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label6.SetBinding(Label.TextProperty, bindingBase4);
			grid.Children.Add(label6);
			boxView5.SetValue(Grid.RowProperty, 0);
			boxView5.SetValue(Grid.RowSpanProperty, 3);
			boxView5.SetValue(Grid.ColumnProperty, 2);
			boxView5.SetValue(Grid.ColumnSpanProperty, 1);
			boxView5.SetValue(VisualElement.BackgroundColorProperty, new Color(0.9254902005195618, 0.8352941274642944, 0.07450980693101883, 1.0));
			boxView5.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			tapGestureRecognizer3.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer3.Tapped += this.boxAvgFuel_Tapped;
			boxView5.GestureRecognizers.Add(tapGestureRecognizer3);
			grid.Children.Add(boxView5);
			boxView6.SetValue(Grid.RowProperty, 0);
			boxView6.SetValue(Grid.RowSpanProperty, 3);
			boxView6.SetValue(Grid.ColumnProperty, 2);
			boxView6.SetValue(Grid.ColumnSpanProperty, 1);
			boxView6.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension10.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = boxView6;
			array13[1] = grid;
			array13[2] = grid4;
			array13[3] = scrollView2;
			array13[4] = grid5;
			array13[5] = this;
			object obj16;
			xamlServiceProvider13.Add(typeFromHandle25, obj16 = new SimpleValueTargetProvider(array13, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(241, 29)));
			DynamicResource dynamicResource10 = markupExtension13.ProvideValue(xamlServiceProvider13);
			boxView6.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource10.Key);
			boxView6.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			boxView6.SetValue(VisualElement.InputTransparentProperty, true);
			grid.Children.Add(boxView6);
			label7.SetValue(Grid.RowProperty, 0);
			label7.SetValue(Grid.ColumnProperty, 2);
			label7.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension11.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = label7;
			array14[1] = grid;
			array14[2] = grid4;
			array14[3] = scrollView2;
			array14[4] = grid5;
			array14[5] = this;
			object obj17;
			xamlServiceProvider14.Add(typeFromHandle27, obj17 = new SimpleValueTargetProvider(array14, Label.FontSizeProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(248, 29)));
			DynamicResource dynamicResource11 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label7.SetDynamicResource(Label.FontSizeProperty, dynamicResource11.Key);
			label7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label7.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label7.SetValue(VisualElement.InputTransparentProperty, true);
			translate4.Text = "ios_MotorHours";
			IMarkupExtension markupExtension15 = translate4;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = label7;
			array15[1] = grid;
			array15[2] = grid4;
			array15[3] = scrollView2;
			array15[4] = grid5;
			array15[5] = this;
			object obj18;
			xamlServiceProvider15.Add(typeFromHandle29, obj18 = new SimpleValueTargetProvider(array15, Label.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(252, 29)));
			object obj19 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label7.Text = obj19;
			grid.Children.Add(label7);
			label8.SetValue(Grid.RowProperty, 1);
			label8.SetValue(Grid.ColumnProperty, 2);
			label8.SetValue(View.MarginProperty, new Thickness(5.0));
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension12.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 6];
			array16[0] = label8;
			array16[1] = grid;
			array16[2] = grid4;
			array16[3] = scrollView2;
			array16[4] = grid5;
			array16[5] = this;
			object obj20;
			xamlServiceProvider16.Add(typeFromHandle31, obj20 = new SimpleValueTargetProvider(array16, Label.FontSizeProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 29)));
			DynamicResource dynamicResource12 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label8.SetDynamicResource(Label.FontSizeProperty, dynamicResource12.Key);
			label8.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label8.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label8.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension5.Path = "PeriodMotorHours";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label8.SetBinding(Label.TextProperty, bindingBase5);
			grid.Children.Add(label8);
			label9.SetValue(Grid.RowProperty, 2);
			label9.SetValue(Grid.ColumnProperty, 2);
			label9.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension13.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = label9;
			array17[1] = grid;
			array17[2] = grid4;
			array17[3] = scrollView2;
			array17[4] = grid5;
			array17[5] = this;
			object obj21;
			xamlServiceProvider17.Add(typeFromHandle33, obj21 = new SimpleValueTargetProvider(array17, Label.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(267, 29)));
			DynamicResource dynamicResource13 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label9.SetDynamicResource(Label.FontSizeProperty, dynamicResource13.Key);
			label9.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label9.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label9.SetValue(VisualElement.InputTransparentProperty, true);
			translate5.Text = "ios_MotorHours_Units";
			IMarkupExtension markupExtension18 = translate5;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 6];
			array18[0] = label9;
			array18[1] = grid;
			array18[2] = grid4;
			array18[3] = scrollView2;
			array18[4] = grid5;
			array18[5] = this;
			object obj22;
			xamlServiceProvider18.Add(typeFromHandle35, obj22 = new SimpleValueTargetProvider(array18, Label.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(271, 29)));
			object obj23 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label9.Text = obj23;
			grid.Children.Add(label9);
			grid4.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 1);
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			rowDefinition14.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition14);
			rowDefinition15.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition15);
			rowDefinition16.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition16);
			boxView7.SetValue(Grid.RowProperty, 0);
			boxView7.SetValue(Grid.RowSpanProperty, 3);
			boxView7.SetValue(Grid.ColumnProperty, 0);
			boxView7.SetValue(Grid.ColumnSpanProperty, 1);
			boxView7.SetValue(VisualElement.BackgroundColorProperty, new Color(1.0, 0.40392157435417175, 0.4000000059604645, 1.0));
			boxView7.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			tapGestureRecognizer4.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer4.Tapped += this.boxAvgFuel_Tapped;
			boxView7.GestureRecognizers.Add(tapGestureRecognizer4);
			grid2.Children.Add(boxView7);
			boxView8.SetValue(Grid.RowProperty, 0);
			boxView8.SetValue(Grid.RowSpanProperty, 3);
			boxView8.SetValue(Grid.ColumnProperty, 0);
			boxView8.SetValue(Grid.ColumnSpanProperty, 1);
			boxView8.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension14.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 6];
			array19[0] = boxView8;
			array19[1] = grid2;
			array19[2] = grid4;
			array19[3] = scrollView2;
			array19[4] = grid5;
			array19[5] = this;
			object obj24;
			xamlServiceProvider19.Add(typeFromHandle37, obj24 = new SimpleValueTargetProvider(array19, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(311, 29)));
			DynamicResource dynamicResource14 = markupExtension19.ProvideValue(xamlServiceProvider19);
			boxView8.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource14.Key);
			boxView8.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			boxView8.SetValue(VisualElement.InputTransparentProperty, true);
			grid2.Children.Add(boxView8);
			label10.SetValue(Grid.RowProperty, 0);
			label10.SetValue(Grid.ColumnProperty, 0);
			label10.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension15.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension20 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 6];
			array20[0] = label10;
			array20[1] = grid2;
			array20[2] = grid4;
			array20[3] = scrollView2;
			array20[4] = grid5;
			array20[5] = this;
			object obj25;
			xamlServiceProvider20.Add(typeFromHandle39, obj25 = new SimpleValueTargetProvider(array20, Label.FontSizeProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(318, 29)));
			DynamicResource dynamicResource15 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource15.Key);
			label10.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label10.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label10.SetValue(VisualElement.InputTransparentProperty, true);
			translate6.Text = "PID_TotalDistance";
			IMarkupExtension markupExtension21 = translate6;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 6];
			array21[0] = label10;
			array21[1] = grid2;
			array21[2] = grid4;
			array21[3] = scrollView2;
			array21[4] = grid5;
			array21[5] = this;
			object obj26;
			xamlServiceProvider21.Add(typeFromHandle41, obj26 = new SimpleValueTargetProvider(array21, Label.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(322, 29)));
			object obj27 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label10.Text = obj27;
			grid2.Children.Add(label10);
			label11.SetValue(Grid.RowProperty, 1);
			label11.SetValue(Grid.ColumnProperty, 0);
			label11.SetValue(View.MarginProperty, new Thickness(5.0));
			label11.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension16.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 6];
			array22[0] = label11;
			array22[1] = grid2;
			array22[2] = grid4;
			array22[3] = scrollView2;
			array22[4] = grid5;
			array22[5] = this;
			object obj28;
			xamlServiceProvider22.Add(typeFromHandle43, obj28 = new SimpleValueTargetProvider(array22, Label.FontSizeProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(328, 29)));
			DynamicResource dynamicResource16 = markupExtension22.ProvideValue(xamlServiceProvider22);
			label11.SetDynamicResource(Label.FontSizeProperty, dynamicResource16.Key);
			label11.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label11.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label11.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension6.Path = "PeriodDistance";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label11.SetBinding(Label.TextProperty, bindingBase6);
			grid2.Children.Add(label11);
			label12.SetValue(Grid.RowProperty, 2);
			label12.SetValue(Grid.ColumnProperty, 0);
			label12.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension17.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 6];
			array23[0] = label12;
			array23[1] = grid2;
			array23[2] = grid4;
			array23[3] = scrollView2;
			array23[4] = grid5;
			array23[5] = this;
			object obj29;
			xamlServiceProvider23.Add(typeFromHandle45, obj29 = new SimpleValueTargetProvider(array23, Label.FontSizeProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(337, 29)));
			DynamicResource dynamicResource17 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label12.SetDynamicResource(Label.FontSizeProperty, dynamicResource17.Key);
			label12.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label12.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label12.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension7.Path = "DistanceUnits";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label12.SetBinding(Label.TextProperty, bindingBase7);
			grid2.Children.Add(label12);
			boxView9.SetValue(Grid.RowProperty, 0);
			boxView9.SetValue(Grid.RowSpanProperty, 3);
			boxView9.SetValue(Grid.ColumnProperty, 1);
			boxView9.SetValue(Grid.ColumnSpanProperty, 1);
			boxView9.SetValue(VisualElement.BackgroundColorProperty, new Color(0.6000000238418579, 0.800000011920929, 0.40392157435417175, 1.0));
			boxView9.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			tapGestureRecognizer5.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer5.Tapped += this.boxAvgFuel_Tapped;
			boxView9.GestureRecognizers.Add(tapGestureRecognizer5);
			grid2.Children.Add(boxView9);
			boxView10.SetValue(Grid.RowProperty, 0);
			boxView10.SetValue(Grid.RowSpanProperty, 3);
			boxView10.SetValue(Grid.ColumnProperty, 1);
			boxView10.SetValue(Grid.ColumnSpanProperty, 1);
			boxView10.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension18.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension24 = dynamicResourceExtension18;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 6];
			array24[0] = boxView10;
			array24[1] = grid2;
			array24[2] = grid4;
			array24[3] = scrollView2;
			array24[4] = grid5;
			array24[5] = this;
			object obj30;
			xamlServiceProvider24.Add(typeFromHandle47, obj30 = new SimpleValueTargetProvider(array24, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(362, 29)));
			DynamicResource dynamicResource18 = markupExtension24.ProvideValue(xamlServiceProvider24);
			boxView10.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource18.Key);
			boxView10.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			boxView10.SetValue(VisualElement.InputTransparentProperty, true);
			grid2.Children.Add(boxView10);
			label13.SetValue(Grid.RowProperty, 0);
			label13.SetValue(Grid.ColumnProperty, 1);
			label13.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension19.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension19;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 6];
			array25[0] = label13;
			array25[1] = grid2;
			array25[2] = grid4;
			array25[3] = scrollView2;
			array25[4] = grid5;
			array25[5] = this;
			object obj31;
			xamlServiceProvider25.Add(typeFromHandle49, obj31 = new SimpleValueTargetProvider(array25, Label.FontSizeProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(369, 29)));
			DynamicResource dynamicResource19 = markupExtension25.ProvideValue(xamlServiceProvider25);
			label13.SetDynamicResource(Label.FontSizeProperty, dynamicResource19.Key);
			label13.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label13.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label13.SetValue(VisualElement.InputTransparentProperty, true);
			translate7.Text = "PID_TotalFuelUsed";
			IMarkupExtension markupExtension26 = translate7;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = label13;
			array26[1] = grid2;
			array26[2] = grid4;
			array26[3] = scrollView2;
			array26[4] = grid5;
			array26[5] = this;
			object obj32;
			xamlServiceProvider26.Add(typeFromHandle51, obj32 = new SimpleValueTargetProvider(array26, Label.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(373, 29)));
			object obj33 = markupExtension26.ProvideValue(xamlServiceProvider26);
			label13.Text = obj33;
			grid2.Children.Add(label13);
			label14.SetValue(Grid.RowProperty, 1);
			label14.SetValue(Grid.ColumnProperty, 1);
			label14.SetValue(View.MarginProperty, new Thickness(5.0));
			label14.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension20.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension27 = dynamicResourceExtension20;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 6];
			array27[0] = label14;
			array27[1] = grid2;
			array27[2] = grid4;
			array27[3] = scrollView2;
			array27[4] = grid5;
			array27[5] = this;
			object obj34;
			xamlServiceProvider27.Add(typeFromHandle53, obj34 = new SimpleValueTargetProvider(array27, Label.FontSizeProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(379, 29)));
			DynamicResource dynamicResource20 = markupExtension27.ProvideValue(xamlServiceProvider27);
			label14.SetDynamicResource(Label.FontSizeProperty, dynamicResource20.Key);
			label14.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label14.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label14.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension8.Path = "PeriodFuelUsed";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label14.SetBinding(Label.TextProperty, bindingBase8);
			grid2.Children.Add(label14);
			label15.SetValue(Grid.RowProperty, 2);
			label15.SetValue(Grid.ColumnProperty, 1);
			label15.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension21.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension21;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 6];
			array28[0] = label15;
			array28[1] = grid2;
			array28[2] = grid4;
			array28[3] = scrollView2;
			array28[4] = grid5;
			array28[5] = this;
			object obj35;
			xamlServiceProvider28.Add(typeFromHandle55, obj35 = new SimpleValueTargetProvider(array28, Label.FontSizeProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(388, 29)));
			DynamicResource dynamicResource21 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label15.SetDynamicResource(Label.FontSizeProperty, dynamicResource21.Key);
			label15.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label15.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label15.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension9.Path = "FuelUsedUnits";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label15.SetBinding(Label.TextProperty, bindingBase9);
			grid2.Children.Add(label15);
			boxView11.SetValue(Grid.RowProperty, 0);
			boxView11.SetValue(Grid.RowSpanProperty, 3);
			boxView11.SetValue(Grid.ColumnProperty, 2);
			boxView11.SetValue(Grid.ColumnSpanProperty, 1);
			boxView11.SetValue(VisualElement.BackgroundColorProperty, new Color(0.6000000238418579, 0.4000000059604645, 0.7960784435272217, 1.0));
			boxView11.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			tapGestureRecognizer6.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer6.Tapped += this.boxAvgFuel_Tapped;
			boxView11.GestureRecognizers.Add(tapGestureRecognizer6);
			grid2.Children.Add(boxView11);
			boxView12.SetValue(Grid.RowProperty, 0);
			boxView12.SetValue(Grid.RowSpanProperty, 3);
			boxView12.SetValue(Grid.ColumnProperty, 2);
			boxView12.SetValue(Grid.ColumnSpanProperty, 1);
			boxView12.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension22.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension22;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 6];
			array29[0] = boxView12;
			array29[1] = grid2;
			array29[2] = grid4;
			array29[3] = scrollView2;
			array29[4] = grid5;
			array29[5] = this;
			object obj36;
			xamlServiceProvider29.Add(typeFromHandle57, obj36 = new SimpleValueTargetProvider(array29, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(413, 29)));
			DynamicResource dynamicResource22 = markupExtension29.ProvideValue(xamlServiceProvider29);
			boxView12.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource22.Key);
			boxView12.SetValue(BoxView.CornerRadiusProperty, new CornerRadiusTypeConverter().ConvertFromInvariantString("9"));
			boxView12.SetValue(VisualElement.InputTransparentProperty, true);
			grid2.Children.Add(boxView12);
			label16.SetValue(Grid.RowProperty, 0);
			label16.SetValue(Grid.ColumnProperty, 2);
			label16.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension23.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension30 = dynamicResourceExtension23;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 6];
			array30[0] = label16;
			array30[1] = grid2;
			array30[2] = grid4;
			array30[3] = scrollView2;
			array30[4] = grid5;
			array30[5] = this;
			object obj37;
			xamlServiceProvider30.Add(typeFromHandle59, obj37 = new SimpleValueTargetProvider(array30, Label.FontSizeProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(420, 29)));
			DynamicResource dynamicResource23 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label16.SetDynamicResource(Label.FontSizeProperty, dynamicResource23.Key);
			label16.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label16.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label16.SetValue(VisualElement.InputTransparentProperty, true);
			translate8.Text = "PID_FuelMoney";
			IMarkupExtension markupExtension31 = translate8;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = label16;
			array31[1] = grid2;
			array31[2] = grid4;
			array31[3] = scrollView2;
			array31[4] = grid5;
			array31[5] = this;
			object obj38;
			xamlServiceProvider31.Add(typeFromHandle61, obj38 = new SimpleValueTargetProvider(array31, Label.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(424, 29)));
			object obj39 = markupExtension31.ProvideValue(xamlServiceProvider31);
			label16.Text = obj39;
			grid2.Children.Add(label16);
			label17.SetValue(Grid.RowProperty, 1);
			label17.SetValue(Grid.ColumnProperty, 2);
			label17.SetValue(View.MarginProperty, new Thickness(5.0));
			label17.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension24.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension32 = dynamicResourceExtension24;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 6];
			array32[0] = label17;
			array32[1] = grid2;
			array32[2] = grid4;
			array32[3] = scrollView2;
			array32[4] = grid5;
			array32[5] = this;
			object obj40;
			xamlServiceProvider32.Add(typeFromHandle63, obj40 = new SimpleValueTargetProvider(array32, Label.FontSizeProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(430, 29)));
			DynamicResource dynamicResource24 = markupExtension32.ProvideValue(xamlServiceProvider32);
			label17.SetDynamicResource(Label.FontSizeProperty, dynamicResource24.Key);
			label17.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label17.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label17.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension10.Path = "PeriodFuelPrice";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label17.SetBinding(Label.TextProperty, bindingBase10);
			grid2.Children.Add(label17);
			label18.SetValue(Grid.RowProperty, 2);
			label18.SetValue(Grid.ColumnProperty, 2);
			label18.SetValue(View.MarginProperty, new Thickness(5.0));
			dynamicResourceExtension25.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension33 = dynamicResourceExtension25;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 6];
			array33[0] = label18;
			array33[1] = grid2;
			array33[2] = grid4;
			array33[3] = scrollView2;
			array33[4] = grid5;
			array33[5] = this;
			object obj41;
			xamlServiceProvider33.Add(typeFromHandle65, obj41 = new SimpleValueTargetProvider(array33, Label.FontSizeProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(439, 29)));
			DynamicResource dynamicResource25 = markupExtension33.ProvideValue(xamlServiceProvider33);
			label18.SetDynamicResource(Label.FontSizeProperty, dynamicResource25.Key);
			label18.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label18.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label18.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension11.Path = "Currency";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label18.SetBinding(Label.TextProperty, bindingBase11);
			grid2.Children.Add(label18);
			grid4.Children.Add(grid2);
			grid3.SetValue(Grid.RowProperty, 2);
			grid3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.btnDays_Clicked;
			button.SetValue(Button.TextProperty, "Days");
			grid3.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnHours_Clicked;
			button2.SetValue(Button.TextProperty, "Hours");
			grid3.Children.Add(button2);
			grid4.Children.Add(grid3);
			scrollView.SetValue(Grid.RowProperty, 3);
			scrollView.SetValue(ScrollView.OrientationProperty, 1);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			button3.Clicked += this.btn_AllTime_Clicked;
			bindingExtension12.Path = "PeriodAllTime";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			button3.SetBinding(Button.CommandProperty, bindingBase12);
			translate9.Text = "ios_AllTime";
			IMarkupExtension markupExtension34 = translate9;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 7];
			array34[0] = button3;
			array34[1] = stackLayout;
			array34[2] = scrollView;
			array34[3] = grid4;
			array34[4] = scrollView2;
			array34[5] = grid5;
			array34[6] = this;
			object obj42;
			xamlServiceProvider34.Add(typeFromHandle67, obj42 = new SimpleValueTargetProvider(array34, Button.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(488, 33)));
			object obj43 = markupExtension34.ProvideValue(xamlServiceProvider34);
			button3.Text = obj43;
			stackLayout.Children.Add(button3);
			button4.Clicked += this.btnCustomPeriod_Clicked;
			translate10.Text = "ios_CustomDays";
			IMarkupExtension markupExtension35 = translate10;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 7];
			array35[0] = button4;
			array35[1] = stackLayout;
			array35[2] = scrollView;
			array35[3] = grid4;
			array35[4] = scrollView2;
			array35[5] = grid5;
			array35[6] = this;
			object obj44;
			xamlServiceProvider35.Add(typeFromHandle69, obj44 = new SimpleValueTargetProvider(array35, Button.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(489, 71)));
			object obj45 = markupExtension35.ProvideValue(xamlServiceProvider35);
			button4.Text = obj45;
			stackLayout.Children.Add(button4);
			bindingExtension13.Path = "PeriodToday";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			button5.SetBinding(Button.CommandProperty, bindingBase13);
			translate11.Text = "ios_Today";
			IMarkupExtension markupExtension36 = translate11;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 7];
			array36[0] = button5;
			array36[1] = stackLayout;
			array36[2] = scrollView;
			array36[3] = grid4;
			array36[4] = scrollView2;
			array36[5] = grid5;
			array36[6] = this;
			object obj46;
			xamlServiceProvider36.Add(typeFromHandle71, obj46 = new SimpleValueTargetProvider(array36, Button.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(490, 69)));
			object obj47 = markupExtension36.ProvideValue(xamlServiceProvider36);
			button5.Text = obj47;
			stackLayout.Children.Add(button5);
			bindingExtension14.Path = "Period7Days";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			button6.SetBinding(Button.CommandProperty, bindingBase14);
			translate12.Text = "ios_7Days";
			IMarkupExtension markupExtension37 = translate12;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 7];
			array37[0] = button6;
			array37[1] = stackLayout;
			array37[2] = scrollView;
			array37[3] = grid4;
			array37[4] = scrollView2;
			array37[5] = grid5;
			array37[6] = this;
			object obj48;
			xamlServiceProvider37.Add(typeFromHandle73, obj48 = new SimpleValueTargetProvider(array37, Button.TextProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(491, 69)));
			object obj49 = markupExtension37.ProvideValue(xamlServiceProvider37);
			button6.Text = obj49;
			stackLayout.Children.Add(button6);
			bindingExtension15.Path = "Period14Days";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			button7.SetBinding(Button.CommandProperty, bindingBase15);
			translate13.Text = "ios_14Days";
			IMarkupExtension markupExtension38 = translate13;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 7];
			array38[0] = button7;
			array38[1] = stackLayout;
			array38[2] = scrollView;
			array38[3] = grid4;
			array38[4] = scrollView2;
			array38[5] = grid5;
			array38[6] = this;
			object obj50;
			xamlServiceProvider38.Add(typeFromHandle75, obj50 = new SimpleValueTargetProvider(array38, Button.TextProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(492, 70)));
			object obj51 = markupExtension38.ProvideValue(xamlServiceProvider38);
			button7.Text = obj51;
			stackLayout.Children.Add(button7);
			bindingExtension16.Path = "Period30Days";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			button8.SetBinding(Button.CommandProperty, bindingBase16);
			translate14.Text = "ios_30Days";
			IMarkupExtension markupExtension39 = translate14;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 7];
			array39[0] = button8;
			array39[1] = stackLayout;
			array39[2] = scrollView;
			array39[3] = grid4;
			array39[4] = scrollView2;
			array39[5] = grid5;
			array39[6] = this;
			object obj52;
			xamlServiceProvider39.Add(typeFromHandle77, obj52 = new SimpleValueTargetProvider(array39, Button.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(493, 70)));
			object obj53 = markupExtension39.ProvideValue(xamlServiceProvider39);
			button8.Text = obj53;
			stackLayout.Children.Add(button8);
			bindingExtension17.Path = "Period90Days";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			button9.SetBinding(Button.CommandProperty, bindingBase17);
			translate15.Text = "ios_90Days";
			IMarkupExtension markupExtension40 = translate15;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 7];
			array40[0] = button9;
			array40[1] = stackLayout;
			array40[2] = scrollView;
			array40[3] = grid4;
			array40[4] = scrollView2;
			array40[5] = grid5;
			array40[6] = this;
			object obj54;
			xamlServiceProvider40.Add(typeFromHandle79, obj54 = new SimpleValueTargetProvider(array40, Button.TextProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(494, 70)));
			object obj55 = markupExtension40.ProvideValue(xamlServiceProvider40);
			button9.Text = obj55;
			stackLayout.Children.Add(button9);
			bindingExtension18.Path = "Period180Days";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			button10.SetBinding(Button.CommandProperty, bindingBase18);
			translate16.Text = "ios_180Days";
			IMarkupExtension markupExtension41 = translate16;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 7];
			array41[0] = button10;
			array41[1] = stackLayout;
			array41[2] = scrollView;
			array41[3] = grid4;
			array41[4] = scrollView2;
			array41[5] = grid5;
			array41[6] = this;
			object obj56;
			xamlServiceProvider41.Add(typeFromHandle81, obj56 = new SimpleValueTargetProvider(array41, Button.TextProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(495, 71)));
			object obj57 = markupExtension41.ProvideValue(xamlServiceProvider41);
			button10.Text = obj57;
			stackLayout.Children.Add(button10);
			bindingExtension19.Path = "Period360Days";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			button11.SetBinding(Button.CommandProperty, bindingBase19);
			translate17.Text = "ios_360Days";
			IMarkupExtension markupExtension42 = translate17;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 7];
			array42[0] = button11;
			array42[1] = stackLayout;
			array42[2] = scrollView;
			array42[3] = grid4;
			array42[4] = scrollView2;
			array42[5] = grid5;
			array42[6] = this;
			object obj58;
			xamlServiceProvider42.Add(typeFromHandle83, obj58 = new SimpleValueTargetProvider(array42, Button.TextProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(496, 71)));
			object obj59 = markupExtension42.ProvideValue(xamlServiceProvider42);
			button11.Text = obj59;
			stackLayout.Children.Add(button11);
			scrollView.Content = stackLayout;
			grid4.Children.Add(scrollView);
			sfChart.SetValue(Grid.RowProperty, 4);
			dynamicResourceExtension26.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension43 = dynamicResourceExtension26;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 5];
			array43[0] = sfChart;
			array43[1] = grid4;
			array43[2] = scrollView2;
			array43[3] = grid5;
			array43[4] = this;
			object obj60;
			xamlServiceProvider43.Add(typeFromHandle85, obj60 = new SimpleValueTargetProvider(array43, SfChart.AreaBackgroundColorProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(504, 25)));
			DynamicResource dynamicResource26 = markupExtension43.ProvideValue(xamlServiceProvider43);
			sfChart.SetDynamicResource(SfChart.AreaBackgroundColorProperty, dynamicResource26.Key);
			dynamicResourceExtension27.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension44 = dynamicResourceExtension27;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 5];
			array44[0] = sfChart;
			array44[1] = grid4;
			array44[2] = scrollView2;
			array44[3] = grid5;
			array44[4] = this;
			object obj61;
			xamlServiceProvider44.Add(typeFromHandle87, obj61 = new SimpleValueTargetProvider(array44, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj61);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(505, 25)));
			DynamicResource dynamicResource27 = markupExtension44.ProvideValue(xamlServiceProvider44);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource27.Key);
			sfChart.SetValue(VisualElement.HeightRequestProperty, 250.0);
			sfChart.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			dateTimeAxis.SetValue(DateTimeAxis.IntervalProperty, new double?(1.0));
			dateTimeAxis.SetValue(DateTimeAxis.IntervalTypeProperty, 1);
			staticResourceExtension.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension45 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 6];
			array45[0] = dateTimeAxis;
			array45[1] = sfChart;
			array45[2] = grid4;
			array45[3] = scrollView2;
			array45[4] = grid5;
			array45[5] = this;
			object obj62;
			xamlServiceProvider45.Add(typeFromHandle89, obj62 = new SimpleValueTargetProvider(array45, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(513, 33)));
			object obj63 = markupExtension45.ProvideValue(xamlServiceProvider45);
			dateTimeAxis.MajorGridLineStyle = obj63;
			staticResourceExtension2.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension46 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 6];
			array46[0] = dateTimeAxis;
			array46[1] = sfChart;
			array46[2] = grid4;
			array46[3] = scrollView2;
			array46[4] = grid5;
			array46[5] = this;
			object obj64;
			xamlServiceProvider46.Add(typeFromHandle91, obj64 = new SimpleValueTargetProvider(array46, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(514, 33)));
			object obj65 = markupExtension46.ProvideValue(xamlServiceProvider46);
			dateTimeAxis.MajorTickStyle = obj65;
			dateTimeAxis.SetValue(DateTimeAxis.RangePaddingProperty, 0);
			dynamicResourceExtension28.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension47 = dynamicResourceExtension28;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 7];
			array47[0] = chartAxisLabelStyle;
			array47[1] = dateTimeAxis;
			array47[2] = sfChart;
			array47[3] = grid4;
			array47[4] = scrollView2;
			array47[5] = grid5;
			array47[6] = this;
			object obj66;
			xamlServiceProvider47.Add(typeFromHandle93, obj66 = new SimpleValueTargetProvider(array47, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(517, 64)));
			DynamicResource dynamicResource28 = markupExtension47.ProvideValue(xamlServiceProvider47);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource28.Key);
			dateTimeAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			dynamicResourceExtension29.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension48 = dynamicResourceExtension29;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 7];
			array48[0] = chartLineStyle;
			array48[1] = dateTimeAxis;
			array48[2] = sfChart;
			array48[3] = grid4;
			array48[4] = scrollView2;
			array48[5] = grid5;
			array48[6] = this;
			object obj67;
			xamlServiceProvider48.Add(typeFromHandle95, obj67 = new SimpleValueTargetProvider(array48, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(520, 59)));
			DynamicResource dynamicResource29 = markupExtension48.ProvideValue(xamlServiceProvider48);
			chartLineStyle.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource29.Key);
			dateTimeAxis.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, dateTimeAxis);
			staticResourceExtension3.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension49 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 6];
			array49[0] = numericalAxis;
			array49[1] = sfChart;
			array49[2] = grid4;
			array49[3] = scrollView2;
			array49[4] = grid5;
			array49[5] = this;
			object obj68;
			xamlServiceProvider49.Add(typeFromHandle97, obj68 = new SimpleValueTargetProvider(array49, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(527, 33)));
			object obj69 = markupExtension49.ProvideValue(xamlServiceProvider49);
			numericalAxis.MajorGridLineStyle = obj69;
			staticResourceExtension4.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension50 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 6];
			array50[0] = numericalAxis;
			array50[1] = sfChart;
			array50[2] = grid4;
			array50[3] = scrollView2;
			array50[4] = grid5;
			array50[5] = this;
			object obj70;
			xamlServiceProvider50.Add(typeFromHandle99, obj70 = new SimpleValueTargetProvider(array50, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(528, 33)));
			object obj71 = markupExtension50.ProvideValue(xamlServiceProvider50);
			numericalAxis.MajorTickStyle = obj71;
			numericalAxis.SetValue(NumericalAxis.RangePaddingProperty, 1);
			dynamicResourceExtension30.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension51 = dynamicResourceExtension30;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 7];
			array51[0] = chartAxisLabelStyle2;
			array51[1] = numericalAxis;
			array51[2] = sfChart;
			array51[3] = grid4;
			array51[4] = scrollView2;
			array51[5] = grid5;
			array51[6] = this;
			object obj72;
			xamlServiceProvider51.Add(typeFromHandle101, obj72 = new SimpleValueTargetProvider(array51, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(531, 64)));
			DynamicResource dynamicResource30 = markupExtension51.ProvideValue(xamlServiceProvider51);
			chartAxisLabelStyle2.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource30.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			dynamicResourceExtension31.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension52 = dynamicResourceExtension31;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 7];
			array52[0] = chartLineStyle2;
			array52[1] = numericalAxis;
			array52[2] = sfChart;
			array52[3] = grid4;
			array52[4] = scrollView2;
			array52[5] = grid5;
			array52[6] = this;
			object obj73;
			xamlServiceProvider52.Add(typeFromHandle103, obj73 = new SimpleValueTargetProvider(array52, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider52.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver52, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(534, 59)));
			DynamicResource dynamicResource31 = markupExtension52.ProvideValue(xamlServiceProvider52);
			chartLineStyle2.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource31.Key);
			numericalAxis.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle2);
			sfChart.SetValue(SfChart.SecondaryAxisProperty, numericalAxis);
			fastLineSeries.SetValue(ChartSeries.EnableAnimationProperty, true);
			fastLineSeries.SetValue(ChartSeries.EnableDataPointSelectionProperty, true);
			fastLineSeries.SetValue(ChartSeries.IsVisibleOnLegendProperty, true);
			bindingExtension20.Path = "DayCycles";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			fastLineSeries.SetBinding(ChartSeries.ItemsSourceProperty, bindingBase20);
			translate18.Text = "PID_AvgFuelConsumption";
			IMarkupExtension markupExtension53 = translate18;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 6];
			array53[0] = fastLineSeries;
			array53[1] = sfChart;
			array53[2] = grid4;
			array53[3] = scrollView2;
			array53[4] = grid5;
			array53[5] = this;
			object obj74;
			xamlServiceProvider53.Add(typeFromHandle105, obj74 = new SimpleValueTargetProvider(array53, ChartSeries.LabelProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider53.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver53, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(555, 33)));
			object obj75 = markupExtension53.ProvideValue(xamlServiceProvider53);
			fastLineSeries.Label = obj75;
			fastLineSeries.SetValue(CartesianSeries.ShowTrackballInfoProperty, true);
			fastLineSeries.SetValue(ChartSeries.StrokeWidthProperty, 1.0);
			fastLineSeries.SetValue(ChartSeries.XBindingPathProperty, "TimeStarted");
			fastLineSeries.SetValue(XyDataSeries.YBindingPathProperty, "AvgFuelConsumption");
			fastLineSeries.SetValue(ChartSeries.ColorProperty, new Color(0.3960784375667572, 0.6000000238418579, 1.0, 1.0));
			sfChart.GetValue(SfChart.SeriesProperty).Add(fastLineSeries);
			fastLineSeries2.SetValue(ChartSeries.EnableAnimationProperty, true);
			fastLineSeries2.SetValue(ChartSeries.EnableDataPointSelectionProperty, true);
			fastLineSeries2.SetValue(ChartSeries.IsVisibleOnLegendProperty, true);
			bindingExtension21.Path = "DayCycles";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			fastLineSeries2.SetBinding(ChartSeries.ItemsSourceProperty, bindingBase21);
			fastLineSeries2.SetValue(CartesianSeries.ShowTrackballInfoProperty, true);
			fastLineSeries2.SetValue(ChartSeries.StrokeWidthProperty, 1.0);
			fastLineSeries2.SetValue(ChartSeries.XBindingPathProperty, "TimeStarted");
			fastLineSeries2.SetValue(XyDataSeries.YBindingPathProperty, "AvgSpeed");
			fastLineSeries2.SetValue(ChartSeries.ColorProperty, new Color(0.6000000238418579, 0.6000000238418579, 0.6000000238418579, 1.0));
			sfChart.GetValue(SfChart.SeriesProperty).Add(fastLineSeries2);
			fastLineSeries3.SetValue(ChartSeries.EnableAnimationProperty, true);
			fastLineSeries3.SetValue(ChartSeries.EnableDataPointSelectionProperty, true);
			fastLineSeries3.SetValue(ChartSeries.IsVisibleOnLegendProperty, true);
			bindingExtension22.Path = "DayCycles";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			fastLineSeries3.SetBinding(ChartSeries.ItemsSourceProperty, bindingBase22);
			fastLineSeries3.SetValue(CartesianSeries.ShowTrackballInfoProperty, true);
			fastLineSeries3.SetValue(ChartSeries.StrokeWidthProperty, 1.0);
			fastLineSeries3.SetValue(ChartSeries.XBindingPathProperty, "TimeStarted");
			fastLineSeries3.SetValue(XyDataSeries.YBindingPathProperty, "FuelUsedCurrentUnits");
			fastLineSeries3.SetValue(ChartSeries.ColorProperty, new Color(0.6000000238418579, 0.800000011920929, 0.40392157435417175, 1.0));
			sfChart.GetValue(SfChart.SeriesProperty).Add(fastLineSeries3);
			fastLineSeries4.SetValue(ChartSeries.EnableAnimationProperty, true);
			fastLineSeries4.SetValue(ChartSeries.EnableDataPointSelectionProperty, true);
			fastLineSeries4.SetValue(ChartSeries.IsVisibleOnLegendProperty, true);
			bindingExtension23.Path = "DayCycles";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			fastLineSeries4.SetBinding(ChartSeries.ItemsSourceProperty, bindingBase23);
			fastLineSeries4.SetValue(CartesianSeries.ShowTrackballInfoProperty, true);
			fastLineSeries4.SetValue(ChartSeries.StrokeWidthProperty, 1.0);
			fastLineSeries4.SetValue(ChartSeries.XBindingPathProperty, "TimeStarted");
			fastLineSeries4.SetValue(XyDataSeries.YBindingPathProperty, "Distance");
			fastLineSeries4.SetValue(ChartSeries.ColorProperty, new Color(1.0, 0.40392157435417175, 0.4000000059604645, 1.0));
			sfChart.GetValue(SfChart.SeriesProperty).Add(fastLineSeries4);
			fastLineSeries5.SetValue(ChartSeries.EnableAnimationProperty, true);
			fastLineSeries5.SetValue(ChartSeries.EnableDataPointSelectionProperty, true);
			fastLineSeries5.SetValue(ChartSeries.IsVisibleOnLegendProperty, true);
			bindingExtension24.Path = "DayCycles";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			fastLineSeries5.SetBinding(ChartSeries.ItemsSourceProperty, bindingBase24);
			fastLineSeries5.SetValue(CartesianSeries.ShowTrackballInfoProperty, true);
			fastLineSeries5.SetValue(ChartSeries.StrokeWidthProperty, 1.0);
			fastLineSeries5.SetValue(ChartSeries.XBindingPathProperty, "TimeStarted");
			fastLineSeries5.SetValue(XyDataSeries.YBindingPathProperty, "TotalFuelPrice");
			fastLineSeries5.SetValue(ChartSeries.ColorProperty, new Color(0.6000000238418579, 0.4000000059604645, 0.7960784435272217, 1.0));
			sfChart.GetValue(SfChart.SeriesProperty).Add(fastLineSeries5);
			fastLineSeries6.SetValue(ChartSeries.EnableAnimationProperty, true);
			fastLineSeries6.SetValue(ChartSeries.EnableDataPointSelectionProperty, true);
			fastLineSeries6.SetValue(ChartSeries.IsVisibleOnLegendProperty, true);
			bindingExtension25.Path = "DayCycles";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			fastLineSeries6.SetBinding(ChartSeries.ItemsSourceProperty, bindingBase25);
			fastLineSeries6.SetValue(CartesianSeries.ShowTrackballInfoProperty, true);
			fastLineSeries6.SetValue(ChartSeries.StrokeWidthProperty, 1.0);
			fastLineSeries6.SetValue(ChartSeries.XBindingPathProperty, "TimeStarted");
			fastLineSeries6.SetValue(XyDataSeries.YBindingPathProperty, "MotorHours");
			fastLineSeries6.SetValue(ChartSeries.ColorProperty, new Color(0.9254902005195618, 0.8352941274642944, 0.07450980693101883, 1.0));
			sfChart.GetValue(SfChart.SeriesProperty).Add(fastLineSeries6);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableDoubleTapProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnablePanningProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableSelectionZoomingProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableZoomingProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.ZoomModeProperty, 0);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartZoomPanBehavior);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.LabelDisplayModeProperty, 1);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.ShowLabelProperty, true);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.ShowLineProperty, true);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartTrackballBehavior);
			grid4.Children.Add(sfChart);
			button12.SetValue(Grid.RowProperty, 5);
			button12.Clicked += this.btnDCList_Clicked;
			button12.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate19.Text = "ios_btnDCList";
			IMarkupExtension markupExtension54 = translate19;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 5];
			array54[0] = button12;
			array54[1] = grid4;
			array54[2] = scrollView2;
			array54[3] = grid5;
			array54[4] = this;
			object obj76;
			xamlServiceProvider54.Add(typeFromHandle107, obj76 = new SimpleValueTargetProvider(array54, Button.TextProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider54.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver54, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(654, 25)));
			object obj77 = markupExtension54.ProvideValue(xamlServiceProvider54);
			button12.Text = obj77;
			grid4.Children.Add(button12);
			label19.SetValue(Grid.RowProperty, 6);
			dynamicResourceExtension32.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension55 = dynamicResourceExtension32;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 5];
			array55[0] = label19;
			array55[1] = grid4;
			array55[2] = scrollView2;
			array55[3] = grid5;
			array55[4] = this;
			object obj78;
			xamlServiceProvider55.Add(typeFromHandle109, obj78 = new SimpleValueTargetProvider(array55, Label.FontSizeProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider55.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver55, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(657, 25)));
			DynamicResource dynamicResource32 = markupExtension55.ProvideValue(xamlServiceProvider55);
			label19.SetDynamicResource(Label.FontSizeProperty, dynamicResource32.Key);
			translate20.Text = "ios_MotorHours_Hint";
			IMarkupExtension markupExtension56 = translate20;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 5];
			array56[0] = label19;
			array56[1] = grid4;
			array56[2] = scrollView2;
			array56[3] = grid5;
			array56[4] = this;
			object obj79;
			xamlServiceProvider56.Add(typeFromHandle111, obj79 = new SimpleValueTargetProvider(array56, Label.TextProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider56.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver56, typeof(FuelStatisticsPage).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(658, 25)));
			object obj80 = markupExtension56.ProvideValue(xamlServiceProvider56);
			label19.Text = obj80;
			grid4.Children.Add(label19);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(Grid.RowSpanProperty, 3);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid4.Children.Add(activityFrame);
			scrollView2.Content = grid4;
			grid5.Children.Add(scrollView2);
			complexAdView.SetValue(Grid.RowProperty, 1);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid5.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid5);
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x00282825 File Offset: 0x00280A25
		[CompilerGenerated]
		private void <.ctor>b__1_0()
		{
			this.ResetModelQuestion();
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x0028282D File Offset: 0x00280A2D
		[CompilerGenerated]
		private void <.ctor>b__1_1()
		{
			if (PlatformHelper.IsiOS && App.UseLegacyUI)
			{
				base.Navigation.PushAsync(new SettingsFuelRatePage());
				return;
			}
			base.Navigation.PushAsync(new SettingsFuelRateV3());
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x00282860 File Offset: 0x00280A60
		[CompilerGenerated]
		private void <.ctor>b__1_2()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x0028286C File Offset: 0x00280A6C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<FuelStatisticsPage>(this, typeof(FuelStatisticsPage));
			this.gridInfoRow1 = NameScopeExtensions.FindByName<Grid>(this, "gridInfoRow1");
			this.boxAvgFuelConsumption = NameScopeExtensions.FindByName<BoxView>(this, "boxAvgFuelConsumption");
			this.boxAvgSpeed = NameScopeExtensions.FindByName<BoxView>(this, "boxAvgSpeed");
			this.boxMotorHours = NameScopeExtensions.FindByName<BoxView>(this, "boxMotorHours");
			this.gridInfoRow2 = NameScopeExtensions.FindByName<Grid>(this, "gridInfoRow2");
			this.boxTotalDistance = NameScopeExtensions.FindByName<BoxView>(this, "boxTotalDistance");
			this.boxTotalFuelUsed = NameScopeExtensions.FindByName<BoxView>(this, "boxTotalFuelUsed");
			this.boxTotalFuelPrice = NameScopeExtensions.FindByName<BoxView>(this, "boxTotalFuelPrice");
			this.btnDays = NameScopeExtensions.FindByName<Button>(this, "btnDays");
			this.btnHours = NameScopeExtensions.FindByName<Button>(this, "btnHours");
			this.panelDays = NameScopeExtensions.FindByName<StackLayout>(this, "panelDays");
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
			this.xaxis = NameScopeExtensions.FindByName<DateTimeAxis>(this, "xaxis");
			this.seriesAvgFuel = NameScopeExtensions.FindByName<FastLineSeries>(this, "seriesAvgFuel");
			this.seriesAvgSpeed = NameScopeExtensions.FindByName<FastLineSeries>(this, "seriesAvgSpeed");
			this.seriesFuelUsed = NameScopeExtensions.FindByName<FastLineSeries>(this, "seriesFuelUsed");
			this.seriesDistance = NameScopeExtensions.FindByName<FastLineSeries>(this, "seriesDistance");
			this.seriesFuelPrice = NameScopeExtensions.FindByName<FastLineSeries>(this, "seriesFuelPrice");
			this.seriesMotorHours = NameScopeExtensions.FindByName<FastLineSeries>(this, "seriesMotorHours");
			this.btnDCList = NameScopeExtensions.FindByName<Button>(this, "btnDCList");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0400209E RID: 8350
		public static FuelStatisticsPage Instance;

		// Token: 0x0400209F RID: 8351
		private bool updater_started;

		// Token: 0x040020A0 RID: 8352
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridInfoRow1;

		// Token: 0x040020A1 RID: 8353
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView boxAvgFuelConsumption;

		// Token: 0x040020A2 RID: 8354
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView boxAvgSpeed;

		// Token: 0x040020A3 RID: 8355
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView boxMotorHours;

		// Token: 0x040020A4 RID: 8356
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridInfoRow2;

		// Token: 0x040020A5 RID: 8357
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView boxTotalDistance;

		// Token: 0x040020A6 RID: 8358
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView boxTotalFuelUsed;

		// Token: 0x040020A7 RID: 8359
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView boxTotalFuelPrice;

		// Token: 0x040020A8 RID: 8360
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDays;

		// Token: 0x040020A9 RID: 8361
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnHours;

		// Token: 0x040020AA RID: 8362
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelDays;

		// Token: 0x040020AB RID: 8363
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x040020AC RID: 8364
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private DateTimeAxis xaxis;

		// Token: 0x040020AD RID: 8365
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FastLineSeries seriesAvgFuel;

		// Token: 0x040020AE RID: 8366
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FastLineSeries seriesAvgSpeed;

		// Token: 0x040020AF RID: 8367
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FastLineSeries seriesFuelUsed;

		// Token: 0x040020B0 RID: 8368
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FastLineSeries seriesDistance;

		// Token: 0x040020B1 RID: 8369
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FastLineSeries seriesFuelPrice;

		// Token: 0x040020B2 RID: 8370
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FastLineSeries seriesMotorHours;

		// Token: 0x040020B3 RID: 8371
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDCList;

		// Token: 0x040020B4 RID: 8372
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x040020B5 RID: 8373
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x02000605 RID: 1541
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003687 RID: 13959 RVA: 0x00282A00 File Offset: 0x00280C00
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003688 RID: 13960 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003689 RID: 13961 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <AddRequestsDelegate>b__11_0(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x040020B6 RID: 8374
			public static readonly FuelStatisticsPage.<>c <>9 = new FuelStatisticsPage.<>c();

			// Token: 0x040020B7 RID: 8375
			public static Func<PID, bool> <>9__11_0;
		}

		// Token: 0x02000606 RID: 1542
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x0600368A RID: 13962 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x0600368B RID: 13963 RVA: 0x00282A0C File Offset: 0x00280C0C
			internal void <OnProgressChanged>b__0()
			{
				this.<>4__this.activityFrame.Text = string.Format(Translate.GetString("ios_Loading"), this.e.Key, this.e.Value);
			}

			// Token: 0x040020B8 RID: 8376
			public FuelStatisticsPage <>4__this;

			// Token: 0x040020B9 RID: 8377
			public KeyValuePair<int, int> e;
		}

		// Token: 0x02000607 RID: 1543
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <FullRefreshModel>d__12 : IAsyncStateMachine
		{
			// Token: 0x0600368C RID: 13964 RVA: 0x00282A58 File Offset: 0x00280C58
			void IAsyncStateMachine.MoveNext()
			{
				FuelStatisticsPage fuelStatisticsPage = this;
				try
				{
					fuelStatisticsPage.activityFrame.IsVisible = true;
					fuelStatisticsPage.model.PeriodAllTime.Execute(null);
					fuelStatisticsPage.chart.BindingContext = fuelStatisticsPage.model;
					fuelStatisticsPage.activityFrame.IsVisible = false;
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

			// Token: 0x0600368D RID: 13965 RVA: 0x00282AE4 File Offset: 0x00280CE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020BA RID: 8378
			public int <>1__state;

			// Token: 0x040020BB RID: 8379
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020BC RID: 8380
			public FuelStatisticsPage <>4__this;
		}

		// Token: 0x02000608 RID: 1544
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ResetModelQuestion>d__7 : IAsyncStateMachine
		{
			// Token: 0x0600368E RID: 13966 RVA: 0x00282AF4 File Offset: 0x00280CF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FuelStatisticsPage fuelStatisticsPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
						{
							fuelStatisticsPage.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK");
							goto IL_00D9;
						}
						taskAwaiter3 = fuelStatisticsPage.DisplayAlert(Translate.GetString("ios_ResetStatsTitle"), "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, FuelStatisticsPage.<ResetModelQuestion>d__7>(ref taskAwaiter3, ref this);
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
						fuelStatisticsPage.model.Reset();
						fuelStatisticsPage.model.PeriodAllTime.Execute(null);
					}
					IL_00D9:;
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

			// Token: 0x0600368F RID: 13967 RVA: 0x00282C18 File Offset: 0x00280E18
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020BD RID: 8381
			public int <>1__state;

			// Token: 0x040020BE RID: 8382
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020BF RID: 8383
			public FuelStatisticsPage <>4__this;

			// Token: 0x040020C0 RID: 8384
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000609 RID: 1545
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StartUpdateCycles>d__3 : IAsyncStateMachine
		{
			// Token: 0x06003690 RID: 13968 RVA: 0x00282C28 File Offset: 0x00280E28
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FuelStatisticsPage fuelStatisticsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!fuelStatisticsPage.updater_started)
						{
							fuelStatisticsPage.updater_started = true;
							goto IL_0094;
						}
						goto IL_00BA;
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					IL_0083:
					taskAwaiter.GetResult();
					DriveCycleViewModel.Current.RefreshPeriodForStatisticsScreenWithoutRebuildingDays();
					IL_0094:
					if (!Application.Current.MainPage.Navigation.NavigationStack.Contains(fuelStatisticsPage))
					{
						fuelStatisticsPage.updater_started = false;
					}
					else
					{
						taskAwaiter = Task.Delay(TimeSpan.FromSeconds(5.0)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FuelStatisticsPage.<StartUpdateCycles>d__3>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0083;
					}
					IL_00BA:;
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

			// Token: 0x06003691 RID: 13969 RVA: 0x00282D2C File Offset: 0x00280F2C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020C1 RID: 8385
			public int <>1__state;

			// Token: 0x040020C2 RID: 8386
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020C3 RID: 8387
			public FuelStatisticsPage <>4__this;

			// Token: 0x040020C4 RID: 8388
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200060A RID: 1546
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCustomPeriod_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06003692 RID: 13970 RVA: 0x00282D3C File Offset: 0x00280F3C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FuelStatisticsPage fuelStatisticsPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0105;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							FuelStatisticsDatePeriodSelectorPage fuelStatisticsDatePeriodSelectorPage = new FuelStatisticsDatePeriodSelectorPage(fuelStatisticsPage.model);
							fuelStatisticsPage.Navigation.PushAsync(fuelStatisticsDatePeriodSelectorPage);
							goto IL_0142;
						}
						taskAwaiter5 = fuelStatisticsPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, FuelStatisticsPage.<btnCustomPeriod_Clicked>d__9>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_010C;
					}
					taskAwaiter3 = fuelStatisticsPage.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FuelStatisticsPage.<btnCustomPeriod_Clicked>d__9>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0105:
					taskAwaiter3.GetResult();
					IL_010C:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0142:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003693 RID: 13971 RVA: 0x00282EBC File Offset: 0x002810BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020C5 RID: 8389
			public int <>1__state;

			// Token: 0x040020C6 RID: 8390
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020C7 RID: 8391
			public FuelStatisticsPage <>4__this;

			// Token: 0x040020C8 RID: 8392
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040020C9 RID: 8393
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200060B RID: 1547
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDCList_Clicked>d__19 : IAsyncStateMachine
		{
			// Token: 0x06003694 RID: 13972 RVA: 0x00282ECC File Offset: 0x002810CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FuelStatisticsPage fuelStatisticsPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0118;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_017C;
					}
					default:
						if (SharedSettings.Current.AdsProductPurchased)
						{
							DriveCyclesListPage driveCyclesListPage = new DriveCyclesListPage(fuelStatisticsPage.model);
							taskAwaiter3 = fuelStatisticsPage.Navigation.PushAsync(driveCyclesListPage).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FuelStatisticsPage.<btnDCList_Clicked>d__19>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter5 = fuelStatisticsPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, FuelStatisticsPage.<btnDCList_Clicked>d__19>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_0118;
						}
						break;
					}
					taskAwaiter3.GetResult();
					goto IL_0183;
					IL_0118:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0183;
					}
					taskAwaiter3 = fuelStatisticsPage.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FuelStatisticsPage.<btnDCList_Clicked>d__19>(ref taskAwaiter3, ref this);
						return;
					}
					IL_017C:
					taskAwaiter3.GetResult();
					IL_0183:;
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

			// Token: 0x06003695 RID: 13973 RVA: 0x002830A8 File Offset: 0x002812A8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020CA RID: 8394
			public int <>1__state;

			// Token: 0x040020CB RID: 8395
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020CC RID: 8396
			public FuelStatisticsPage <>4__this;

			// Token: 0x040020CD RID: 8397
			private TaskAwaiter <>u__1;

			// Token: 0x040020CE RID: 8398
			private TaskAwaiter<bool> <>u__2;
		}
	}
}
