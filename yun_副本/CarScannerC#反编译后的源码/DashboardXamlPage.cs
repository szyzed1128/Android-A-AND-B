using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.DashboardPages;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using MR.Gestures;
using Syncfusion.XForms.PopupLayout;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020000B9 RID: 185
	[XamlFilePath("Pages\\Dashboard\\DashboardXamlPage.xaml")]
	public class DashboardXamlPage : ContentPage, INotifyPropertyChanged
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00027D6B File Offset: 0x00025F6B
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00027D72 File Offset: 0x00025F72
		public static DashboardXamlPage Instance
		{
			[CompilerGenerated]
			get
			{
				return DashboardXamlPage.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				DashboardXamlPage.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00027D7A File Offset: 0x00025F7A
		public Grid GetButtonsGrid()
		{
			return this.gridButtons;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00027D84 File Offset: 0x00025F84
		public async ValueTask HideTopGrid()
		{
			if (SharedSettings.Current.DashboardHideTopControls && this.gridButtons.Opacity == 1.0 && !this.pageSettingsPopupLayout.IsOpen)
			{
				if (Device.RuntimePlatform == "iOS")
				{
					Page.SetUseSafeArea(base.On<iOS>(), !SharedSettings.Current.iOSDashboardUseFullscreen);
				}
				if (SharedSettings.Current.DashboardAnimation)
				{
					await ViewExtensions.FadeTo(this.gridButtons, 1.0, 0U, null);
					await ViewExtensions.FadeTo(this.gridButtons, 0.0, 250U, null);
				}
				else
				{
					this.gridButtons.Opacity = 0.0;
				}
				for (int i = 0; i < 3; i++)
				{
					this.LayoutRoot.LowerChild(this.gridButtons);
				}
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00027DC8 File Offset: 0x00025FC8
		public async ValueTask ShowTopGrid()
		{
			if (SharedSettings.Current.DashboardHideTopControls)
			{
				this.RestartTimer();
				if (this.gridButtons.Opacity == 0.0)
				{
					if (Device.RuntimePlatform == "iOS" && DeviceDisplay.MainDisplayInfo.Orientation == 1)
					{
						Page.SetUseSafeArea(base.On<iOS>(), true);
					}
					if (SharedSettings.Current.DashboardAnimation)
					{
						await ViewExtensions.FadeTo(this.gridButtons, 0.0, 0U, null);
						await ViewExtensions.FadeTo(this.gridButtons, 1.0, 250U, null);
					}
					else
					{
						this.gridButtons.Opacity = 1.0;
					}
					for (int i = 0; i < 3; i++)
					{
						this.LayoutRoot.RaiseChild(this.gridButtons);
					}
				}
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00027E0B File Offset: 0x0002600B
		public void RestartTimer()
		{
			if (SharedSettings.Current.DashboardHideTopControls)
			{
				this.timer.Stop();
				this.timer.Start();
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00027E30 File Offset: 0x00026030
		public DashboardXamlPage()
		{
			DashboardXamlPage.Instance = this;
			this.ShouldLoadDashboardFromSettings = true;
			this.InitializeComponent();
			if (!SharedSettings.Current.DashboardHideTopControls)
			{
				Grid.SetRow(this.gridButtons, 0);
			}
			if (!SharedSettings.Current.DashboardAnimation)
			{
				this.popupView.AnimationMode = 6;
			}
			string localFilePath = FileSystemHelper.GetLocalFilePath("dashbg");
			this.timer = new Timer();
			this.timer.Interval = this.hideInterval;
			this.timer.Stop();
			this.timer.AutoReset = true;
			this.timer.Elapsed += this.Timer_Elapsed;
			if (File.Exists(localFilePath))
			{
				this.imgBackground.Source = ImageSource.FromFile(localFilePath);
				this.imgBackground.IsVisible = true;
			}
			else
			{
				this.imgBackground.IsVisible = false;
			}
			if (PlatformHelper.IsiOS)
			{
				Page.SetPrefersHomeIndicatorAutoHidden(this, true);
			}
			if (PlatformHelper.IsAndroid)
			{
				if (SharedSettings.Current.AndroidUseFullscreen)
				{
					try
					{
						PlatformHelper.DroidService.Window_SetFullscreenOn();
					}
					catch (Exception)
					{
					}
				}
				if (SharedSettings.Current.AndroidRecolorStatusBarInDarkTheme && PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
				{
					try
					{
						this.originalStatusBarColor = PlatformHelper.DroidService.Window_StatusBarColor;
					}
					catch (Exception)
					{
					}
				}
			}
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
				return;
			}
			this.ad.IsVisible = true;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00027FBC File Offset: 0x000261BC
		private void ucUpdateInBackground_SizeChanged(object sender, object e)
		{
			CheckBoxWithLabel checkBoxWithLabel = sender as CheckBoxWithLabel;
			if (checkBoxWithLabel != null)
			{
				checkBoxWithLabel.IsToggled = this.Pages[this.CurrentPage].UpdateInBackground;
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00027FEF File Offset: 0x000261EF
		private void ucUpdateInBackground_Toggled(object sender, ToggledEventArgs e)
		{
			this.Pages[this.CurrentPage].UpdateInBackground = e.Value;
			DashboardListViewModel.Current.SaveDashboardToSettings();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00028017 File Offset: 0x00026217
		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.HideTopGrid();
			});
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0002802C File Offset: 0x0002622C
		private async void Handle_SizeChanged(object sender, EventArgs e)
		{
			if (base.Width > base.Height)
			{
				base.Resources["popupMaxWidth"] = base.Width * 0.75;
				base.Resources["popupMaxHeight"] = base.Height * 0.8;
			}
			else
			{
				base.Resources["popupMaxWidth"] = base.Width * 0.75;
				base.Resources["popupMaxHeight"] = base.Height * 0.75;
			}
			if (this.gridButtons.Opacity > 0.0 && Device.RuntimePlatform == "iOS" && DeviceDisplay.MainDisplayInfo.Orientation == 1)
			{
				Page.SetUseSafeArea(base.On<iOS>(), true);
			}
			if (this.pageSettingsPopupLayout.IsOpen)
			{
				this.pageSettingsPopupLayout.IsOpen = false;
				await Task.Delay(50);
				this.btnSettings_Clicked(this.btnSettings, EventArgs.Empty);
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00028064 File Offset: 0x00026264
		private void BackgroundGrid_Swiped(object sender, SwipeEventArgs e)
		{
			if (this.Pages == null || this.Pages.Count == 0)
			{
				return;
			}
			if (this.CurrentPage >= this.Pages.Count || this.CurrentPage < 0)
			{
				return;
			}
			Dash_CustomPage dash_CustomPage = this.Pages[this.CurrentPage] as Dash_CustomPage;
			if (dash_CustomPage != null && dash_CustomPage.GestureMode == Dash_CustomPage.GestureModes.Move && dash_CustomPage.DraggingControl != null)
			{
				return;
			}
			this.RestartTimer();
			if (this.btnPrevPage.IsEnabled && this.btnNextPage.IsEnabled && Math.Abs(e.TotalDistance.X) > Math.Abs(e.TotalDistance.Y))
			{
				if (e.TotalDistance.X > 0.0)
				{
					this.btnPrevPage_Clicked(null, null);
					return;
				}
				this.btnNextPage_Clicked(null, null);
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00028142 File Offset: 0x00026342
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			this.RestartTimer();
			base.DisplayAlert(Translate.GetString("ios_MainPage_TileDashboard"), Translate.GetString("ios_Dashboard_InfoText"), "OK");
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0002816A File Offset: 0x0002636A
		private void btnMirror_Clicked(object sender, EventArgs e)
		{
			this.RestartTimer();
			this.HUDMode = !this.HUDMode;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00028181 File Offset: 0x00026381
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x000281A8 File Offset: 0x000263A8
		public bool HUDMode
		{
			get
			{
				return Math.Abs(this.BackgroundGrid.RotationY - 180.0) < 0.1;
			}
			set
			{
				if (value)
				{
					this.BackgroundGrid.RotationY = 180.0;
					SharedSettings.Current.DashboardHUDMode = true;
					this.btnMirror.BackgroundColor = Color.Red;
					return;
				}
				this.BackgroundGrid.RotationY = 0.0;
				SharedSettings.Current.DashboardHUDMode = false;
				this.btnMirror.BackgroundColor = Color.Transparent;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00028218 File Offset: 0x00026418
		private void Page_Appearing(object sender, EventArgs e)
		{
			if (Device.RuntimePlatform != "Android")
			{
				DependencyService.Get<IStatusBar>(0).HideStatusBar();
			}
			if (SharedSettings.Current.DashboardTheme == 0 || SharedSettings.Current.DashboardTheme == 2)
			{
				this.btnInfo.Source = "icons8_info_invert.png";
				this.btnSettings.Source = "icons8_settings_invert.png";
				this.btnMirror.Source = "icons8_mirror_inverted.png";
				base.BackgroundColor = Color.Black;
				this.gridButtonsBackground.Color = (Color)base.Resources["TopGridDarkBackground"];
				if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidRecolorStatusBarInDarkTheme && PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
				{
					PlatformHelper.DroidService.Window_SetStatusBarColor(Color.Black);
				}
				if (PlatformHelper.IsiOS)
				{
					PlatformHelper.IOSService.SetStatusBarStyle_LightContent();
				}
			}
			else
			{
				base.BackgroundColor = Color.White;
				this.gridButtonsBackground.Color = (Color)base.Resources["TopGridLightBackground"];
				this.btnInfo.Source = "icons8_info.png";
				this.btnSettings.Source = "icons8_settings.png";
				this.btnMirror.Source = "icons8_mirror.png";
				if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidRecolorStatusBarInDarkTheme && PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
				{
					PlatformHelper.DroidService.Window_SetStatusBarColor(this.originalStatusBarColor);
				}
				if (PlatformHelper.IsiOS)
				{
					PlatformHelper.IOSService.SetStatusBarStyle_DarkContent();
				}
			}
			this.btnBack.BackgroundColor = Color.Transparent;
			this.btnPrevPage.BackgroundColor = Color.Transparent;
			this.labelSlash.BackgroundColor = Color.Transparent;
			this.btnNextPage.BackgroundColor = Color.Transparent;
			this.btnMirror.BackgroundColor = Color.Transparent;
			this.btnSettings.BackgroundColor = Color.Transparent;
			this.btnInfo.BackgroundColor = Color.Transparent;
			if (this.imgBackground.IsVisible)
			{
				this.btnBack.BackgroundColor = Color.Transparent;
				this.btnPrevPage.BackgroundColor = Color.Transparent;
				this.labelSlash.BackgroundColor = Color.Transparent;
				this.btnNextPage.BackgroundColor = Color.Transparent;
				this.btnMirror.BackgroundColor = Color.Transparent;
				this.btnSettings.BackgroundColor = Color.Transparent;
				this.btnInfo.BackgroundColor = Color.Transparent;
			}
			if (this.ShouldLoadDashboardFromSettings)
			{
				Device.BeginInvokeOnMainThread(async delegate
				{
					this.gridButtons.IsEnabled = false;
					await this.LoadDashboardFromSettings();
					this.labelSlash.BindingContext = this;
					this.HUDMode = SharedSettings.Current.DashboardHUDMode;
					this.gridButtons.IsEnabled = true;
					try
					{
						EventHandler<DashboardPage> dashboardLoaded = DashboardXamlPage.DashboardLoaded;
						if (dashboardLoaded != null)
						{
							dashboardLoaded(this, this.CurrentDashboardPage);
						}
					}
					catch (Exception)
					{
					}
				});
			}
			this.timer.Start();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000284C8 File Offset: 0x000266C8
		private void Page_Disappearing(object sender, EventArgs e)
		{
			this.timer.Stop();
			DependencyService.Get<IStatusBar>(0).ShowStatusBar();
			if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidRecolorStatusBarInDarkTheme && PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
			{
				PlatformHelper.DroidService.Window_SetStatusBarColor(this.originalStatusBarColor);
			}
			if (PlatformHelper.IsiOS)
			{
				PlatformHelper.IOSService.SetStatusBarStyle_LightContent();
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060003AB RID: 939 RVA: 0x0002852C File Offset: 0x0002672C
		// (remove) Token: 0x060003AC RID: 940 RVA: 0x00028564 File Offset: 0x00026764
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0002859C File Offset: 0x0002679C
		private void OnPropertyChanged(string PropertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003AE RID: 942 RVA: 0x000285C0 File Offset: 0x000267C0
		public int CurrentPagePlusOne
		{
			get
			{
				return this.CurrentPage + 1;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003AF RID: 943 RVA: 0x000285CA File Offset: 0x000267CA
		public DashboardPage CurrentDashboardPage
		{
			get
			{
				if (this.Pages == null || this.Pages.Count == 0 || this.CurrentPage > this.Pages.Count)
				{
					return null;
				}
				return this.Pages[this.CurrentPage];
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00028607 File Offset: 0x00026807
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0002860F File Offset: 0x0002680F
		public int CurrentPage
		{
			get
			{
				return this._CurrentPage;
			}
			set
			{
				this.ChangePage(value);
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0002861C File Offset: 0x0002681C
		private async Task ChangePage(int page)
		{
			this.RestartTimer();
			int currentPage = this._CurrentPage;
			this.btnNextPage.IsEnabled = false;
			this.btnPrevPage.IsEnabled = false;
			if (this.Pages != null && this._CurrentPage < this.Pages.Count && !this.Pages[this._CurrentPage].UpdateInBackground)
			{
				this.Pages[this._CurrentPage].Stop();
			}
			if (page >= this.TotalPages)
			{
				this._CurrentPage = 0;
			}
			else
			{
				this._CurrentPage = page;
			}
			this.OnPropertyChanged("CurrentPage");
			this.OnPropertyChanged("CurrentPagePlusOne");
			this.OnPropertyChanged("CurrentPageLabel");
			this.OnPropertyChanged("CurrentDashboardPage");
			SharedSettings.Current.DashboardLastPage = this.CurrentPage;
			this.BackgroundGrid.Children.Clear();
			try
			{
				DashboardPage newPage = this.Pages[this.CurrentPage];
				this.BackgroundGrid.Children.Add(newPage);
				if (SharedSettings.Current.DashboardAnimation)
				{
					await ViewExtensions.FadeTo(newPage, 0.0, 0U, null);
					await ViewExtensions.FadeTo(newPage, 1.0, 200U, null);
				}
				newPage = null;
			}
			catch (Exception)
			{
			}
			await this.Pages[this.CurrentPage].Start();
			this.btnNextPage.IsEnabled = true;
			this.btnPrevPage.IsEnabled = true;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00028667 File Offset: 0x00026867
		public int TotalPages
		{
			get
			{
				if (this.Pages != null)
				{
					return this.Pages.Count;
				}
				return 0;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0002867E File Offset: 0x0002687E
		public ObservableCollection<DashboardPage> Pages
		{
			get
			{
				return DashboardListViewModel.Current.Pages;
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0002868C File Offset: 0x0002688C
		private async Task LoadDashboardFromSettings()
		{
			this.activityFrame.IsVisible = true;
			await Task.Run(delegate
			{
				DashboardListViewModel.Current.LoadDashboardFromSettings();
			});
			this.OnPropertyChanged("CurrentPageLabel");
			this.BackgroundGrid.Children.Clear();
			this.CurrentPage = SharedSettings.Current.DashboardLastPage;
			this.gridButtons.BindingContext = this;
			this.activityFrame.IsVisible = false;
			this.ShouldLoadDashboardFromSettings = false;
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060003B6 RID: 950 RVA: 0x000286D0 File Offset: 0x000268D0
		// (remove) Token: 0x060003B7 RID: 951 RVA: 0x00028704 File Offset: 0x00026904
		public static event EventHandler<DashboardPage> DashboardLoaded
		{
			[CompilerGenerated]
			add
			{
				EventHandler<DashboardPage> eventHandler = DashboardXamlPage.DashboardLoaded;
				EventHandler<DashboardPage> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<DashboardPage> eventHandler3 = (EventHandler<DashboardPage>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<DashboardPage>>(ref DashboardXamlPage.DashboardLoaded, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<DashboardPage> eventHandler = DashboardXamlPage.DashboardLoaded;
				EventHandler<DashboardPage> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<DashboardPage> eventHandler3 = (EventHandler<DashboardPage>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<DashboardPage>>(ref DashboardXamlPage.DashboardLoaded, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00028738 File Offset: 0x00026938
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			this.RestartTimer();
			if (this.lvPagesLayout.IsOpen)
			{
				this.lvPagesLayout.IsOpen = false;
			}
			else if (this.pageSettingsPopupLayout.IsOpen)
			{
				this.pageSettingsPopupLayout.IsOpen = false;
			}
			else if (this.gridButtons.Opacity == 0.0)
			{
				this.ShowTopGrid();
			}
			else
			{
				DashboardXamlPage.Instance = null;
				try
				{
					foreach (DashboardPage dashboardPage in this.Pages)
					{
						dashboardPage.Stop();
					}
					foreach (DashboardPage dashboardPage2 in this.Pages)
					{
						foreach (DashboardItem dashboardItem in dashboardPage2.Items)
						{
							dashboardItem.Model.SelectedPID = PID.Empty;
						}
					}
				}
				catch (Exception)
				{
				}
				MainAppRequestProducer.Delegate = null;
				if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidUseFullscreen)
				{
					try
					{
						PlatformHelper.DroidService.Window_SetFullscreenOff();
					}
					catch (Exception)
					{
					}
				}
				base.Navigation.PopAsync();
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0002876F File Offset: 0x0002696F
		protected override bool OnBackButtonPressed()
		{
			this.btnBack_Clicked(this.btnBack, EventArgs.Empty);
			return true;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00028784 File Offset: 0x00026984
		public async void btnPrevPage_Clicked(object sender, EventArgs e)
		{
			await App.OBDReader.ClearRequestQueue();
			if (this.CurrentPage > 0)
			{
				this.CurrentPage--;
			}
			else
			{
				this.CurrentPage = this.TotalPages - 1;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000287BC File Offset: 0x000269BC
		public async void btnNextPage_Clicked(object sender, EventArgs e)
		{
			await App.OBDReader.ClearRequestQueue();
			if (this.CurrentPage < this.TotalPages - 1)
			{
				this.CurrentPage++;
			}
			else
			{
				this.CurrentPage = 0;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000287F4 File Offset: 0x000269F4
		private void btnSettings_Clicked(object sender, EventArgs e)
		{
			if (this.Pages == null || this.Pages.Count == 0 || this.CurrentPage >= this.Pages.Count || this.CurrentPage < 0)
			{
				return;
			}
			this.RestartTimer();
			if (this.Pages[this.CurrentPage] is Dash_CustomPage)
			{
				this.popupView.ContentTemplate = (DataTemplate)base.Resources["popupTemplateForCustomPage"];
			}
			else
			{
				this.popupView.ContentTemplate = (DataTemplate)base.Resources["popupTemplateForTemplatedPage"];
			}
			if (this.CurrentPage > 0 && this.CurrentPage < DashboardListViewModel.Current.Pages.Count)
			{
				this.popupView.BindingContext = DashboardListViewModel.Current.Pages[this.CurrentPage];
			}
			try
			{
				this.pageSettingsPopupLayout.Show(false);
			}
			catch (Exception)
			{
			}
			if (this.lvPagesLayout.IsOpen)
			{
				this.lvPagesLayout.IsOpen = false;
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00028910 File Offset: 0x00026B10
		private void btnShowSelector_Clicked(object sender, EventArgs e)
		{
			if (!this.lvPagesLayout.IsOpen)
			{
				this.lvPagesLayout.Show(false);
				this.pageSettingsPopupLayout.IsOpen = false;
				this.HideTopGrid();
				return;
			}
			this.lvPagesLayout.IsOpen = false;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0002894B File Offset: 0x00026B4B
		private void lvPages_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			this.CurrentPage = this.Pages.IndexOf(e.Item as DashboardPage);
			this.lvPagesLayout.IsOpen = false;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00028978 File Offset: 0x00026B78
		public string CurrentPageLabel
		{
			get
			{
				return this.CurrentPagePlusOne.ToString(CultureInfo.InvariantCulture) + "/" + this.TotalPages.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000289B8 File Offset: 0x00026BB8
		private async void btnMovePageRight_Clicked(object sender, EventArgs e)
		{
			if (this.CurrentPage == this.Pages.Count - 1)
			{
				if (this.Pages.Count > 1)
				{
					DashboardPage dashboardPage = this.Pages[0];
					DashboardPage dashboardPage2 = this.Pages[this.Pages.Count - 1];
					this.Pages[this.Pages.Count - 1] = dashboardPage;
					this.Pages[0] = dashboardPage2;
					this.btnNextPage_Clicked(this.btnPrevPage, null);
					DashboardListViewModel.Current.SaveDashboardToSettings();
				}
			}
			else
			{
				DashboardPage dashboardPage3 = this.Pages[this.CurrentPage + 1];
				DashboardPage dashboardPage4 = this.Pages[this.CurrentPage];
				this.Pages[this.CurrentPage] = dashboardPage3;
				this.Pages[this.CurrentPage + 1] = dashboardPage4;
				this.btnNextPage_Clicked(this.btnNextPage, null);
				DashboardListViewModel.Current.SaveDashboardToSettings();
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000289F0 File Offset: 0x00026BF0
		private async void btnMovePageLeft_Clicked(object sender, EventArgs e)
		{
			if (this.CurrentPage == 0)
			{
				if (this.Pages.Count > 1)
				{
					DashboardPage dashboardPage = this.Pages[0];
					DashboardPage dashboardPage2 = this.Pages[this.Pages.Count - 1];
					this.Pages[this.Pages.Count - 1] = dashboardPage;
					this.Pages[0] = dashboardPage2;
					this.btnPrevPage_Clicked(this.btnPrevPage, null);
					DashboardListViewModel.Current.SaveDashboardToSettings();
				}
			}
			else
			{
				DashboardPage dashboardPage3 = this.Pages[this.CurrentPage - 1];
				DashboardPage dashboardPage4 = this.Pages[this.CurrentPage];
				this.Pages[this.CurrentPage] = dashboardPage3;
				this.Pages[this.CurrentPage - 1] = dashboardPage4;
				this.btnPrevPage_Clicked(this.btnPrevPage, null);
				DashboardListViewModel.Current.SaveDashboardToSettings();
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00028A28 File Offset: 0x00026C28
		private async void btnDelPage_Clicked(object sender, EventArgs e)
		{
			if (DashboardListViewModel.Current.Pages.Count == 1)
			{
				await base.DisplayAlert(Translate.GetString("ios_DashboardEditor_LastPageTitle"), Translate.GetString("ios_DashboardEditor_LastPageText"), "OK");
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_DashboardEditor_DelPageTitle"), Translate.GetString("ios_DashboardEditor_DelPageText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					try
					{
						DashboardPage dashboardPage = this.Pages[this.CurrentPage];
						dashboardPage.Stop();
						DashboardListViewModel.Current.Pages.Remove(dashboardPage);
						if (this.CurrentPage >= 0 && this.CurrentPage < this.Pages.Count)
						{
							this.CurrentPage = this.CurrentPage;
						}
						else
						{
							this.CurrentPage = this.Pages.Count - 1;
						}
						DashboardListViewModel.Current.SaveDashboardToSettings();
						this.pageSettingsPopupLayout.IsOpen = false;
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00028A60 File Offset: 0x00026C60
		private async void btnEditPage_Clicked(object sender, EventArgs e)
		{
			try
			{
				DashboardPage dashboardPage = this.Pages[this.CurrentPage];
				DashboardEditorPage dashboardEditorPage = new DashboardEditorPage();
				dashboardEditorPage.BindingContext = dashboardPage;
				base.Navigation.PushAsync(dashboardEditorPage, true);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00028A98 File Offset: 0x00026C98
		private void btnAddItem_Clicked(object sender, EventArgs e)
		{
			if (this.CurrentPage >= 0 && this.CurrentPage < this.Pages.Count)
			{
				Dash_CustomPage dash_CustomPage = this.Pages[this.CurrentPage] as Dash_CustomPage;
				if (dash_CustomPage != null)
				{
					dash_CustomPage.AskUserToAddNewItemAsync(new Point(Math.Round(base.Width / 2.5, 0), Math.Round(base.Height / 2.5, 0)));
				}
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00028B14 File Offset: 0x00026D14
		private void btnAddPage_Clicked(object sender, EventArgs e)
		{
			bool flag = SharedSettings.Current.AdsProductPurchased || DashboardListViewModel.Current.Pages.Count < 3;
			if (flag)
			{
				DashboardPageAdder dashboardPageAdder = new DashboardPageAdder();
				base.Navigation.PushAsync(dashboardPageAdder);
				return;
			}
			base.DisplayAlert(string.Format(Translate.GetString("ios_DashboardMaxPageInFree"), 3), "", "OK");
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000027D4 File Offset: 0x000009D4
		private void BackgroundGrid_Tapped(object sender, TapEventArgs e)
		{
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00028B88 File Offset: 0x00026D88
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardXamlPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardXamlPage.xaml",
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
			Color backgroundColor = StaticLists.BackgroundColor;
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 5);
			Color color = new Color(0.6901960968971252, 0.6901960968971252, 0.6901960968971252, 1.0);
			Color color2 = new Color(0.3764705955982208, 0.3764705955982208, 0.3764705955982208, 1.0);
			double num = 300.0;
			double num2 = 300.0;
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 14);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 14);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 14);
			Setter setter5;
			VisualDiagnostics.RegisterSourceInfo(setter5 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 14);
			Setter setter6;
			VisualDiagnostics.RegisterSourceInfo(setter6 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			Setter setter7;
			VisualDiagnostics.RegisterSourceInfo(setter7 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Frame)), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 10);
			Setter setter8;
			VisualDiagnostics.RegisterSourceInfo(setter8 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 14);
			Setter setter9;
			VisualDiagnostics.RegisterSourceInfo(setter9 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 14);
			Setter setter10;
			VisualDiagnostics.RegisterSourceInfo(setter10 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 14);
			Setter setter11;
			VisualDiagnostics.RegisterSourceInfo(setter11 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 14);
			Setter setter12;
			VisualDiagnostics.RegisterSourceInfo(setter12 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			Setter setter13;
			VisualDiagnostics.RegisterSourceInfo(setter13 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 14);
			Style style2;
			VisualDiagnostics.RegisterSourceInfo(style2 = new Style(typeof(CachedImage)), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 10);
			Setter setter14;
			VisualDiagnostics.RegisterSourceInfo(setter14 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 14);
			Setter setter15;
			VisualDiagnostics.RegisterSourceInfo(setter15 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 14);
			Setter setter16;
			VisualDiagnostics.RegisterSourceInfo(setter16 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 14);
			Setter setter17;
			VisualDiagnostics.RegisterSourceInfo(setter17 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 14);
			Setter setter18;
			VisualDiagnostics.RegisterSourceInfo(setter18 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 14);
			Setter setter19;
			VisualDiagnostics.RegisterSourceInfo(setter19 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 14);
			Setter setter20;
			VisualDiagnostics.RegisterSourceInfo(setter20 = new Setter(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 14);
			Style style3;
			VisualDiagnostics.RegisterSourceInfo(style3 = new Style(typeof(Label)), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 10);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 10);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 10);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 18);
			CachedImage cachedImage;
			VisualDiagnostics.RegisterSourceInfo(cachedImage = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 22);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 14);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 293, 17);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 25);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 30);
			PopupView popupView;
			VisualDiagnostics.RegisterSourceInfo(popupView = new PopupView(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 302, 22);
			SfPopupLayout sfPopupLayout;
			VisualDiagnostics.RegisterSourceInfo(sfPopupLayout = new SfPopupLayout(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 17);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 14);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 370, 22);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 371, 22);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 372, 22);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 373, 22);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 374, 22);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 378, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 394, 21);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 396, 21);
			Color textColor = StaticLists.TextColor;
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 397, 21);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 387, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 418, 25);
			Color textColor2 = StaticLists.TextColor;
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 420, 25);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 412, 22);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 25);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 25);
			Color textColor3 = StaticLists.TextColor;
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 25);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 422, 22);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 440, 25);
			Color textColor4 = StaticLists.TextColor;
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 442, 25);
			LinkButton linkButton4;
			VisualDiagnostics.RegisterSourceInfo(linkButton4 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 433, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 407, 18);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 461, 30);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 451, 22);
			TapGestureRecognizer tapGestureRecognizer2;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 475, 30);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 465, 22);
			TapGestureRecognizer tapGestureRecognizer3;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer3 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 30);
			Image image3;
			VisualDiagnostics.RegisterSourceInfo(image3 = new Image(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 479, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 445, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 359, 14);
			DataTemplate dataTemplate4;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate4 = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 515, 30);
			PopupView popupView2;
			VisualDiagnostics.RegisterSourceInfo(popupView2 = new PopupView(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 504, 22);
			SfPopupLayout sfPopupLayout2;
			VisualDiagnostics.RegisterSourceInfo(sfPopupLayout2 = new SfPopupLayout(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 498, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("thisPage", this);
			if (this.StyleId == null)
			{
				this.StyleId = "thisPage";
			}
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			NameScope nameScope6 = new NameScope();
			NameScope nameScope7 = new NameScope();
			NameScope nameScope8 = new NameScope();
			NameScope nameScope9 = new NameScope();
			NameScope nameScope10 = new NameScope();
			NameScope nameScope11 = new NameScope();
			NameScope nameScope12 = new NameScope();
			NameScope nameScope13 = new NameScope();
			NameScope nameScope14 = new NameScope();
			NameScope nameScope15 = new NameScope();
			NameScope nameScope16 = new NameScope();
			NameScope nameScope17 = new NameScope();
			NameScope nameScope18 = new NameScope();
			NameScope nameScope19 = new NameScope();
			NameScope nameScope20 = new NameScope();
			NameScope nameScope21 = new NameScope();
			nameScope.RegisterName("LayoutRoot", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("imgBackground", cachedImage);
			if (cachedImage.StyleId == null)
			{
				cachedImage.StyleId = "imgBackground";
			}
			nameScope.RegisterName("BackgroundGrid", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "BackgroundGrid";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("lvPagesLayout", sfPopupLayout);
			if (sfPopupLayout.StyleId == null)
			{
				sfPopupLayout.StyleId = "lvPagesLayout";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			nameScope.RegisterName("gridButtons", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridButtons";
			}
			nameScope.RegisterName("gridButtonsBackground", boxView);
			if (boxView.StyleId == null)
			{
				boxView.StyleId = "gridButtonsBackground";
			}
			nameScope.RegisterName("btnBack", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "btnBack";
			}
			nameScope.RegisterName("panelPageSelector", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelPageSelector";
			}
			nameScope.RegisterName("btnPrevPage", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "btnPrevPage";
			}
			nameScope.RegisterName("labelSlash", linkButton3);
			if (linkButton3.StyleId == null)
			{
				linkButton3.StyleId = "labelSlash";
			}
			nameScope.RegisterName("btnNextPage", linkButton4);
			if (linkButton4.StyleId == null)
			{
				linkButton4.StyleId = "btnNextPage";
			}
			nameScope.RegisterName("panelSettingsInfoWhite", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "panelSettingsInfoWhite";
			}
			nameScope.RegisterName("btnSettings", image);
			if (image.StyleId == null)
			{
				image.StyleId = "btnSettings";
			}
			nameScope.RegisterName("btnMirror", image2);
			if (image2.StyleId == null)
			{
				image2.StyleId = "btnMirror";
			}
			nameScope.RegisterName("btnInfo", image3);
			if (image3.StyleId == null)
			{
				image3.StyleId = "btnInfo";
			}
			nameScope.RegisterName("pageSettingsPopupLayout", sfPopupLayout2);
			if (sfPopupLayout2.StyleId == null)
			{
				sfPopupLayout2.StyleId = "pageSettingsPopupLayout";
			}
			nameScope.RegisterName("popupView", popupView2);
			if (popupView2.StyleId == null)
			{
				popupView2.StyleId = "popupView";
			}
			this.thisPage = this;
			this.LayoutRoot = grid3;
			this.imgBackground = cachedImage;
			this.BackgroundGrid = grid;
			this.activityFrame = activityFrame;
			this.lvPagesLayout = sfPopupLayout;
			this.ad = complexAdView;
			this.gridButtons = grid2;
			this.gridButtonsBackground = boxView;
			this.btnBack = linkButton;
			this.panelPageSelector = stackLayout;
			this.btnPrevPage = linkButton2;
			this.labelSlash = linkButton3;
			this.btnNextPage = linkButton4;
			this.panelSettingsInfoWhite = stackLayout2;
			this.btnSettings = image;
			this.btnMirror = image2;
			this.btnInfo = image3;
			this.pageSettingsPopupLayout = sfPopupLayout2;
			this.popupView = popupView2;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 1);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Page_Appearing;
			bindingExtension.Source = backgroundColor;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			this.SetBinding(VisualElement.BackgroundColorProperty, bindingBase);
			this.Disappearing += this.Page_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, false);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources.Add("TopGridLightBackground", color);
			this.Resources.Add("TopGridDarkBackground", color2);
			this.Resources.Add("popupMaxWidth", num);
			this.Resources.Add("popupMaxHeight", num2);
			setter.Property = Layout.PaddingProperty;
			setter.Value = "0,3,0,0";
			setter.Value = new Thickness(0.0, 3.0, 0.0, 0.0);
			style.Setters.Add(setter);
			setter2.Property = Frame.BorderColorProperty;
			setter2.Value = "#C0C0C0";
			setter2.Value = new Color(0.7529411911964417, 0.7529411911964417, 0.7529411911964417, 1.0);
			style.Setters.Add(setter2);
			setter3.Property = VisualElement.BackgroundColorProperty;
			setter3.Value = "#C0C0C0";
			setter3.Value = new Color(0.7529411911964417, 0.7529411911964417, 0.7529411911964417, 1.0);
			style.Setters.Add(setter3);
			setter4.Property = Frame.HasShadowProperty;
			setter4.Value = "False";
			setter4.Value = false;
			style.Setters.Add(setter4);
			setter5.Property = View.MarginProperty;
			setter5.Value = "0";
			setter5.Value = new Thickness(0.0);
			style.Setters.Add(setter5);
			setter6.Property = View.HorizontalOptionsProperty;
			setter6.Value = "Center";
			setter6.Value = LayoutOptions.Center;
			style.Setters.Add(setter6);
			setter7.Property = VisualElement.MinimumHeightRequestProperty;
			setter7.Value = "60";
			setter7.Value = 60.0;
			style.Setters.Add(setter7);
			this.Resources.Add("pageSettingsFrameStyle", style);
			setter8.Property = Grid.RowProperty;
			setter8.Value = "0";
			setter8.Value = 0;
			style2.Setters.Add(setter8);
			setter9.Property = VisualElement.HeightRequestProperty;
			setter9.Value = "40";
			setter9.Value = 40.0;
			style2.Setters.Add(setter9);
			setter10.Property = VisualElement.WidthRequestProperty;
			setter10.Value = "40";
			setter10.Value = 40.0;
			style2.Setters.Add(setter10);
			setter11.Property = VisualElement.BackgroundColorProperty;
			setter11.Value = "Transparent";
			setter11.Value = Color.Transparent;
			style2.Setters.Add(setter11);
			setter12.Property = VisualElement.InputTransparentProperty;
			setter12.Value = "True";
			setter12.Value = true;
			style2.Setters.Add(setter12);
			setter13.Property = View.HorizontalOptionsProperty;
			setter13.Value = "Center";
			setter13.Value = LayoutOptions.Center;
			style2.Setters.Add(setter13);
			this.Resources.Add("pageSettingsImage", style2);
			setter14.Property = Grid.RowProperty;
			setter14.Value = "1";
			setter14.Value = 1;
			style3.Setters.Add(setter14);
			setter15.Property = VisualElement.InputTransparentProperty;
			setter15.Value = "True";
			setter15.Value = true;
			style3.Setters.Add(setter15);
			setter16.Property = Label.TextColorProperty;
			setter16.Value = "Black";
			setter16.Value = Color.Black;
			style3.Setters.Add(setter16);
			setter17.Property = Label.HorizontalTextAlignmentProperty;
			setter17.Value = "Center";
			setter17.Value = new TextAlignmentConverter().ConvertFromInvariantString("Center");
			style3.Setters.Add(setter17);
			setter18.Property = Label.LineBreakModeProperty;
			setter18.Value = "WordWrap";
			setter18.Value = 1;
			style3.Setters.Add(setter18);
			setter19.Property = Label.MaxLinesProperty;
			setter19.Value = "2";
			setter19.Value = 2;
			style3.Setters.Add(setter19);
			setter20.Property = View.MarginProperty;
			setter20.Value = "2,0";
			setter20.Value = new Thickness(2.0, 0.0);
			style3.Setters.Add(setter20);
			this.Resources.Add("pageSettingsLabel", style3);
			IDataTemplate dataTemplate5 = dataTemplate;
			DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44 <InitializeComponent>_anonXamlCDataTemplate_ = new DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44();
			object[] array = new object[0 + 2];
			array[0] = dataTemplate;
			array[1] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			this.Resources.Add("popupTemplateForCustomPage", dataTemplate);
			IDataTemplate dataTemplate6 = dataTemplate2;
			DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45 <InitializeComponent>_anonXamlCDataTemplate_2 = new DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45();
			object[] array2 = new object[0 + 2];
			array2[0] = dataTemplate2;
			array2[1] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			this.Resources.Add("popupTemplateForTemplatedPage", dataTemplate2);
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			grid3.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid3.SetValue(Grid.ColumnSpacingProperty, 0.0);
			grid3.SetValue(Grid.RowSpacingProperty, 0.0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			cachedImage.SetValue(Grid.RowProperty, 0);
			cachedImage.SetValue(Grid.RowSpanProperty, 2);
			cachedImage.SetValue(CachedImage.AspectProperty, 1);
			cachedImage.SetValue(CachedImage.CacheTypeProperty, new CacheType?(1));
			cachedImage.SetValue(CachedImage.DownsampleToViewSizeProperty, true);
			cachedImage.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid3.Children.Add(cachedImage);
			grid.SetValue(Grid.RowProperty, 1);
			grid.SetValue(View.MarginProperty, new Thickness(0.0, 0.0));
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			grid.Swiped += this.BackgroundGrid_Swiped;
			grid.Tapped += this.BackgroundGrid_Tapped;
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			grid3.Children.Add(grid);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			translate.Text = "ios_LoadingDashboard";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = activityFrame;
			array3[1] = grid3;
			array3[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array3, ActivityFrame.TextProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(293, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			activityFrame.Text = obj2;
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(activityFrame);
			sfPopupLayout.SetValue(Grid.RowProperty, 1);
			sfPopupLayout.SetValue(SfPopupLayout.IsOpenProperty, false);
			sfPopupLayout.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			popupView.SetValue(PopupView.AnimationModeProperty, 4);
			popupView.SetValue(VisualElement.BackgroundColorProperty, Color.White);
			translate2.Text = "ios_MainPage_TileDashboard";
			IMarkupExtension markupExtension2 = translate2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = popupView;
			array4[1] = sfPopupLayout;
			array4[2] = grid3;
			array4[3] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, PopupView.HeaderTitleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver2.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(305, 25)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			popupView.HeaderTitle = obj4;
			popupView.SetValue(PopupView.ShowCloseButtonProperty, true);
			popupView.SetValue(PopupView.ShowFooterProperty, false);
			popupView.SetValue(PopupView.ShowHeaderProperty, true);
			IDataTemplate dataTemplate7 = dataTemplate3;
			DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_46 <InitializeComponent>_anonXamlCDataTemplate_3 = new DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_46();
			object[] array5 = new object[0 + 5];
			array5[0] = dataTemplate3;
			array5[1] = popupView;
			array5[2] = sfPopupLayout;
			array5[3] = grid3;
			array5[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array5;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate7.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			popupView.SetValue(PopupView.ContentTemplateProperty, dataTemplate3);
			sfPopupLayout.SetValue(SfPopupLayout.PopupViewProperty, popupView);
			grid3.Children.Add(sfPopupLayout);
			complexAdView.SetValue(Grid.RowProperty, 2);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = complexAdView;
			array6[1] = grid3;
			array6[2] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver3.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver3.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver3.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(353, 17)));
			DynamicResource dynamicResource = markupExtension3.ProvideValue(xamlServiceProvider3);
			complexAdView.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid3.Children.Add(complexAdView);
			grid2.SetValue(Grid.RowProperty, 1);
			grid2.SetValue(Layout.PaddingProperty, new Thickness(0.0, 0.0));
			grid2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			grid2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			grid2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			boxView.SetValue(Grid.ColumnProperty, 0);
			boxView.SetValue(Grid.ColumnSpanProperty, 5);
			boxView.SetValue(View.MarginProperty, new Thickness(-5.0, -5.0));
			boxView.SetValue(VisualElement.BackgroundColorProperty, Color.Black);
			boxView.SetValue(VisualElement.OpacityProperty, 0.9);
			boxView.SetValue(BoxView.ColorProperty, Color.Black);
			grid2.Children.Add(boxView);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 0.0, 0.0));
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton.Clicked += this.btnBack_Clicked;
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = linkButton;
			array7[1] = grid2;
			array7[2] = grid3;
			array7[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array7, Button.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver4.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver4.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver4.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(394, 21)));
			DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.SetDynamicResource(Button.FontSizeProperty, dynamicResource2.Key);
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			translate3.Text = "ios_Back";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = linkButton;
			array8[1] = grid2;
			array8[2] = grid3;
			array8[3] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver5.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver5.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver5.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(396, 21)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			linkButton.Text = obj8;
			bindingExtension2.Source = textColor;
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			linkButton.SetBinding(Button.TextColorProperty, bindingBase2);
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid2.Children.Add(linkButton);
			stackLayout.SetValue(Grid.ColumnProperty, 2);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			linkButton2.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton2.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton2.Clicked += this.btnPrevPage_Clicked;
			dynamicResourceExtension3.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = linkButton2;
			array9[1] = stackLayout;
			array9[2] = grid2;
			array9[3] = grid3;
			array9[4] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array9, Button.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver6.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver6.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver6.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(418, 25)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			linkButton2.SetDynamicResource(Button.FontSizeProperty, dynamicResource3.Key);
			linkButton2.SetValue(Button.TextProperty, " << ");
			bindingExtension3.Source = textColor2;
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			linkButton2.SetBinding(Button.TextColorProperty, bindingBase3);
			stackLayout.Children.Add(linkButton2);
			linkButton3.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			linkButton3.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton3.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton3.Clicked += this.btnShowSelector_Clicked;
			dynamicResourceExtension4.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = linkButton3;
			array10[1] = stackLayout;
			array10[2] = grid2;
			array10[3] = grid3;
			array10[4] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array10, Button.FontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver7.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver7.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver7.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(428, 25)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton3.SetDynamicResource(Button.FontSizeProperty, dynamicResource4.Key);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "CurrentPageLabel";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			linkButton3.SetBinding(Button.TextProperty, bindingBase4);
			bindingExtension5.Source = textColor3;
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			linkButton3.SetBinding(Button.TextColorProperty, bindingBase5);
			linkButton3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout.Children.Add(linkButton3);
			linkButton4.SetValue(Grid.ColumnProperty, 3);
			linkButton4.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton4.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton4.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton4.Clicked += this.btnNextPage_Clicked;
			dynamicResourceExtension5.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = linkButton4;
			array11[1] = stackLayout;
			array11[2] = grid2;
			array11[3] = grid3;
			array11[4] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array11, Button.FontSizeProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver8.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
			xmlNamespaceResolver8.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver8.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver8.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DashboardXamlPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(440, 25)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton4.SetDynamicResource(Button.FontSizeProperty, dynamicResource5.Key);
			linkButton4.SetValue(Button.TextProperty, " >> ");
			bindingExtension6.Source = textColor4;
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			linkButton4.SetBinding(Button.TextColorProperty, bindingBase6);
			stackLayout.Children.Add(linkButton4);
			grid2.Children.Add(stackLayout);
			stackLayout2.SetValue(Grid.ColumnProperty, 4);
			stackLayout2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			image.SetValue(View.MarginProperty, new Thickness(0.0));
			image.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			image.SetValue(VisualElement.HeightRequestProperty, 35.0);
			image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			image.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_settings.png"));
			image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			image.SetValue(VisualElement.WidthRequestProperty, 40.0);
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.btnSettings_Clicked;
			image.GestureRecognizers.Add(tapGestureRecognizer);
			stackLayout2.Children.Add(image);
			image2.SetValue(View.MarginProperty, new Thickness(0.0, 5.0));
			image2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			image2.SetValue(VisualElement.HeightRequestProperty, 35.0);
			image2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			image2.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_mirror.png"));
			image2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			image2.SetValue(VisualElement.WidthRequestProperty, 40.0);
			tapGestureRecognizer2.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer2.Tapped += this.btnMirror_Clicked;
			image2.GestureRecognizers.Add(tapGestureRecognizer2);
			stackLayout2.Children.Add(image2);
			image3.SetValue(Grid.ColumnProperty, 5);
			image3.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 5.0, 0.0));
			image3.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			image3.SetValue(VisualElement.HeightRequestProperty, 35.0);
			image3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			image3.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_info.png"));
			image3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			image3.SetValue(VisualElement.WidthRequestProperty, 40.0);
			tapGestureRecognizer3.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer3.Tapped += this.btnInfo_Clicked;
			image3.GestureRecognizers.Add(tapGestureRecognizer3);
			stackLayout2.Children.Add(image3);
			grid2.Children.Add(stackLayout2);
			grid3.Children.Add(grid2);
			sfPopupLayout2.SetValue(SfPopupLayout.IsOpenProperty, false);
			sfPopupLayout2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			popupView2.SetValue(PopupView.AnimationModeProperty, 4);
			popupView2.SetValue(PopupView.AutoSizeModeProperty, 2);
			popupView2.SetValue(VisualElement.BackgroundColorProperty, new Color(0.7529411911964417, 0.7529411911964417, 0.7529411911964417, 1.0));
			popupView2.SetValue(PopupView.HeaderTitleProperty, "");
			popupView2.SetValue(PopupView.ShowCloseButtonProperty, true);
			popupView2.SetValue(PopupView.ShowFooterProperty, true);
			popupView2.SetValue(PopupView.ShowHeaderProperty, false);
			IDataTemplate dataTemplate8 = dataTemplate4;
			DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_48 <InitializeComponent>_anonXamlCDataTemplate_4 = new DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_48();
			object[] array12 = new object[0 + 5];
			array12[0] = dataTemplate4;
			array12[1] = popupView2;
			array12[2] = sfPopupLayout2;
			array12[3] = grid3;
			array12[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_4.parentValues = array12;
			<InitializeComponent>_anonXamlCDataTemplate_4.root = this;
			dataTemplate8.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_4.LoadDataTemplate);
			popupView2.SetValue(PopupView.FooterTemplateProperty, dataTemplate4);
			sfPopupLayout2.SetValue(SfPopupLayout.PopupViewProperty, popupView2);
			grid3.Children.Add(sfPopupLayout2);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0002C0A4 File Offset: 0x0002A2A4
		[CompilerGenerated]
		private void <Timer_Elapsed>b__14_0()
		{
			this.HideTopGrid();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0002C0B0 File Offset: 0x0002A2B0
		[CompilerGenerated]
		private async void <Page_Appearing>b__23_0()
		{
			this.gridButtons.IsEnabled = false;
			await this.LoadDashboardFromSettings();
			this.labelSlash.BindingContext = this;
			this.HUDMode = SharedSettings.Current.DashboardHUDMode;
			this.gridButtons.IsEnabled = true;
			try
			{
				EventHandler<DashboardPage> dashboardLoaded = DashboardXamlPage.DashboardLoaded;
				if (dashboardLoaded != null)
				{
					dashboardLoaded(this, this.CurrentDashboardPage);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0002C0E8 File Offset: 0x0002A2E8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardXamlPage>(this, typeof(DashboardXamlPage));
			this.thisPage = NameScopeExtensions.FindByName<ContentPage>(this, "thisPage");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.imgBackground = NameScopeExtensions.FindByName<CachedImage>(this, "imgBackground");
			this.BackgroundGrid = NameScopeExtensions.FindByName<Grid>(this, "BackgroundGrid");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.lvPagesLayout = NameScopeExtensions.FindByName<SfPopupLayout>(this, "lvPagesLayout");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.gridButtonsBackground = NameScopeExtensions.FindByName<BoxView>(this, "gridButtonsBackground");
			this.btnBack = NameScopeExtensions.FindByName<LinkButton>(this, "btnBack");
			this.panelPageSelector = NameScopeExtensions.FindByName<StackLayout>(this, "panelPageSelector");
			this.btnPrevPage = NameScopeExtensions.FindByName<LinkButton>(this, "btnPrevPage");
			this.labelSlash = NameScopeExtensions.FindByName<LinkButton>(this, "labelSlash");
			this.btnNextPage = NameScopeExtensions.FindByName<LinkButton>(this, "btnNextPage");
			this.panelSettingsInfoWhite = NameScopeExtensions.FindByName<StackLayout>(this, "panelSettingsInfoWhite");
			this.btnSettings = NameScopeExtensions.FindByName<Image>(this, "btnSettings");
			this.btnMirror = NameScopeExtensions.FindByName<Image>(this, "btnMirror");
			this.btnInfo = NameScopeExtensions.FindByName<Image>(this, "btnInfo");
			this.pageSettingsPopupLayout = NameScopeExtensions.FindByName<SfPopupLayout>(this, "pageSettingsPopupLayout");
			this.popupView = NameScopeExtensions.FindByName<PopupView>(this, "popupView");
		}

		// Token: 0x04000270 RID: 624
		[CompilerGenerated]
		private static DashboardXamlPage <Instance>k__BackingField;

		// Token: 0x04000271 RID: 625
		public bool ShouldLoadDashboardFromSettings;

		// Token: 0x04000272 RID: 626
		private double hideInterval = 5000.0;

		// Token: 0x04000273 RID: 627
		private Timer timer;

		// Token: 0x04000274 RID: 628
		private int originalStatusBarColor;

		// Token: 0x04000275 RID: 629
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04000276 RID: 630
		private int _CurrentPage;

		// Token: 0x04000277 RID: 631
		[CompilerGenerated]
		private static EventHandler<DashboardPage> DashboardLoaded;

		// Token: 0x04000278 RID: 632
		private const int MAX_FREE_PAGES = 3;

		// Token: 0x04000279 RID: 633
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage thisPage;

		// Token: 0x0400027A RID: 634
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x0400027B RID: 635
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CachedImage imgBackground;

		// Token: 0x0400027C RID: 636
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid BackgroundGrid;

		// Token: 0x0400027D RID: 637
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0400027E RID: 638
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfPopupLayout lvPagesLayout;

		// Token: 0x0400027F RID: 639
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x04000280 RID: 640
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04000281 RID: 641
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView gridButtonsBackground;

		// Token: 0x04000282 RID: 642
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnBack;

		// Token: 0x04000283 RID: 643
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelPageSelector;

		// Token: 0x04000284 RID: 644
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPrevPage;

		// Token: 0x04000285 RID: 645
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton labelSlash;

		// Token: 0x04000286 RID: 646
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnNextPage;

		// Token: 0x04000287 RID: 647
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelSettingsInfoWhite;

		// Token: 0x04000288 RID: 648
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnSettings;

		// Token: 0x04000289 RID: 649
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnMirror;

		// Token: 0x0400028A RID: 650
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnInfo;

		// Token: 0x0400028B RID: 651
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfPopupLayout pageSettingsPopupLayout;

		// Token: 0x0400028C RID: 652
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private PopupView popupView;

		// Token: 0x020000BA RID: 186
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<Page_Appearing>b__23_0>d : IAsyncStateMachine
		{
			// Token: 0x060003CB RID: 971 RVA: 0x0002C25C File Offset: 0x0002A45C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dashboardXamlPage.gridButtons.IsEnabled = false;
						taskAwaiter = dashboardXamlPage.LoadDashboardFromSettings().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardXamlPage.<<Page_Appearing>b__23_0>d>(ref taskAwaiter, ref this);
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
					dashboardXamlPage.labelSlash.BindingContext = dashboardXamlPage;
					dashboardXamlPage.HUDMode = SharedSettings.Current.DashboardHUDMode;
					dashboardXamlPage.gridButtons.IsEnabled = true;
					try
					{
						EventHandler<DashboardPage> dashboardLoaded = DashboardXamlPage.DashboardLoaded;
						if (dashboardLoaded != null)
						{
							dashboardLoaded(dashboardXamlPage, dashboardXamlPage.CurrentDashboardPage);
						}
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

			// Token: 0x060003CC RID: 972 RVA: 0x0002C370 File Offset: 0x0002A570
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400028D RID: 653
			public int <>1__state;

			// Token: 0x0400028E RID: 654
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400028F RID: 655
			public DashboardXamlPage <>4__this;

			// Token: 0x04000290 RID: 656
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000BB RID: 187
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060003CD RID: 973 RVA: 0x0002C37E File Offset: 0x0002A57E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060003CE RID: 974 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060003CF RID: 975 RVA: 0x0002C38A File Offset: 0x0002A58A
			internal void <LoadDashboardFromSettings>b__42_0()
			{
				DashboardListViewModel.Current.LoadDashboardFromSettings();
			}

			// Token: 0x04000291 RID: 657
			public static readonly DashboardXamlPage.<>c <>9 = new DashboardXamlPage.<>c();

			// Token: 0x04000292 RID: 658
			public static Action <>9__42_0;
		}

		// Token: 0x020000BC RID: 188
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ChangePage>d__37 : IAsyncStateMachine
		{
			// Token: 0x060003D0 RID: 976 RVA: 0x0002C398 File Offset: 0x0002A598
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num > 1)
					{
						if (num == 2)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_028C;
						}
						dashboardXamlPage.RestartTimer();
						int currentPage = dashboardXamlPage._CurrentPage;
						dashboardXamlPage.btnNextPage.IsEnabled = false;
						dashboardXamlPage.btnPrevPage.IsEnabled = false;
						if (dashboardXamlPage.Pages != null && dashboardXamlPage._CurrentPage < dashboardXamlPage.Pages.Count && !dashboardXamlPage.Pages[dashboardXamlPage._CurrentPage].UpdateInBackground)
						{
							dashboardXamlPage.Pages[dashboardXamlPage._CurrentPage].Stop();
						}
						if (page >= dashboardXamlPage.TotalPages)
						{
							dashboardXamlPage._CurrentPage = 0;
						}
						else
						{
							dashboardXamlPage._CurrentPage = page;
						}
						dashboardXamlPage.OnPropertyChanged("CurrentPage");
						dashboardXamlPage.OnPropertyChanged("CurrentPagePlusOne");
						dashboardXamlPage.OnPropertyChanged("CurrentPageLabel");
						dashboardXamlPage.OnPropertyChanged("CurrentDashboardPage");
						SharedSettings.Current.DashboardLastPage = dashboardXamlPage.CurrentPage;
						dashboardXamlPage.BackgroundGrid.Children.Clear();
					}
					try
					{
						TaskAwaiter<bool> taskAwaiter3;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<bool> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<bool>);
								num2 = -1;
								goto IL_0217;
							}
							newPage = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage];
							dashboardXamlPage.BackgroundGrid.Children.Add(newPage);
							if (!SharedSettings.Current.DashboardAnimation)
							{
								goto IL_021F;
							}
							taskAwaiter3 = ViewExtensions.FadeTo(newPage, 0.0, 0U, null).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardXamlPage.<ChangePage>d__37>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						taskAwaiter3.GetResult();
						taskAwaiter3 = ViewExtensions.FadeTo(newPage, 1.0, 200U, null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardXamlPage.<ChangePage>d__37>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0217:
						taskAwaiter3.GetResult();
						IL_021F:
						newPage = null;
					}
					catch (Exception)
					{
					}
					taskAwaiter = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage].Start().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardXamlPage.<ChangePage>d__37>(ref taskAwaiter, ref this);
						return;
					}
					IL_028C:
					taskAwaiter.GetResult();
					dashboardXamlPage.btnNextPage.IsEnabled = true;
					dashboardXamlPage.btnPrevPage.IsEnabled = true;
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

			// Token: 0x060003D1 RID: 977 RVA: 0x0002C6B4 File Offset: 0x0002A8B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000293 RID: 659
			public int <>1__state;

			// Token: 0x04000294 RID: 660
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000295 RID: 661
			public DashboardXamlPage <>4__this;

			// Token: 0x04000296 RID: 662
			public int page;

			// Token: 0x04000297 RID: 663
			private DashboardPage <newPage>5__2;

			// Token: 0x04000298 RID: 664
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000299 RID: 665
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020000BD RID: 189
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_SizeChanged>d__16 : IAsyncStateMachine
		{
			// Token: 0x060003D2 RID: 978 RVA: 0x0002C6C4 File Offset: 0x0002A8C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (dashboardXamlPage.Width > dashboardXamlPage.Height)
						{
							dashboardXamlPage.Resources["popupMaxWidth"] = dashboardXamlPage.Width * 0.75;
							dashboardXamlPage.Resources["popupMaxHeight"] = dashboardXamlPage.Height * 0.8;
						}
						else
						{
							dashboardXamlPage.Resources["popupMaxWidth"] = dashboardXamlPage.Width * 0.75;
							dashboardXamlPage.Resources["popupMaxHeight"] = dashboardXamlPage.Height * 0.75;
						}
						if (dashboardXamlPage.gridButtons.Opacity > 0.0 && Device.RuntimePlatform == "iOS" && DeviceDisplay.MainDisplayInfo.Orientation == 1)
						{
							Page.SetUseSafeArea(dashboardXamlPage.On<iOS>(), true);
						}
						if (!dashboardXamlPage.pageSettingsPopupLayout.IsOpen)
						{
							goto IL_017F;
						}
						dashboardXamlPage.pageSettingsPopupLayout.IsOpen = false;
						taskAwaiter = Task.Delay(50).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardXamlPage.<Handle_SizeChanged>d__16>(ref taskAwaiter, ref this);
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
					dashboardXamlPage.btnSettings_Clicked(dashboardXamlPage.btnSettings, EventArgs.Empty);
					IL_017F:;
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

			// Token: 0x060003D3 RID: 979 RVA: 0x0002C89C File Offset: 0x0002AA9C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400029A RID: 666
			public int <>1__state;

			// Token: 0x0400029B RID: 667
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400029C RID: 668
			public DashboardXamlPage <>4__this;

			// Token: 0x0400029D RID: 669
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000BE RID: 190
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <HideTopGrid>d__6 : IAsyncStateMachine
		{
			// Token: 0x060003D4 RID: 980 RVA: 0x0002C8AC File Offset: 0x0002AAAC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_0160;
						}
						if (!SharedSettings.Current.DashboardHideTopControls || dashboardXamlPage.gridButtons.Opacity != 1.0 || dashboardXamlPage.pageSettingsPopupLayout.IsOpen)
						{
							goto IL_019B;
						}
						if (Device.RuntimePlatform == "iOS")
						{
							Page.SetUseSafeArea(dashboardXamlPage.On<iOS>(), !SharedSettings.Current.iOSDashboardUseFullscreen);
						}
						if (!SharedSettings.Current.DashboardAnimation)
						{
							dashboardXamlPage.gridButtons.Opacity = 0.0;
							goto IL_017E;
						}
						taskAwaiter = ViewExtensions.FadeTo(dashboardXamlPage.gridButtons, 1.0, 0U, null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardXamlPage.<HideTopGrid>d__6>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					taskAwaiter = ViewExtensions.FadeTo(dashboardXamlPage.gridButtons, 0.0, 250U, null).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardXamlPage.<HideTopGrid>d__6>(ref taskAwaiter, ref this);
						return;
					}
					IL_0160:
					taskAwaiter.GetResult();
					IL_017E:
					for (int i = 0; i < 3; i++)
					{
						dashboardXamlPage.LayoutRoot.LowerChild(dashboardXamlPage.gridButtons);
					}
					IL_019B:;
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

			// Token: 0x060003D5 RID: 981 RVA: 0x0002CAA0 File Offset: 0x0002ACA0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400029E RID: 670
			public int <>1__state;

			// Token: 0x0400029F RID: 671
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x040002A0 RID: 672
			public DashboardXamlPage <>4__this;

			// Token: 0x040002A1 RID: 673
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020000BF RID: 191
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadDashboardFromSettings>d__42 : IAsyncStateMachine
		{
			// Token: 0x060003D6 RID: 982 RVA: 0x0002CAB0 File Offset: 0x0002ACB0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dashboardXamlPage.activityFrame.IsVisible = true;
						taskAwaiter = Task.Run(delegate
						{
							DashboardListViewModel.Current.LoadDashboardFromSettings();
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardXamlPage.<LoadDashboardFromSettings>d__42>(ref taskAwaiter, ref this);
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
					dashboardXamlPage.OnPropertyChanged("CurrentPageLabel");
					dashboardXamlPage.BackgroundGrid.Children.Clear();
					dashboardXamlPage.CurrentPage = SharedSettings.Current.DashboardLastPage;
					dashboardXamlPage.gridButtons.BindingContext = dashboardXamlPage;
					dashboardXamlPage.activityFrame.IsVisible = false;
					dashboardXamlPage.ShouldLoadDashboardFromSettings = false;
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

			// Token: 0x060003D7 RID: 983 RVA: 0x0002CBDC File Offset: 0x0002ADDC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002A2 RID: 674
			public int <>1__state;

			// Token: 0x040002A3 RID: 675
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040002A4 RID: 676
			public DashboardXamlPage <>4__this;

			// Token: 0x040002A5 RID: 677
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000C0 RID: 192
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShowTopGrid>d__7 : IAsyncStateMachine
		{
			// Token: 0x060003D8 RID: 984 RVA: 0x0002CBEC File Offset: 0x0002ADEC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_015A;
						}
						if (!SharedSettings.Current.DashboardHideTopControls)
						{
							goto IL_0199;
						}
						dashboardXamlPage.RestartTimer();
						if (dashboardXamlPage.gridButtons.Opacity != 0.0)
						{
							goto IL_0199;
						}
						if (Device.RuntimePlatform == "iOS" && DeviceDisplay.MainDisplayInfo.Orientation == 1)
						{
							Page.SetUseSafeArea(dashboardXamlPage.On<iOS>(), true);
						}
						if (!SharedSettings.Current.DashboardAnimation)
						{
							dashboardXamlPage.gridButtons.Opacity = 1.0;
							goto IL_0178;
						}
						taskAwaiter = ViewExtensions.FadeTo(dashboardXamlPage.gridButtons, 0.0, 0U, null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardXamlPage.<ShowTopGrid>d__7>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					taskAwaiter = ViewExtensions.FadeTo(dashboardXamlPage.gridButtons, 1.0, 250U, null).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardXamlPage.<ShowTopGrid>d__7>(ref taskAwaiter, ref this);
						return;
					}
					IL_015A:
					taskAwaiter.GetResult();
					IL_0178:
					for (int i = 0; i < 3; i++)
					{
						dashboardXamlPage.LayoutRoot.RaiseChild(dashboardXamlPage.gridButtons);
					}
					IL_0199:;
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

			// Token: 0x060003D9 RID: 985 RVA: 0x0002CDDC File Offset: 0x0002AFDC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002A6 RID: 678
			public int <>1__state;

			// Token: 0x040002A7 RID: 679
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x040002A8 RID: 680
			public DashboardXamlPage <>4__this;

			// Token: 0x040002A9 RID: 681
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020000C1 RID: 193
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__46 : IAsyncStateMachine
		{
			// Token: 0x060003DA RID: 986 RVA: 0x0002CDEC File Offset: 0x0002AFEC
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					dashboardXamlPage.RestartTimer();
					if (dashboardXamlPage.lvPagesLayout.IsOpen)
					{
						dashboardXamlPage.lvPagesLayout.IsOpen = false;
					}
					else if (dashboardXamlPage.pageSettingsPopupLayout.IsOpen)
					{
						dashboardXamlPage.pageSettingsPopupLayout.IsOpen = false;
					}
					else if (dashboardXamlPage.gridButtons.Opacity == 0.0)
					{
						dashboardXamlPage.ShowTopGrid();
					}
					else
					{
						DashboardXamlPage.Instance = null;
						try
						{
							IEnumerator<DashboardPage> enumerator = dashboardXamlPage.Pages.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DashboardPage dashboardPage = enumerator.Current;
									dashboardPage.Stop();
								}
							}
							finally
							{
								if (num < 0 && enumerator != null)
								{
									enumerator.Dispose();
								}
							}
							enumerator = dashboardXamlPage.Pages.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DashboardPage dashboardPage2 = enumerator.Current;
									IEnumerator<DashboardItem> enumerator2 = dashboardPage2.Items.GetEnumerator();
									try
									{
										while (enumerator2.MoveNext())
										{
											DashboardItem dashboardItem = enumerator2.Current;
											dashboardItem.Model.SelectedPID = PID.Empty;
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
							}
							finally
							{
								if (num < 0 && enumerator != null)
								{
									enumerator.Dispose();
								}
							}
						}
						catch (Exception)
						{
						}
						MainAppRequestProducer.Delegate = null;
						if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidUseFullscreen)
						{
							try
							{
								PlatformHelper.DroidService.Window_SetFullscreenOff();
							}
							catch (Exception)
							{
							}
						}
						dashboardXamlPage.Navigation.PopAsync();
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

			// Token: 0x060003DB RID: 987 RVA: 0x0002D004 File Offset: 0x0002B204
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002AA RID: 682
			public int <>1__state;

			// Token: 0x040002AB RID: 683
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002AC RID: 684
			public DashboardXamlPage <>4__this;
		}

		// Token: 0x020000C2 RID: 194
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDelPage_Clicked>d__57 : IAsyncStateMachine
		{
			// Token: 0x060003DC RID: 988 RVA: 0x0002D014 File Offset: 0x0002B214
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						TaskAwaiter<bool> taskAwaiter5;
						if (num != 1)
						{
							if (DashboardListViewModel.Current.Pages.Count == 1)
							{
								taskAwaiter3 = dashboardXamlPage.DisplayAlert(Translate.GetString("ios_DashboardEditor_LastPageTitle"), Translate.GetString("ios_DashboardEditor_LastPageText"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardXamlPage.<btnDelPage_Clicked>d__57>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0097;
							}
							else
							{
								taskAwaiter5 = dashboardXamlPage.DisplayAlert(Translate.GetString("ios_DashboardEditor_DelPageTitle"), Translate.GetString("ios_DashboardEditor_DelPageText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardXamlPage.<btnDelPage_Clicked>d__57>(ref taskAwaiter5, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						if (taskAwaiter5.GetResult())
						{
							try
							{
								DashboardPage dashboardPage = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage];
								dashboardPage.Stop();
								DashboardListViewModel.Current.Pages.Remove(dashboardPage);
								if (dashboardXamlPage.CurrentPage >= 0 && dashboardXamlPage.CurrentPage < dashboardXamlPage.Pages.Count)
								{
									dashboardXamlPage.CurrentPage = dashboardXamlPage.CurrentPage;
								}
								else
								{
									dashboardXamlPage.CurrentPage = dashboardXamlPage.Pages.Count - 1;
								}
								DashboardListViewModel.Current.SaveDashboardToSettings();
								dashboardXamlPage.pageSettingsPopupLayout.IsOpen = false;
							}
							catch (Exception)
							{
							}
							goto IL_01AF;
						}
						goto IL_01AF;
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					IL_0097:
					taskAwaiter3.GetResult();
					IL_01AF:;
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

			// Token: 0x060003DD RID: 989 RVA: 0x0002D234 File Offset: 0x0002B434
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002AD RID: 685
			public int <>1__state;

			// Token: 0x040002AE RID: 686
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002AF RID: 687
			public DashboardXamlPage <>4__this;

			// Token: 0x040002B0 RID: 688
			private TaskAwaiter <>u__1;

			// Token: 0x040002B1 RID: 689
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x020000C3 RID: 195
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnEditPage_Clicked>d__58 : IAsyncStateMachine
		{
			// Token: 0x060003DE RID: 990 RVA: 0x0002D244 File Offset: 0x0002B444
			void IAsyncStateMachine.MoveNext()
			{
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					try
					{
						DashboardPage dashboardPage = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage];
						DashboardEditorPage dashboardEditorPage = new DashboardEditorPage();
						dashboardEditorPage.BindingContext = dashboardPage;
						dashboardXamlPage.Navigation.PushAsync(dashboardEditorPage, true);
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

			// Token: 0x060003DF RID: 991 RVA: 0x0002D2D4 File Offset: 0x0002B4D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002B2 RID: 690
			public int <>1__state;

			// Token: 0x040002B3 RID: 691
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002B4 RID: 692
			public DashboardXamlPage <>4__this;
		}

		// Token: 0x020000C4 RID: 196
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnMovePageLeft_Clicked>d__56 : IAsyncStateMachine
		{
			// Token: 0x060003E0 RID: 992 RVA: 0x0002D2E4 File Offset: 0x0002B4E4
			void IAsyncStateMachine.MoveNext()
			{
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					if (dashboardXamlPage.CurrentPage == 0)
					{
						if (dashboardXamlPage.Pages.Count > 1)
						{
							DashboardPage dashboardPage = dashboardXamlPage.Pages[0];
							DashboardPage dashboardPage2 = dashboardXamlPage.Pages[dashboardXamlPage.Pages.Count - 1];
							dashboardXamlPage.Pages[dashboardXamlPage.Pages.Count - 1] = dashboardPage;
							dashboardXamlPage.Pages[0] = dashboardPage2;
							dashboardXamlPage.btnPrevPage_Clicked(dashboardXamlPage.btnPrevPage, null);
							DashboardListViewModel.Current.SaveDashboardToSettings();
						}
					}
					else
					{
						DashboardPage dashboardPage3 = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage - 1];
						DashboardPage dashboardPage4 = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage];
						dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage] = dashboardPage3;
						dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage - 1] = dashboardPage4;
						dashboardXamlPage.btnPrevPage_Clicked(dashboardXamlPage.btnPrevPage, null);
						DashboardListViewModel.Current.SaveDashboardToSettings();
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

			// Token: 0x060003E1 RID: 993 RVA: 0x0002D41C File Offset: 0x0002B61C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002B5 RID: 693
			public int <>1__state;

			// Token: 0x040002B6 RID: 694
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002B7 RID: 695
			public DashboardXamlPage <>4__this;
		}

		// Token: 0x020000C5 RID: 197
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnMovePageRight_Clicked>d__55 : IAsyncStateMachine
		{
			// Token: 0x060003E2 RID: 994 RVA: 0x0002D42C File Offset: 0x0002B62C
			void IAsyncStateMachine.MoveNext()
			{
				DashboardXamlPage dashboardXamlPage = this;
				try
				{
					if (dashboardXamlPage.CurrentPage == dashboardXamlPage.Pages.Count - 1)
					{
						if (dashboardXamlPage.Pages.Count > 1)
						{
							DashboardPage dashboardPage = dashboardXamlPage.Pages[0];
							DashboardPage dashboardPage2 = dashboardXamlPage.Pages[dashboardXamlPage.Pages.Count - 1];
							dashboardXamlPage.Pages[dashboardXamlPage.Pages.Count - 1] = dashboardPage;
							dashboardXamlPage.Pages[0] = dashboardPage2;
							dashboardXamlPage.btnNextPage_Clicked(dashboardXamlPage.btnPrevPage, null);
							DashboardListViewModel.Current.SaveDashboardToSettings();
						}
					}
					else
					{
						DashboardPage dashboardPage3 = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage + 1];
						DashboardPage dashboardPage4 = dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage];
						dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage] = dashboardPage3;
						dashboardXamlPage.Pages[dashboardXamlPage.CurrentPage + 1] = dashboardPage4;
						dashboardXamlPage.btnNextPage_Clicked(dashboardXamlPage.btnNextPage, null);
						DashboardListViewModel.Current.SaveDashboardToSettings();
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

			// Token: 0x060003E3 RID: 995 RVA: 0x0002D570 File Offset: 0x0002B770
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002B8 RID: 696
			public int <>1__state;

			// Token: 0x040002B9 RID: 697
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002BA RID: 698
			public DashboardXamlPage <>4__this;
		}

		// Token: 0x020000C6 RID: 198
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnNextPage_Clicked>d__49 : IAsyncStateMachine
		{
			// Token: 0x060003E4 RID: 996 RVA: 0x0002D580 File Offset: 0x0002B780
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardXamlPage.<btnNextPage_Clicked>d__49>(ref taskAwaiter, ref this);
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
					if (dashboardXamlPage.CurrentPage < dashboardXamlPage.TotalPages - 1)
					{
						int currentPage = dashboardXamlPage.CurrentPage;
						dashboardXamlPage.CurrentPage = currentPage + 1;
					}
					else
					{
						dashboardXamlPage.CurrentPage = 0;
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

			// Token: 0x060003E5 RID: 997 RVA: 0x0002D664 File Offset: 0x0002B864
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002BB RID: 699
			public int <>1__state;

			// Token: 0x040002BC RID: 700
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002BD RID: 701
			public DashboardXamlPage <>4__this;

			// Token: 0x040002BE RID: 702
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000C7 RID: 199
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPrevPage_Clicked>d__48 : IAsyncStateMachine
		{
			// Token: 0x060003E6 RID: 998 RVA: 0x0002D674 File Offset: 0x0002B874
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardXamlPage dashboardXamlPage = this;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardXamlPage.<btnPrevPage_Clicked>d__48>(ref taskAwaiter, ref this);
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
					if (dashboardXamlPage.CurrentPage > 0)
					{
						int currentPage = dashboardXamlPage.CurrentPage;
						dashboardXamlPage.CurrentPage = currentPage - 1;
					}
					else
					{
						dashboardXamlPage.CurrentPage = dashboardXamlPage.TotalPages - 1;
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

			// Token: 0x060003E7 RID: 999 RVA: 0x0002D758 File Offset: 0x0002B958
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002BF RID: 703
			public int <>1__state;

			// Token: 0x040002C0 RID: 704
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002C1 RID: 705
			public DashboardXamlPage <>4__this;

			// Token: 0x040002C2 RID: 706
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000C8 RID: 200
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_44
		{
			// Token: 0x060003E8 RID: 1000 RVA: 0x0002D768 File Offset: 0x0002B968
			public <InitializeComponent>_anonXamlCDataTemplate_44()
			{
			}

			// Token: 0x060003E9 RID: 1001 RVA: 0x0002D77C File Offset: 0x0002B97C
			internal object LoadDataTemplate()
			{
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 21);
				TapGestureRecognizer tapGestureRecognizer;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 26);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 80);
				CachedImage cachedImage;
				VisualDiagnostics.RegisterSourceInfo(cachedImage = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 26);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 32);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 75);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 22);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 18);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 21);
				TapGestureRecognizer tapGestureRecognizer2;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 26);
				StaticResourceExtension staticResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 80);
				CachedImage cachedImage2;
				VisualDiagnostics.RegisterSourceInfo(cachedImage2 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 26);
				StaticResourceExtension staticResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 32);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 75);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 26);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 22);
				Frame frame2;
				VisualDiagnostics.RegisterSourceInfo(frame2 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 18);
				StaticResourceExtension staticResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 21);
				TapGestureRecognizer tapGestureRecognizer3;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer3 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 26);
				StaticResourceExtension staticResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 82);
				CachedImage cachedImage3;
				VisualDiagnostics.RegisterSourceInfo(cachedImage3 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 26);
				StaticResourceExtension staticResourceExtension9;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 32);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 75);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 26);
				Grid grid3;
				VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
				Frame frame3;
				VisualDiagnostics.RegisterSourceInfo(frame3 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 18);
				StaticResourceExtension staticResourceExtension10;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 21);
				TapGestureRecognizer tapGestureRecognizer4;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer4 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 26);
				StaticResourceExtension staticResourceExtension11;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 80);
				CachedImage cachedImage4;
				VisualDiagnostics.RegisterSourceInfo(cachedImage4 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 26);
				StaticResourceExtension staticResourceExtension12;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 32);
				Translate translate4;
				VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 75);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 26);
				Grid grid4;
				VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 22);
				Frame frame4;
				VisualDiagnostics.RegisterSourceInfo(frame4 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 18);
				StaticResourceExtension staticResourceExtension13;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 21);
				TapGestureRecognizer tapGestureRecognizer5;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer5 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 26);
				StaticResourceExtension staticResourceExtension14;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 86);
				CachedImage cachedImage5;
				VisualDiagnostics.RegisterSourceInfo(cachedImage5 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 26);
				StaticResourceExtension staticResourceExtension15;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 32);
				Translate translate5;
				VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 75);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 26);
				Grid grid5;
				VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 22);
				Frame frame5;
				VisualDiagnostics.RegisterSourceInfo(frame5 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 18);
				StaticResourceExtension staticResourceExtension16;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 21);
				TapGestureRecognizer tapGestureRecognizer6;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer6 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 26);
				StaticResourceExtension staticResourceExtension17;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension17 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 87);
				CachedImage cachedImage6;
				VisualDiagnostics.RegisterSourceInfo(cachedImage6 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 26);
				StaticResourceExtension staticResourceExtension18;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension18 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 32);
				Translate translate6;
				VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 75);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 26);
				Grid grid6;
				VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 22);
				Frame frame6;
				VisualDiagnostics.RegisterSourceInfo(frame6 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 18);
				UniformGrid uniformGrid;
				VisualDiagnostics.RegisterSourceInfo(uniformGrid = new UniformGrid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 14);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(uniformGrid, nameScope);
				uniformGrid.SetValue(View.MarginProperty, new Thickness(5.0, 5.0, 5.0, 5.0));
				uniformGrid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				uniformGrid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				frame.SetValue(Grid.RowProperty, 0);
				frame.SetValue(Grid.ColumnProperty, 0);
				staticResourceExtension.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = frame;
				array2[1] = uniformGrid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 21)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				frame.Style = obj2;
				tapGestureRecognizer.Tapped += this.root.btnAddPage_Clicked;
				frame.GestureRecognizers.Add(tapGestureRecognizer);
				grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("dash_add_page.png"));
				staticResourceExtension2.Key = "pageSettingsImage";
				IMarkupExtension markupExtension2 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array3, 4, num2);
				object[] array4 = array3;
				array4[0] = cachedImage;
				array4[1] = grid;
				array4[2] = frame;
				array4[3] = uniformGrid;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver2.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver2.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 80)));
				object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
				cachedImage.Style = obj4;
				grid.Children.Add(cachedImage);
				staticResourceExtension3.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension3 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = label;
				array6[1] = grid;
				array6[2] = frame;
				array6[3] = uniformGrid;
				object obj5;
				xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array6, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver3.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver3.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver3.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver3.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 32)));
				object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label.Style = obj6;
				translate.Text = "ios_AddPage";
				IMarkupExtension markupExtension4 = translate;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = label;
				array8[1] = grid;
				array8[2] = frame;
				array8[3] = uniformGrid;
				object obj7;
				xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver4.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver4.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver4.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver4.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 75)));
				object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label.Text = obj8;
				grid.Children.Add(label);
				frame.SetValue(ContentView.ContentProperty, grid);
				uniformGrid.Children.Add(frame);
				frame2.SetValue(Grid.RowProperty, 0);
				frame2.SetValue(Grid.ColumnProperty, 1);
				staticResourceExtension4.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension5 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array9, 2, num5);
				object[] array10 = array9;
				array10[0] = frame2;
				array10[1] = uniformGrid;
				object obj9;
				xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array10, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver5.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver5.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver5.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver5.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 21)));
				object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
				frame2.Style = obj10;
				tapGestureRecognizer2.Tapped += this.root.btnAddItem_Clicked;
				frame2.GestureRecognizers.Add(tapGestureRecognizer2);
				grid2.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage2.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("dash_add_item.png"));
				staticResourceExtension5.Key = "pageSettingsImage";
				IMarkupExtension markupExtension6 = staticResourceExtension5;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array11, 4, num6);
				object[] array12 = array11;
				array12[0] = cachedImage2;
				array12[1] = grid2;
				array12[2] = frame2;
				array12[3] = uniformGrid;
				object obj11;
				xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array12, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver6.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver6.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver6.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver6.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 80)));
				object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
				cachedImage2.Style = obj12;
				grid2.Children.Add(cachedImage2);
				staticResourceExtension6.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension7 = staticResourceExtension6;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array13, 4, num7);
				object[] array14 = array13;
				array14[0] = label2;
				array14[1] = grid2;
				array14[2] = frame2;
				array14[3] = uniformGrid;
				object obj13;
				xamlServiceProvider7.Add(typeFromHandle13, obj13 = new SimpleValueTargetProvider(array14, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver7.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver7.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver7.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver7.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 32)));
				object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label2.Style = obj14;
				translate2.Text = "dash_AddItem";
				IMarkupExtension markupExtension8 = translate2;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array15, 4, num8);
				object[] array16 = array15;
				array16[0] = label2;
				array16[1] = grid2;
				array16[2] = frame2;
				array16[3] = uniformGrid;
				object obj15;
				xamlServiceProvider8.Add(typeFromHandle15, obj15 = new SimpleValueTargetProvider(array16, Label.TextProperty, nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver8.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver8.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver8.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver8.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 75)));
				object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
				label2.Text = obj16;
				grid2.Children.Add(label2);
				frame2.SetValue(ContentView.ContentProperty, grid2);
				uniformGrid.Children.Add(frame2);
				frame3.SetValue(Grid.RowProperty, 1);
				frame3.SetValue(Grid.ColumnProperty, 0);
				staticResourceExtension7.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension9 = staticResourceExtension7;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array17, 2, num9);
				object[] array18 = array17;
				array18[0] = frame3;
				array18[1] = uniformGrid;
				object obj17;
				xamlServiceProvider9.Add(typeFromHandle17, obj17 = new SimpleValueTargetProvider(array18, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver9.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver9.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver9.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver9.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver9.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 21)));
				object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
				frame3.Style = obj18;
				tapGestureRecognizer3.Tapped += this.root.btnEditPage_Clicked;
				frame3.GestureRecognizers.Add(tapGestureRecognizer3);
				grid3.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage3.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_settings.png"));
				staticResourceExtension8.Key = "pageSettingsImage";
				IMarkupExtension markupExtension10 = staticResourceExtension8;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array19, 4, num10);
				object[] array20 = array19;
				array20[0] = cachedImage3;
				array20[1] = grid3;
				array20[2] = frame3;
				array20[3] = uniformGrid;
				object obj19;
				xamlServiceProvider10.Add(typeFromHandle19, obj19 = new SimpleValueTargetProvider(array20, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj19);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver10.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver10.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver10.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver10.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver10.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 82)));
				object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
				cachedImage3.Style = obj20;
				grid3.Children.Add(cachedImage3);
				staticResourceExtension9.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension11 = staticResourceExtension9;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array21, 4, num11);
				object[] array22 = array21;
				array22[0] = label3;
				array22[1] = grid3;
				array22[2] = frame3;
				array22[3] = uniformGrid;
				object obj21;
				xamlServiceProvider11.Add(typeFromHandle21, obj21 = new SimpleValueTargetProvider(array22, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj21);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver11.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver11.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver11.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver11.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver11.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 32)));
				object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
				label3.Style = obj22;
				translate3.Text = "ios_EditPage";
				IMarkupExtension markupExtension12 = translate3;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array23, 4, num12);
				object[] array24 = array23;
				array24[0] = label3;
				array24[1] = grid3;
				array24[2] = frame3;
				array24[3] = uniformGrid;
				object obj23;
				xamlServiceProvider12.Add(typeFromHandle23, obj23 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj23);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver12.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver12.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver12.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver12.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver12.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver12.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 75)));
				object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
				label3.Text = obj24;
				grid3.Children.Add(label3);
				frame3.SetValue(ContentView.ContentProperty, grid3);
				uniformGrid.Children.Add(frame3);
				frame4.SetValue(Grid.RowProperty, 1);
				frame4.SetValue(Grid.ColumnProperty, 1);
				staticResourceExtension10.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension13 = staticResourceExtension10;
				XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
				Type typeFromHandle25 = typeof(IProvideValueTarget);
				int num13;
				object[] array25 = new object[(num13 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array25, 2, num13);
				object[] array26 = array25;
				array26[0] = frame4;
				array26[1] = uniformGrid;
				object obj25;
				xamlServiceProvider13.Add(typeFromHandle25, obj25 = new SimpleValueTargetProvider(array26, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider13.Add(typeof(IReferenceProvider), obj25);
				Type typeFromHandle26 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
				xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver13.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver13.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver13.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver13.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver13.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver13.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver13.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 21)));
				object obj26 = markupExtension13.ProvideValue(xamlServiceProvider13);
				frame4.Style = obj26;
				tapGestureRecognizer4.Tapped += this.root.btnDelPage_Clicked;
				frame4.GestureRecognizers.Add(tapGestureRecognizer4);
				grid4.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage4.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("dash_del_page.png"));
				staticResourceExtension11.Key = "pageSettingsImage";
				IMarkupExtension markupExtension14 = staticResourceExtension11;
				XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
				Type typeFromHandle27 = typeof(IProvideValueTarget);
				int num14;
				object[] array27 = new object[(num14 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array27, 4, num14);
				object[] array28 = array27;
				array28[0] = cachedImage4;
				array28[1] = grid4;
				array28[2] = frame4;
				array28[3] = uniformGrid;
				object obj27;
				xamlServiceProvider14.Add(typeFromHandle27, obj27 = new SimpleValueTargetProvider(array28, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider14.Add(typeof(IReferenceProvider), obj27);
				Type typeFromHandle28 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
				xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver14.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver14.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver14.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver14.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver14.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver14.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver14.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 80)));
				object obj28 = markupExtension14.ProvideValue(xamlServiceProvider14);
				cachedImage4.Style = obj28;
				grid4.Children.Add(cachedImage4);
				staticResourceExtension12.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension15 = staticResourceExtension12;
				XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
				Type typeFromHandle29 = typeof(IProvideValueTarget);
				int num15;
				object[] array29 = new object[(num15 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array29, 4, num15);
				object[] array30 = array29;
				array30[0] = label4;
				array30[1] = grid4;
				array30[2] = frame4;
				array30[3] = uniformGrid;
				object obj29;
				xamlServiceProvider15.Add(typeFromHandle29, obj29 = new SimpleValueTargetProvider(array30, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider15.Add(typeof(IReferenceProvider), obj29);
				Type typeFromHandle30 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
				xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver15.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver15.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver15.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver15.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver15.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver15.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver15.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 32)));
				object obj30 = markupExtension15.ProvideValue(xamlServiceProvider15);
				label4.Style = obj30;
				translate4.Text = "ios_DelPage";
				IMarkupExtension markupExtension16 = translate4;
				XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
				Type typeFromHandle31 = typeof(IProvideValueTarget);
				int num16;
				object[] array31 = new object[(num16 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array31, 4, num16);
				object[] array32 = array31;
				array32[0] = label4;
				array32[1] = grid4;
				array32[2] = frame4;
				array32[3] = uniformGrid;
				object obj31;
				xamlServiceProvider16.Add(typeFromHandle31, obj31 = new SimpleValueTargetProvider(array32, Label.TextProperty, nameScope));
				xamlServiceProvider16.Add(typeof(IReferenceProvider), obj31);
				Type typeFromHandle32 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
				xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver16.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver16.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver16.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver16.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver16.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver16.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver16.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 75)));
				object obj32 = markupExtension16.ProvideValue(xamlServiceProvider16);
				label4.Text = obj32;
				grid4.Children.Add(label4);
				frame4.SetValue(ContentView.ContentProperty, grid4);
				uniformGrid.Children.Add(frame4);
				frame5.SetValue(Grid.RowProperty, 2);
				frame5.SetValue(Grid.ColumnProperty, 0);
				staticResourceExtension13.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension17 = staticResourceExtension13;
				XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
				Type typeFromHandle33 = typeof(IProvideValueTarget);
				int num17;
				object[] array33 = new object[(num17 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array33, 2, num17);
				object[] array34 = array33;
				array34[0] = frame5;
				array34[1] = uniformGrid;
				object obj33;
				xamlServiceProvider17.Add(typeFromHandle33, obj33 = new SimpleValueTargetProvider(array34, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider17.Add(typeof(IReferenceProvider), obj33);
				Type typeFromHandle34 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
				xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver17.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver17.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver17.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver17.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver17.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver17.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver17.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 21)));
				object obj34 = markupExtension17.ProvideValue(xamlServiceProvider17);
				frame5.Style = obj34;
				tapGestureRecognizer5.Tapped += this.root.btnMovePageLeft_Clicked;
				frame5.GestureRecognizers.Add(tapGestureRecognizer5);
				grid5.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage5.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_chevron_left.png"));
				staticResourceExtension14.Key = "pageSettingsImage";
				IMarkupExtension markupExtension18 = staticResourceExtension14;
				XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
				Type typeFromHandle35 = typeof(IProvideValueTarget);
				int num18;
				object[] array35 = new object[(num18 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array35, 4, num18);
				object[] array36 = array35;
				array36[0] = cachedImage5;
				array36[1] = grid5;
				array36[2] = frame5;
				array36[3] = uniformGrid;
				object obj35;
				xamlServiceProvider18.Add(typeFromHandle35, obj35 = new SimpleValueTargetProvider(array36, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider18.Add(typeof(IReferenceProvider), obj35);
				Type typeFromHandle36 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
				xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver18.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver18.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver18.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver18.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver18.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver18.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver18.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 86)));
				object obj36 = markupExtension18.ProvideValue(xamlServiceProvider18);
				cachedImage5.Style = obj36;
				grid5.Children.Add(cachedImage5);
				staticResourceExtension15.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension19 = staticResourceExtension15;
				XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
				Type typeFromHandle37 = typeof(IProvideValueTarget);
				int num19;
				object[] array37 = new object[(num19 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array37, 4, num19);
				object[] array38 = array37;
				array38[0] = label5;
				array38[1] = grid5;
				array38[2] = frame5;
				array38[3] = uniformGrid;
				object obj37;
				xamlServiceProvider19.Add(typeFromHandle37, obj37 = new SimpleValueTargetProvider(array38, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider19.Add(typeof(IReferenceProvider), obj37);
				Type typeFromHandle38 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
				xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver19.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver19.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver19.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver19.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver19.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver19.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver19.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 32)));
				object obj38 = markupExtension19.ProvideValue(xamlServiceProvider19);
				label5.Style = obj38;
				translate5.Text = "ios_MovePageLeft";
				IMarkupExtension markupExtension20 = translate5;
				XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
				Type typeFromHandle39 = typeof(IProvideValueTarget);
				int num20;
				object[] array39 = new object[(num20 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array39, 4, num20);
				object[] array40 = array39;
				array40[0] = label5;
				array40[1] = grid5;
				array40[2] = frame5;
				array40[3] = uniformGrid;
				object obj39;
				xamlServiceProvider20.Add(typeFromHandle39, obj39 = new SimpleValueTargetProvider(array40, Label.TextProperty, nameScope));
				xamlServiceProvider20.Add(typeof(IReferenceProvider), obj39);
				Type typeFromHandle40 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
				xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver20.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver20.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver20.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver20.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver20.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver20.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver20.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 75)));
				object obj40 = markupExtension20.ProvideValue(xamlServiceProvider20);
				label5.Text = obj40;
				grid5.Children.Add(label5);
				frame5.SetValue(ContentView.ContentProperty, grid5);
				uniformGrid.Children.Add(frame5);
				frame6.SetValue(Grid.RowProperty, 2);
				frame6.SetValue(Grid.ColumnProperty, 1);
				staticResourceExtension16.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension21 = staticResourceExtension16;
				XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
				Type typeFromHandle41 = typeof(IProvideValueTarget);
				int num21;
				object[] array41 = new object[(num21 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array41, 2, num21);
				object[] array42 = array41;
				array42[0] = frame6;
				array42[1] = uniformGrid;
				object obj41;
				xamlServiceProvider21.Add(typeFromHandle41, obj41 = new SimpleValueTargetProvider(array42, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider21.Add(typeof(IReferenceProvider), obj41);
				Type typeFromHandle42 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
				xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver21.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver21.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver21.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver21.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver21.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver21.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver21.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 21)));
				object obj42 = markupExtension21.ProvideValue(xamlServiceProvider21);
				frame6.Style = obj42;
				tapGestureRecognizer6.Tapped += this.root.btnMovePageRight_Clicked;
				frame6.GestureRecognizers.Add(tapGestureRecognizer6);
				grid6.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage6.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_chevron_right.png"));
				staticResourceExtension17.Key = "pageSettingsImage";
				IMarkupExtension markupExtension22 = staticResourceExtension17;
				XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
				Type typeFromHandle43 = typeof(IProvideValueTarget);
				int num22;
				object[] array43 = new object[(num22 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array43, 4, num22);
				object[] array44 = array43;
				array44[0] = cachedImage6;
				array44[1] = grid6;
				array44[2] = frame6;
				array44[3] = uniformGrid;
				object obj43;
				xamlServiceProvider22.Add(typeFromHandle43, obj43 = new SimpleValueTargetProvider(array44, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider22.Add(typeof(IReferenceProvider), obj43);
				Type typeFromHandle44 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
				xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver22.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver22.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver22.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver22.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver22.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver22.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver22.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 87)));
				object obj44 = markupExtension22.ProvideValue(xamlServiceProvider22);
				cachedImage6.Style = obj44;
				grid6.Children.Add(cachedImage6);
				staticResourceExtension18.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension23 = staticResourceExtension18;
				XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
				Type typeFromHandle45 = typeof(IProvideValueTarget);
				int num23;
				object[] array45 = new object[(num23 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array45, 4, num23);
				object[] array46 = array45;
				array46[0] = label6;
				array46[1] = grid6;
				array46[2] = frame6;
				array46[3] = uniformGrid;
				object obj45;
				xamlServiceProvider23.Add(typeFromHandle45, obj45 = new SimpleValueTargetProvider(array46, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider23.Add(typeof(IReferenceProvider), obj45);
				Type typeFromHandle46 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
				xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver23.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver23.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver23.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver23.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver23.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver23.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver23.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 32)));
				object obj46 = markupExtension23.ProvideValue(xamlServiceProvider23);
				label6.Style = obj46;
				translate6.Text = "ios_MovePageRight";
				IMarkupExtension markupExtension24 = translate6;
				XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
				Type typeFromHandle47 = typeof(IProvideValueTarget);
				int num24;
				object[] array47 = new object[(num24 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array47, 4, num24);
				object[] array48 = array47;
				array48[0] = label6;
				array48[1] = grid6;
				array48[2] = frame6;
				array48[3] = uniformGrid;
				object obj47;
				xamlServiceProvider24.Add(typeFromHandle47, obj47 = new SimpleValueTargetProvider(array48, Label.TextProperty, nameScope));
				xamlServiceProvider24.Add(typeof(IReferenceProvider), obj47);
				Type typeFromHandle48 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
				xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver24.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver24.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver24.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver24.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver24.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver24.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver24.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_44).GetTypeInfo().Assembly));
				xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 75)));
				object obj48 = markupExtension24.ProvideValue(xamlServiceProvider24);
				label6.Text = obj48;
				grid6.Children.Add(label6);
				frame6.SetValue(ContentView.ContentProperty, grid6);
				uniformGrid.Children.Add(frame6);
				return uniformGrid;
			}

			// Token: 0x040002C3 RID: 707
			internal object[] parentValues;

			// Token: 0x040002C4 RID: 708
			internal DashboardXamlPage root;
		}

		// Token: 0x020000C9 RID: 201
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_45
		{
			// Token: 0x060003EA RID: 1002 RVA: 0x00030D94 File Offset: 0x0002EF94
			public <InitializeComponent>_anonXamlCDataTemplate_45()
			{
			}

			// Token: 0x060003EB RID: 1003 RVA: 0x00030DA8 File Offset: 0x0002EFA8
			internal object LoadDataTemplate()
			{
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 24);
				TapGestureRecognizer tapGestureRecognizer;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 26);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 80);
				CachedImage cachedImage;
				VisualDiagnostics.RegisterSourceInfo(cachedImage = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 26);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 32);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 75);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 22);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 18);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 24);
				TapGestureRecognizer tapGestureRecognizer2;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 26);
				StaticResourceExtension staticResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 86);
				CachedImage cachedImage2;
				VisualDiagnostics.RegisterSourceInfo(cachedImage2 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 26);
				StaticResourceExtension staticResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 32);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 75);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 26);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 22);
				Frame frame2;
				VisualDiagnostics.RegisterSourceInfo(frame2 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 18);
				StaticResourceExtension staticResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 24);
				TapGestureRecognizer tapGestureRecognizer3;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer3 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 26);
				StaticResourceExtension staticResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 87);
				CachedImage cachedImage3;
				VisualDiagnostics.RegisterSourceInfo(cachedImage3 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 26);
				StaticResourceExtension staticResourceExtension9;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 32);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 75);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 26);
				Grid grid3;
				VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 22);
				Frame frame3;
				VisualDiagnostics.RegisterSourceInfo(frame3 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 18);
				StaticResourceExtension staticResourceExtension10;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 24);
				TapGestureRecognizer tapGestureRecognizer4;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer4 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 26);
				StaticResourceExtension staticResourceExtension11;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 82);
				CachedImage cachedImage4;
				VisualDiagnostics.RegisterSourceInfo(cachedImage4 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 26);
				StaticResourceExtension staticResourceExtension12;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 32);
				Translate translate4;
				VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 75);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 26);
				Grid grid4;
				VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 22);
				Frame frame4;
				VisualDiagnostics.RegisterSourceInfo(frame4 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 18);
				StaticResourceExtension staticResourceExtension13;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 24);
				TapGestureRecognizer tapGestureRecognizer5;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer5 = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 26);
				StaticResourceExtension staticResourceExtension14;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 80);
				CachedImage cachedImage5;
				VisualDiagnostics.RegisterSourceInfo(cachedImage5 = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 26);
				StaticResourceExtension staticResourceExtension15;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 32);
				Translate translate5;
				VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 75);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 26);
				Grid grid5;
				VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 22);
				Frame frame5;
				VisualDiagnostics.RegisterSourceInfo(frame5 = new Frame(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 18);
				UniformGrid uniformGrid;
				VisualDiagnostics.RegisterSourceInfo(uniformGrid = new UniformGrid(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 14);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(uniformGrid, nameScope);
				uniformGrid.SetValue(View.MarginProperty, new Thickness(5.0, 5.0, 5.0, 5.0));
				uniformGrid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				uniformGrid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				staticResourceExtension.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = frame;
				array2[1] = uniformGrid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 24)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				frame.Style = obj2;
				tapGestureRecognizer.Tapped += this.root.btnAddPage_Clicked;
				frame.GestureRecognizers.Add(tapGestureRecognizer);
				grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("dash_add_page.png"));
				staticResourceExtension2.Key = "pageSettingsImage";
				IMarkupExtension markupExtension2 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array3, 4, num2);
				object[] array4 = array3;
				array4[0] = cachedImage;
				array4[1] = grid;
				array4[2] = frame;
				array4[3] = uniformGrid;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver2.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver2.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(169, 80)));
				object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
				cachedImage.Style = obj4;
				grid.Children.Add(cachedImage);
				staticResourceExtension3.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension3 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = label;
				array6[1] = grid;
				array6[2] = frame;
				array6[3] = uniformGrid;
				object obj5;
				xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array6, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver3.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver3.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver3.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver3.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 32)));
				object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label.Style = obj6;
				translate.Text = "ios_AddPage";
				IMarkupExtension markupExtension4 = translate;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = label;
				array8[1] = grid;
				array8[2] = frame;
				array8[3] = uniformGrid;
				object obj7;
				xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver4.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver4.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver4.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver4.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 75)));
				object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label.Text = obj8;
				grid.Children.Add(label);
				frame.SetValue(ContentView.ContentProperty, grid);
				uniformGrid.Children.Add(frame);
				staticResourceExtension4.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension5 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array9, 2, num5);
				object[] array10 = array9;
				array10[0] = frame2;
				array10[1] = uniformGrid;
				object obj9;
				xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array10, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver5.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver5.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver5.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver5.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(181, 24)));
				object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
				frame2.Style = obj10;
				tapGestureRecognizer2.Tapped += this.root.btnMovePageLeft_Clicked;
				frame2.GestureRecognizers.Add(tapGestureRecognizer2);
				grid2.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage2.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_chevron_left.png"));
				staticResourceExtension5.Key = "pageSettingsImage";
				IMarkupExtension markupExtension6 = staticResourceExtension5;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array11, 4, num6);
				object[] array12 = array11;
				array12[0] = cachedImage2;
				array12[1] = grid2;
				array12[2] = frame2;
				array12[3] = uniformGrid;
				object obj11;
				xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array12, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver6.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver6.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver6.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver6.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 86)));
				object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
				cachedImage2.Style = obj12;
				grid2.Children.Add(cachedImage2);
				staticResourceExtension6.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension7 = staticResourceExtension6;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array13, 4, num7);
				object[] array14 = array13;
				array14[0] = label2;
				array14[1] = grid2;
				array14[2] = frame2;
				array14[3] = uniformGrid;
				object obj13;
				xamlServiceProvider7.Add(typeFromHandle13, obj13 = new SimpleValueTargetProvider(array14, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver7.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver7.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver7.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver7.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 32)));
				object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label2.Style = obj14;
				translate2.Text = "ios_MovePageLeft";
				IMarkupExtension markupExtension8 = translate2;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array15, 4, num8);
				object[] array16 = array15;
				array16[0] = label2;
				array16[1] = grid2;
				array16[2] = frame2;
				array16[3] = uniformGrid;
				object obj15;
				xamlServiceProvider8.Add(typeFromHandle15, obj15 = new SimpleValueTargetProvider(array16, Label.TextProperty, nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver8.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver8.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver8.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver8.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 75)));
				object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
				label2.Text = obj16;
				grid2.Children.Add(label2);
				frame2.SetValue(ContentView.ContentProperty, grid2);
				uniformGrid.Children.Add(frame2);
				staticResourceExtension7.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension9 = staticResourceExtension7;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array17, 2, num9);
				object[] array18 = array17;
				array18[0] = frame3;
				array18[1] = uniformGrid;
				object obj17;
				xamlServiceProvider9.Add(typeFromHandle17, obj17 = new SimpleValueTargetProvider(array18, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver9.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver9.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver9.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver9.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver9.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(193, 24)));
				object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
				frame3.Style = obj18;
				tapGestureRecognizer3.Tapped += this.root.btnMovePageRight_Clicked;
				frame3.GestureRecognizers.Add(tapGestureRecognizer3);
				grid3.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage3.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_chevron_right.png"));
				staticResourceExtension8.Key = "pageSettingsImage";
				IMarkupExtension markupExtension10 = staticResourceExtension8;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array19, 4, num10);
				object[] array20 = array19;
				array20[0] = cachedImage3;
				array20[1] = grid3;
				array20[2] = frame3;
				array20[3] = uniformGrid;
				object obj19;
				xamlServiceProvider10.Add(typeFromHandle19, obj19 = new SimpleValueTargetProvider(array20, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj19);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver10.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver10.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver10.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver10.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver10.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(195, 87)));
				object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
				cachedImage3.Style = obj20;
				grid3.Children.Add(cachedImage3);
				staticResourceExtension9.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension11 = staticResourceExtension9;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array21, 4, num11);
				object[] array22 = array21;
				array22[0] = label3;
				array22[1] = grid3;
				array22[2] = frame3;
				array22[3] = uniformGrid;
				object obj21;
				xamlServiceProvider11.Add(typeFromHandle21, obj21 = new SimpleValueTargetProvider(array22, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj21);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver11.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver11.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver11.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver11.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver11.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(196, 32)));
				object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
				label3.Style = obj22;
				translate3.Text = "ios_MovePageRight";
				IMarkupExtension markupExtension12 = translate3;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array23, 4, num12);
				object[] array24 = array23;
				array24[0] = label3;
				array24[1] = grid3;
				array24[2] = frame3;
				array24[3] = uniformGrid;
				object obj23;
				xamlServiceProvider12.Add(typeFromHandle23, obj23 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj23);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver12.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver12.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver12.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver12.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver12.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver12.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(196, 75)));
				object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
				label3.Text = obj24;
				grid3.Children.Add(label3);
				frame3.SetValue(ContentView.ContentProperty, grid3);
				uniformGrid.Children.Add(frame3);
				staticResourceExtension10.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension13 = staticResourceExtension10;
				XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
				Type typeFromHandle25 = typeof(IProvideValueTarget);
				int num13;
				object[] array25 = new object[(num13 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array25, 2, num13);
				object[] array26 = array25;
				array26[0] = frame4;
				array26[1] = uniformGrid;
				object obj25;
				xamlServiceProvider13.Add(typeFromHandle25, obj25 = new SimpleValueTargetProvider(array26, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider13.Add(typeof(IReferenceProvider), obj25);
				Type typeFromHandle26 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
				xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver13.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver13.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver13.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver13.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver13.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver13.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver13.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 24)));
				object obj26 = markupExtension13.ProvideValue(xamlServiceProvider13);
				frame4.Style = obj26;
				tapGestureRecognizer4.Tapped += this.root.btnEditPage_Clicked;
				frame4.GestureRecognizers.Add(tapGestureRecognizer4);
				grid4.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage4.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_settings.png"));
				staticResourceExtension11.Key = "pageSettingsImage";
				IMarkupExtension markupExtension14 = staticResourceExtension11;
				XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
				Type typeFromHandle27 = typeof(IProvideValueTarget);
				int num14;
				object[] array27 = new object[(num14 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array27, 4, num14);
				object[] array28 = array27;
				array28[0] = cachedImage4;
				array28[1] = grid4;
				array28[2] = frame4;
				array28[3] = uniformGrid;
				object obj27;
				xamlServiceProvider14.Add(typeFromHandle27, obj27 = new SimpleValueTargetProvider(array28, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider14.Add(typeof(IReferenceProvider), obj27);
				Type typeFromHandle28 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
				xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver14.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver14.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver14.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver14.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver14.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver14.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver14.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 82)));
				object obj28 = markupExtension14.ProvideValue(xamlServiceProvider14);
				cachedImage4.Style = obj28;
				grid4.Children.Add(cachedImage4);
				staticResourceExtension12.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension15 = staticResourceExtension12;
				XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
				Type typeFromHandle29 = typeof(IProvideValueTarget);
				int num15;
				object[] array29 = new object[(num15 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array29, 4, num15);
				object[] array30 = array29;
				array30[0] = label4;
				array30[1] = grid4;
				array30[2] = frame4;
				array30[3] = uniformGrid;
				object obj29;
				xamlServiceProvider15.Add(typeFromHandle29, obj29 = new SimpleValueTargetProvider(array30, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider15.Add(typeof(IReferenceProvider), obj29);
				Type typeFromHandle30 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
				xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver15.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver15.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver15.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver15.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver15.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver15.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver15.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(206, 32)));
				object obj30 = markupExtension15.ProvideValue(xamlServiceProvider15);
				label4.Style = obj30;
				translate4.Text = "ios_EditPage";
				IMarkupExtension markupExtension16 = translate4;
				XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
				Type typeFromHandle31 = typeof(IProvideValueTarget);
				int num16;
				object[] array31 = new object[(num16 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array31, 4, num16);
				object[] array32 = array31;
				array32[0] = label4;
				array32[1] = grid4;
				array32[2] = frame4;
				array32[3] = uniformGrid;
				object obj31;
				xamlServiceProvider16.Add(typeFromHandle31, obj31 = new SimpleValueTargetProvider(array32, Label.TextProperty, nameScope));
				xamlServiceProvider16.Add(typeof(IReferenceProvider), obj31);
				Type typeFromHandle32 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
				xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver16.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver16.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver16.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver16.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver16.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver16.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver16.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(206, 75)));
				object obj32 = markupExtension16.ProvideValue(xamlServiceProvider16);
				label4.Text = obj32;
				grid4.Children.Add(label4);
				frame4.SetValue(ContentView.ContentProperty, grid4);
				uniformGrid.Children.Add(frame4);
				staticResourceExtension13.Key = "pageSettingsFrameStyle";
				IMarkupExtension markupExtension17 = staticResourceExtension13;
				XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
				Type typeFromHandle33 = typeof(IProvideValueTarget);
				int num17;
				object[] array33 = new object[(num17 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array33, 2, num17);
				object[] array34 = array33;
				array34[0] = frame5;
				array34[1] = uniformGrid;
				object obj33;
				xamlServiceProvider17.Add(typeFromHandle33, obj33 = new SimpleValueTargetProvider(array34, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider17.Add(typeof(IReferenceProvider), obj33);
				Type typeFromHandle34 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
				xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver17.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver17.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver17.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver17.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver17.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver17.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver17.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(215, 24)));
				object obj34 = markupExtension17.ProvideValue(xamlServiceProvider17);
				frame5.Style = obj34;
				tapGestureRecognizer5.Tapped += this.root.btnDelPage_Clicked;
				frame5.GestureRecognizers.Add(tapGestureRecognizer5);
				grid5.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("50, Auto"));
				cachedImage5.SetValue(CachedImage.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("dash_del_page.png"));
				staticResourceExtension14.Key = "pageSettingsImage";
				IMarkupExtension markupExtension18 = staticResourceExtension14;
				XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
				Type typeFromHandle35 = typeof(IProvideValueTarget);
				int num18;
				object[] array35 = new object[(num18 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array35, 4, num18);
				object[] array36 = array35;
				array36[0] = cachedImage5;
				array36[1] = grid5;
				array36[2] = frame5;
				array36[3] = uniformGrid;
				object obj35;
				xamlServiceProvider18.Add(typeFromHandle35, obj35 = new SimpleValueTargetProvider(array36, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider18.Add(typeof(IReferenceProvider), obj35);
				Type typeFromHandle36 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
				xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver18.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver18.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver18.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver18.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver18.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver18.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver18.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(217, 80)));
				object obj36 = markupExtension18.ProvideValue(xamlServiceProvider18);
				cachedImage5.Style = obj36;
				grid5.Children.Add(cachedImage5);
				staticResourceExtension15.Key = "pageSettingsLabel";
				IMarkupExtension markupExtension19 = staticResourceExtension15;
				XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
				Type typeFromHandle37 = typeof(IProvideValueTarget);
				int num19;
				object[] array37 = new object[(num19 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array37, 4, num19);
				object[] array38 = array37;
				array38[0] = label5;
				array38[1] = grid5;
				array38[2] = frame5;
				array38[3] = uniformGrid;
				object obj37;
				xamlServiceProvider19.Add(typeFromHandle37, obj37 = new SimpleValueTargetProvider(array38, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider19.Add(typeof(IReferenceProvider), obj37);
				Type typeFromHandle38 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
				xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver19.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver19.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver19.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver19.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver19.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver19.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver19.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(218, 32)));
				object obj38 = markupExtension19.ProvideValue(xamlServiceProvider19);
				label5.Style = obj38;
				translate5.Text = "ios_DelPage";
				IMarkupExtension markupExtension20 = translate5;
				XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
				Type typeFromHandle39 = typeof(IProvideValueTarget);
				int num20;
				object[] array39 = new object[(num20 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array39, 4, num20);
				object[] array40 = array39;
				array40[0] = label5;
				array40[1] = grid5;
				array40[2] = frame5;
				array40[3] = uniformGrid;
				object obj39;
				xamlServiceProvider20.Add(typeFromHandle39, obj39 = new SimpleValueTargetProvider(array40, Label.TextProperty, nameScope));
				xamlServiceProvider20.Add(typeof(IReferenceProvider), obj39);
				Type typeFromHandle40 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
				xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver20.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver20.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver20.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver20.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver20.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver20.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver20.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_45).GetTypeInfo().Assembly));
				xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(218, 75)));
				object obj40 = markupExtension20.ProvideValue(xamlServiceProvider20);
				label5.Text = obj40;
				grid5.Children.Add(label5);
				frame5.SetValue(ContentView.ContentProperty, grid5);
				uniformGrid.Children.Add(frame5);
				return uniformGrid;
			}

			// Token: 0x040002C5 RID: 709
			internal object[] parentValues;

			// Token: 0x040002C6 RID: 710
			internal DashboardXamlPage root;
		}

		// Token: 0x020000CA RID: 202
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_46
		{
			// Token: 0x060003EC RID: 1004 RVA: 0x00033AB0 File Offset: 0x00031CB0
			public <InitializeComponent>_anonXamlCDataTemplate_46()
			{
			}

			// Token: 0x060003ED RID: 1005 RVA: 0x00033AC4 File Offset: 0x00031CC4
			internal object LoadDataTemplate()
			{
				ReferenceExtension referenceExtension;
				VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 37);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 317, 37);
				DataTemplate dataTemplate;
				VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 319, 42);
				ListView listView;
				VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 311, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(listView, nameScope);
				nameScope.RegisterName("lvPages", listView);
				if (listView.StyleId == null)
				{
					listView.StyleId = "lvPages";
				}
				listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
				referenceExtension.Name = "thisPage";
				IMarkupExtension markupExtension = referenceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = listView;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, BindableObject.BindingContextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_46).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(314, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				listView.SetValue(BindableObject.BindingContextProperty, obj2);
				listView.ItemTapped += this.root.lvPages_ItemTapped;
				bindingExtension.Path = "Pages";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "CurrentDashboardPage";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				listView.SetBinding(ListView.SelectedItemProperty, bindingBase2);
				IDataTemplate dataTemplate2 = dataTemplate;
				DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_46.<LoadDataTemplate>_anonXamlCDataTemplate_47 <LoadDataTemplate>_anonXamlCDataTemplate_ = new DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_46.<LoadDataTemplate>_anonXamlCDataTemplate_47();
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = dataTemplate;
				array4[1] = listView;
				<LoadDataTemplate>_anonXamlCDataTemplate_.parentValues = array4;
				<LoadDataTemplate>_anonXamlCDataTemplate_.root = this.root;
				dataTemplate2.LoadTemplate = new Func<object>(<LoadDataTemplate>_anonXamlCDataTemplate_.LoadDataTemplate);
				listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
				return listView;
			}

			// Token: 0x040002C7 RID: 711
			internal object[] parentValues;

			// Token: 0x040002C8 RID: 712
			internal DashboardXamlPage root;

			// Token: 0x020000CB RID: 203
			[CompilerGenerated]
			private sealed class <LoadDataTemplate>_anonXamlCDataTemplate_47
			{
				// Token: 0x060003EE RID: 1006 RVA: 0x00033E6C File Offset: 0x0003206C
				public <LoadDataTemplate>_anonXamlCDataTemplate_47()
				{
				}

				// Token: 0x060003EF RID: 1007 RVA: 0x00033E80 File Offset: 0x00032080
				internal object LoadDataTemplate()
				{
					BindingExtension bindingExtension;
					VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 55);
					TextCell textCell;
					VisualDiagnostics.RegisterSourceInfo(textCell = new TextCell(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 46);
					NameScope nameScope = new NameScope();
					NameScope.SetNameScope(textCell, nameScope);
					bindingExtension.Mode = 2;
					bindingExtension.Path = "Title";
					bindingExtension.TypedBinding = new TypedBinding<DashboardPage, string>(delegate(DashboardPage A_0)
					{
						if (A_0 != null)
						{
							return new ValueTuple<string, bool>(A_0.Title, true);
						}
						return default(ValueTuple<string, bool>);
					}, null, new Tuple<Func<DashboardPage, object>, string>[]
					{
						new Tuple<Func<DashboardPage, object>, string>((DashboardPage A_0) => A_0, "Title")
					});
					BindingBase bindingBase = bindingExtension.ProvideValue(null);
					textCell.SetBinding(TextCell.TextProperty, bindingBase);
					textCell.SetValue(TextCell.TextColorProperty, Color.Black);
					return textCell;
				}

				// Token: 0x060003F0 RID: 1008 RVA: 0x00033F68 File Offset: 0x00032168
				[CompilerGenerated]
				private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1332(DashboardPage A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Title, true);
					}
					return default(ValueTuple<string, bool>);
				}

				// Token: 0x060003F1 RID: 1009 RVA: 0x00033F98 File Offset: 0x00032198
				[CompilerGenerated]
				private static object <LoadDataTemplate>typedBindingsM__1333(DashboardPage A_0)
				{
					return A_0;
				}

				// Token: 0x040002C9 RID: 713
				internal object[] parentValues;

				// Token: 0x040002CA RID: 714
				internal DashboardXamlPage root;
			}
		}

		// Token: 0x020000CC RID: 204
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_48
		{
			// Token: 0x060003F2 RID: 1010 RVA: 0x00033FA8 File Offset: 0x000321A8
			public <InitializeComponent>_anonXamlCDataTemplate_48()
			{
			}

			// Token: 0x060003F3 RID: 1011 RVA: 0x00033FBC File Offset: 0x000321BC
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 519, 37);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 522, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardXamlPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 516, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(label, nameScope);
				nameScope.RegisterName("labelHint", label);
				if (label.StyleId == null)
				{
					label.StyleId = "labelHint";
				}
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = label;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_48).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(519, 37)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				translate.Text = "ios_DashboardEditor_Hint";
				IMarkupExtension markupExtension2 = translate;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array3, 1, num2);
				object[] array4 = array3;
				array4[0] = label;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("buttons", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver2.Add("dashPages", "clr-namespace:CarScannerXamarinForms.Pages.Dashboard");
				xmlNamespaceResolver2.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
				xmlNamespaceResolver2.Add("sfPopup", "clr-namespace:Syncfusion.XForms.PopupLayout;assembly=Syncfusion.SfPopupLayout.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardXamlPage.<InitializeComponent>_anonXamlCDataTemplate_48).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(522, 37)));
				object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.Text = obj3;
				label.SetValue(Label.TextColorProperty, Color.Black);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				return label;
			}

			// Token: 0x040002CB RID: 715
			internal object[] parentValues;

			// Token: 0x040002CC RID: 716
			internal DashboardXamlPage root;
		}
	}
}
