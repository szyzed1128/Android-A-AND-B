using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
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

namespace CarScannerXamarinForms
{
	// Token: 0x020000D9 RID: 217
	[XamlFilePath("Pages\\LiveDataChartPage.xaml")]
	public class LiveDataChartPage : ContentPage
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x0003A3B8 File Offset: 0x000385B8
		public LiveDataChartPage()
		{
			this.InitializeComponent();
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
			this.CreateModels();
			this.Init();
			this._Model0.PIDChanged += this.Model_PidChanged;
			this._Model1.PIDChanged += this.Model_PidChanged;
			this._Model2.PIDChanged += this.Model_PidChanged;
			this._Model3.PIDChanged += this.Model_PidChanged;
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0003A4A8 File Offset: 0x000386A8
		private void RealignAndroidButtons()
		{
			if (Device.RuntimePlatform == "Android" || Device.RuntimePlatform == "UWP")
			{
				this.gridButtons.ColumnDefinitions.Clear();
				this.gridButtons.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Star
				});
				this.gridButtons.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Star
				});
				this.gridButtons.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Star
				});
				this.gridButtons.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Star
				});
				this.gridButtons.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Star
				});
				this.gridButtons.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Star
				});
				Grid.SetColumn(this.btn1, 0);
				Grid.SetColumn(this.btn2, 1);
				Grid.SetColumn(this.btn3, 2);
				Grid.SetColumn(this.btn4, 3);
				Grid.SetColumn(this.btnPlay, 4);
				Grid.SetColumn(this.btnPause, 4);
				Grid.SetColumn(this.btnInfo, 5);
				this.btnPause.HorizontalOptions = LayoutOptions.Center;
				this.btnPlay.HorizontalOptions = LayoutOptions.Center;
				this.btnInfo.HorizontalOptions = LayoutOptions.Center;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0003A62E File Offset: 0x0003882E
		private void btnPause_Clicked(object sender, EventArgs e)
		{
			this.Pause();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0003A638 File Offset: 0x00038838
		private void Pause()
		{
			this.isPaused = true;
			this.btnPause.IsVisible = false;
			this.btnPlay.IsVisible = true;
			this.Model0.Unsubscribe();
			this.Model1.Unsubscribe();
			this.Model2.Unsubscribe();
			this.Model3.Unsubscribe();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0003A690 File Offset: 0x00038890
		private void Play()
		{
			this.isPaused = false;
			this.btnPause.IsVisible = true;
			this.btnPlay.IsVisible = false;
			this.Model0.Subscribe();
			this.Model1.Subscribe();
			this.Model2.Subscribe();
			this.Model3.Subscribe();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0003A6E8 File Offset: 0x000388E8
		private void btnPlay_Clicked(object sender, EventArgs e)
		{
			this.Play();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0003A6F0 File Offset: 0x000388F0
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_MainPage_TileLiveData"), Translate.GetString("ios_MainPage_TileLiveData_Info"), "OK");
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0003A714 File Offset: 0x00038914
		private void UpdateRequestsDelegate(List<OBDRequest> requests)
		{
			this.Model0.GetRequests(requests, null, "");
			this.Model1.GetRequests(requests, null, "");
			this.Model2.GetRequests(requests, null, "");
			this.Model3.GetRequests(requests, null, "");
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0003A76C File Offset: 0x0003896C
		private async void Init()
		{
			MainAppRequestProducer.Delegate = new AddRequestsDelegate(this.UpdateRequestsDelegate);
			this.Model0.SelectedPID = PID.Empty;
			this.Model1.SelectedPID = PID.Empty;
			this.Model2.SelectedPID = PID.Empty;
			this.Model3.SelectedPID = PID.Empty;
			this.LDPS = LiveDataPageSettings.Load();
			switch (this.LDPS.ChartsVisible)
			{
			case 1:
			{
				this.ShowOneChart();
				PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId0);
				if (pid != null)
				{
					this.Model0.SelectedPID = pid;
				}
				break;
			}
			case 2:
			{
				this.Show2Charts();
				PID pid2 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId0);
				if (pid2 != null)
				{
					this.Model0.SelectedPID = pid2;
				}
				PID pid3 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId1);
				if (pid3 != null)
				{
					this.Model1.SelectedPID = pid3;
				}
				break;
			}
			case 3:
			{
				this.Show3Charts();
				PID pid4 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId0);
				if (pid4 != null)
				{
					this.Model0.SelectedPID = pid4;
				}
				PID pid5 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId1);
				if (pid5 != null)
				{
					this.Model1.SelectedPID = pid5;
				}
				PID pid6 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId2);
				if (pid6 != null)
				{
					this.Model2.SelectedPID = pid6;
				}
				break;
			}
			case 4:
			{
				this.Show4Charts();
				PID pid7 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId0);
				if (pid7 != null)
				{
					this.Model0.SelectedPID = pid7;
				}
				PID pid8 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId1);
				if (pid8 != null)
				{
					this.Model1.SelectedPID = pid8;
				}
				PID pid9 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId2);
				if (pid9 != null)
				{
					this.Model2.SelectedPID = pid9;
				}
				PID pid10 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == this.LDPS.PIDId3);
				if (pid10 != null)
				{
					this.Model3.SelectedPID = pid10;
				}
				break;
			}
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0003A7A4 File Offset: 0x000389A4
		private void Page_Appearing(object sender, EventArgs e)
		{
			if (this.isPaused)
			{
				this.Pause();
			}
			DependencyService.Get<IStatusBar>(0).HideStatusBar();
			if (!SharedSettings.Current.LiveData_InfoShowed)
			{
				SharedSettings.Current.LiveData_InfoShowed = true;
				base.DisplayAlert(Translate.GetString("ios_MainPage_TileLiveData"), Translate.GetString("ios_MainPage_TileLiveData_Info"), "OK");
			}
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0003A808 File Offset: 0x00038A08
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			MainAppRequestProducer.Delegate = null;
			this.Model0.Unsubscribe();
			this.Model1.Unsubscribe();
			this.Model2.Unsubscribe();
			this.Model3.Unsubscribe();
			this._Model0.PIDChanged -= this.Model_PidChanged;
			this._Model1.PIDChanged -= this.Model_PidChanged;
			this._Model2.PIDChanged -= this.Model_PidChanged;
			this._Model3.PIDChanged -= this.Model_PidChanged;
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
			await base.Navigation.PopAsync(true);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0003A83F File Offset: 0x00038A3F
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnBack_Clicked(this, null);
			}
			return true;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0003A86C File Offset: 0x00038A6C
		private void Page_Disappearing(object sender, EventArgs e)
		{
			try
			{
				DependencyService.Get<IStatusBar>(0).ShowStatusBar();
				LiveDataPageSettings ldps = this.LDPS;
				IPID selectedPID = this.Model0.SelectedPID;
				ldps.PIDId0 = ((selectedPID != null) ? selectedPID.Id : 0);
				LiveDataPageSettings ldps2 = this.LDPS;
				IPID selectedPID2 = this.Model1.SelectedPID;
				ldps2.PIDId1 = ((selectedPID2 != null) ? selectedPID2.Id : 0);
				LiveDataPageSettings ldps3 = this.LDPS;
				IPID selectedPID3 = this.Model2.SelectedPID;
				ldps3.PIDId2 = ((selectedPID3 != null) ? selectedPID3.Id : 0);
				LiveDataPageSettings ldps4 = this.LDPS;
				IPID selectedPID4 = this.Model3.SelectedPID;
				ldps4.PIDId3 = ((selectedPID4 != null) ? selectedPID4.Id : 0);
				if (this.Model3Grid.IsVisible)
				{
					this.LDPS.ChartsVisible = 4;
				}
				else if (this.Model2Grid.IsVisible)
				{
					this.LDPS.ChartsVisible = 3;
				}
				else if (this.Model1Grid.IsVisible)
				{
					this.LDPS.ChartsVisible = 2;
				}
				else
				{
					this.LDPS.ChartsVisible = 1;
				}
				this.LDPS.Save();
			}
			catch
			{
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0003A98C File Offset: 0x00038B8C
		public LiveDataPIDModel Model0
		{
			get
			{
				return this._Model0;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0003A994 File Offset: 0x00038B94
		public LiveDataPIDModel Model1
		{
			get
			{
				return this._Model1;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0003A99C File Offset: 0x00038B9C
		public LiveDataPIDModel Model2
		{
			get
			{
				return this._Model2;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0003A9A4 File Offset: 0x00038BA4
		public LiveDataPIDModel Model3
		{
			get
			{
				return this._Model3;
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0003A9AC File Offset: 0x00038BAC
		private void CreateModels()
		{
			this._Model0 = new LiveDataPIDModel
			{
				Name = "model0",
				Mode = LiveDataModes.LiveDataChart
			};
			this._Model1 = new LiveDataPIDModel
			{
				Name = "model1",
				Mode = LiveDataModes.LiveDataChart
			};
			this._Model2 = new LiveDataPIDModel
			{
				Name = "model2",
				Mode = LiveDataModes.LiveDataChart
			};
			this._Model3 = new LiveDataPIDModel
			{
				Name = "model3",
				Mode = LiveDataModes.LiveDataChart
			};
			this.models.Add(this._Model0);
			this.models.Add(this._Model1);
			this.models.Add(this._Model2);
			this.models.Add(this._Model3);
			this.Model0Grid.BindingContext = this.Model0;
			this.Model1Grid.BindingContext = this.Model1;
			this.Model2Grid.BindingContext = this.Model2;
			this.Model3Grid.BindingContext = this.Model3;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0003AAB8 File Offset: 0x00038CB8
		private async void Model_PidChanged(IPID NewPID, LiveDataPIDModel Model)
		{
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000027D4 File Offset: 0x000009D4
		private void SetTimer()
		{
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0003AAE8 File Offset: 0x00038CE8
		private void ShowOneChart()
		{
			this.BackgroudGrid.RowDefinitions.Clear();
			this.BackgroudGrid.ColumnDefinitions.Clear();
			this.Model0Grid.IsVisible = true;
			this.Model1Grid.IsVisible = false;
			this.Model2Grid.IsVisible = false;
			this.Model3Grid.IsVisible = false;
			this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, 1)
			});
			this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(1.0, 1)
			});
			Grid.SetRow(this.Model0Grid, 0);
			Grid.SetColumn(this.Model0Grid, 0);
			Grid.SetColumnSpan(this.Model0Grid, 1);
			Grid.SetRowSpan(this.Model0Grid, 1);
			Grid.SetRow(this.Model1Grid, 0);
			Grid.SetRow(this.Model2Grid, 0);
			Grid.SetRow(this.Model3Grid, 0);
			Grid.SetColumn(this.Model1Grid, 0);
			Grid.SetColumn(this.Model2Grid, 0);
			Grid.SetColumn(this.Model3Grid, 0);
			Grid.SetColumnSpan(this.Model1Grid, 1);
			Grid.SetColumnSpan(this.Model2Grid, 1);
			Grid.SetColumnSpan(this.Model3Grid, 1);
			Grid.SetRowSpan(this.Model1Grid, 1);
			Grid.SetRowSpan(this.Model2Grid, 1);
			Grid.SetRowSpan(this.Model3Grid, 1);
			this.Model1.SelectedPID = PID.Empty;
			this.Model2.SelectedPID = PID.Empty;
			this.Model3.SelectedPID = PID.Empty;
			this.LDPS.ChartsVisible = 1;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0003AC98 File Offset: 0x00038E98
		private void Show2Charts()
		{
			this.Model0Grid.IsVisible = true;
			this.Model1Grid.IsVisible = true;
			this.Model2Grid.IsVisible = false;
			this.Model3Grid.IsVisible = false;
			this.BackgroudGrid.RowDefinitions.Clear();
			this.BackgroudGrid.ColumnDefinitions.Clear();
			this.Model2.SelectedPID = PID.Empty;
			this.Model3.SelectedPID = PID.Empty;
			if (this.isPortrait)
			{
				this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1.0, 1)
				});
				Grid.SetRow(this.Model1Grid, 0);
				Grid.SetRow(this.Model2Grid, 0);
				Grid.SetRow(this.Model3Grid, 0);
				Grid.SetColumn(this.Model1Grid, 0);
				Grid.SetColumn(this.Model2Grid, 0);
				Grid.SetColumn(this.Model3Grid, 0);
				Grid.SetColumnSpan(this.Model1Grid, 1);
				Grid.SetColumnSpan(this.Model2Grid, 1);
				Grid.SetColumnSpan(this.Model3Grid, 1);
				Grid.SetRowSpan(this.Model1Grid, 1);
				Grid.SetRowSpan(this.Model2Grid, 1);
				Grid.SetRowSpan(this.Model3Grid, 1);
				Grid.SetRow(this.Model0Grid, 0);
				Grid.SetRow(this.Model1Grid, 1);
				Grid.SetColumn(this.Model0Grid, 0);
				Grid.SetColumn(this.Model1Grid, 0);
				Grid.SetColumnSpan(this.Model0Grid, 2);
				Grid.SetColumnSpan(this.Model1Grid, 2);
				Grid.SetRowSpan(this.Model0Grid, 1);
				Grid.SetRowSpan(this.Model1Grid, 1);
			}
			else
			{
				this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1.0, 1)
				});
				Grid.SetRow(this.Model1Grid, 0);
				Grid.SetRow(this.Model2Grid, 0);
				Grid.SetRow(this.Model3Grid, 0);
				Grid.SetColumn(this.Model1Grid, 0);
				Grid.SetColumn(this.Model2Grid, 0);
				Grid.SetColumn(this.Model3Grid, 0);
				Grid.SetColumnSpan(this.Model1Grid, 1);
				Grid.SetColumnSpan(this.Model2Grid, 1);
				Grid.SetColumnSpan(this.Model3Grid, 1);
				Grid.SetRowSpan(this.Model1Grid, 1);
				Grid.SetRowSpan(this.Model2Grid, 1);
				Grid.SetRowSpan(this.Model3Grid, 1);
				Grid.SetRow(this.Model0Grid, 0);
				Grid.SetRow(this.Model1Grid, 0);
				Grid.SetColumn(this.Model0Grid, 0);
				Grid.SetColumn(this.Model1Grid, 1);
				Grid.SetColumnSpan(this.Model0Grid, 1);
				Grid.SetColumnSpan(this.Model1Grid, 1);
				Grid.SetRowSpan(this.Model0Grid, 2);
				Grid.SetRowSpan(this.Model1Grid, 2);
			}
			this.LDPS.ChartsVisible = 2;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0003B010 File Offset: 0x00039210
		private async void AskForPurchase()
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
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0003B048 File Offset: 0x00039248
		private void Show3Charts()
		{
			this.Model0Grid.IsVisible = true;
			this.Model1Grid.IsVisible = true;
			this.Model2Grid.IsVisible = true;
			this.Model3Grid.IsVisible = false;
			this.BackgroudGrid.RowDefinitions.Clear();
			this.BackgroudGrid.ColumnDefinitions.Clear();
			if (this.isPortrait)
			{
				this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1.0, 1)
				});
				Grid.SetRow(this.Model0Grid, 0);
				Grid.SetRow(this.Model1Grid, 1);
				Grid.SetRow(this.Model2Grid, 2);
				Grid.SetRow(this.Model3Grid, 0);
				Grid.SetColumn(this.Model0Grid, 0);
				Grid.SetColumn(this.Model1Grid, 0);
				Grid.SetColumn(this.Model2Grid, 0);
				Grid.SetColumn(this.Model3Grid, 0);
			}
			else
			{
				this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1.0, 1)
				});
				this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1.0, 1)
				});
				Grid.SetRow(this.Model0Grid, 0);
				Grid.SetRow(this.Model1Grid, 0);
				Grid.SetRow(this.Model2Grid, 0);
				Grid.SetRow(this.Model3Grid, 0);
				Grid.SetColumn(this.Model0Grid, 0);
				Grid.SetColumn(this.Model1Grid, 1);
				Grid.SetColumn(this.Model2Grid, 2);
				Grid.SetColumn(this.Model3Grid, 0);
			}
			Grid.SetColumnSpan(this.Model0Grid, 1);
			Grid.SetColumnSpan(this.Model1Grid, 1);
			Grid.SetColumnSpan(this.Model2Grid, 1);
			Grid.SetColumnSpan(this.Model3Grid, 1);
			Grid.SetRowSpan(this.Model0Grid, 1);
			Grid.SetRowSpan(this.Model1Grid, 1);
			Grid.SetRowSpan(this.Model2Grid, 1);
			Grid.SetRowSpan(this.Model3Grid, 1);
			this.LDPS.ChartsVisible = 3;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0003B334 File Offset: 0x00039534
		private void Show4Charts()
		{
			this.Model0Grid.IsVisible = true;
			this.Model1Grid.IsVisible = true;
			this.Model2Grid.IsVisible = true;
			this.Model3Grid.IsVisible = true;
			this.BackgroudGrid.RowDefinitions.Clear();
			this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, 1)
			});
			this.BackgroudGrid.RowDefinitions.Add(new RowDefinition
			{
				Height = new GridLength(1.0, 1)
			});
			this.BackgroudGrid.ColumnDefinitions.Clear();
			this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(1.0, 1)
			});
			this.BackgroudGrid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(1.0, 1)
			});
			Grid.SetRow(this.Model0Grid, 0);
			Grid.SetRow(this.Model1Grid, 1);
			Grid.SetRow(this.Model2Grid, 0);
			Grid.SetRow(this.Model3Grid, 1);
			Grid.SetColumn(this.Model0Grid, 0);
			Grid.SetColumn(this.Model1Grid, 0);
			Grid.SetColumn(this.Model2Grid, 1);
			Grid.SetColumn(this.Model3Grid, 1);
			Grid.SetColumnSpan(this.Model0Grid, 1);
			Grid.SetColumnSpan(this.Model1Grid, 1);
			Grid.SetColumnSpan(this.Model2Grid, 1);
			Grid.SetColumnSpan(this.Model3Grid, 1);
			Grid.SetRowSpan(this.Model0Grid, 1);
			Grid.SetRowSpan(this.Model1Grid, 1);
			Grid.SetRowSpan(this.Model2Grid, 1);
			Grid.SetRowSpan(this.Model3Grid, 1);
			this.LDPS.ChartsVisible = 4;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0003B505 File Offset: 0x00039705
		private void btnShow1_Clicked(object sender, EventArgs e)
		{
			this.ShowOneChart();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0003B50D File Offset: 0x0003970D
		private void btnShow2_Clicked(object sender, EventArgs e)
		{
			this.Show2Charts();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0003B515 File Offset: 0x00039715
		private void btnShow3_Clicked(object sender, EventArgs e)
		{
			this.Show3Charts();
			this.AskForPurchase();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0003B523 File Offset: 0x00039723
		private void btnShow4_Clicked(object sender, EventArgs e)
		{
			this.Show4Charts();
			this.AskForPurchase();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0003B534 File Offset: 0x00039734
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			if (height >= width)
			{
				this.isPortrait = true;
			}
			else
			{
				this.isPortrait = false;
			}
			if (this.LDPS != null)
			{
				if (this.LDPS.ChartsVisible == 2)
				{
					this.Show2Charts();
				}
				if (this.LDPS.ChartsVisible == 3)
				{
					this.Show3Charts();
				}
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0003B590 File Offset: 0x00039790
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LiveDataChartPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/LiveDataChartPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 18);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 17);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 14);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 17);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 17);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 17);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 14);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 17);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 17);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 17);
			LinkButton linkButton4;
			VisualDiagnostics.RegisterSourceInfo(linkButton4 = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 14);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 17);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 17);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 17);
			LinkButton linkButton5;
			VisualDiagnostics.RegisterSourceInfo(linkButton5 = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 14);
			LinkButton linkButton6;
			VisualDiagnostics.RegisterSourceInfo(linkButton6 = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 14);
			LinkButton linkButton7;
			VisualDiagnostics.RegisterSourceInfo(linkButton7 = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 14);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 17);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 17);
			LinkButton linkButton8;
			VisualDiagnostics.RegisterSourceInfo(linkButton8 = new LinkButton(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 22);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 22);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 22);
			ColumnDefinition columnDefinition9;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 22);
			LiveDataChartUC liveDataChartUC;
			VisualDiagnostics.RegisterSourceInfo(liveDataChartUC = new LiveDataChartUC(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 18);
			LiveDataChartUC liveDataChartUC2;
			VisualDiagnostics.RegisterSourceInfo(liveDataChartUC2 = new LiveDataChartUC(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 21);
			LiveDataChartUC liveDataChartUC3;
			VisualDiagnostics.RegisterSourceInfo(liveDataChartUC3 = new LiveDataChartUC(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 18);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 21);
			LiveDataChartUC liveDataChartUC4;
			VisualDiagnostics.RegisterSourceInfo(liveDataChartUC4 = new LiveDataChartUC(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\LiveDataChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("btnBack", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "btnBack";
			}
			nameScope.RegisterName("btn1", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "btn1";
			}
			nameScope.RegisterName("btn2", linkButton3);
			if (linkButton3.StyleId == null)
			{
				linkButton3.StyleId = "btn2";
			}
			nameScope.RegisterName("btn3", linkButton4);
			if (linkButton4.StyleId == null)
			{
				linkButton4.StyleId = "btn3";
			}
			nameScope.RegisterName("btn4", linkButton5);
			if (linkButton5.StyleId == null)
			{
				linkButton5.StyleId = "btn4";
			}
			nameScope.RegisterName("btnPause", linkButton6);
			if (linkButton6.StyleId == null)
			{
				linkButton6.StyleId = "btnPause";
			}
			nameScope.RegisterName("btnPlay", linkButton7);
			if (linkButton7.StyleId == null)
			{
				linkButton7.StyleId = "btnPlay";
			}
			nameScope.RegisterName("btnInfo", linkButton8);
			if (linkButton8.StyleId == null)
			{
				linkButton8.StyleId = "btnInfo";
			}
			nameScope.RegisterName("LayoutRoot", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("BackgroudGrid", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "BackgroudGrid";
			}
			nameScope.RegisterName("Model0Grid", liveDataChartUC);
			if (liveDataChartUC.StyleId == null)
			{
				liveDataChartUC.StyleId = "Model0Grid";
			}
			nameScope.RegisterName("Model1Grid", liveDataChartUC2);
			if (liveDataChartUC2.StyleId == null)
			{
				liveDataChartUC2.StyleId = "Model1Grid";
			}
			nameScope.RegisterName("Model2Grid", liveDataChartUC3);
			if (liveDataChartUC3.StyleId == null)
			{
				liveDataChartUC3.StyleId = "Model2Grid";
			}
			nameScope.RegisterName("Model3Grid", liveDataChartUC4);
			if (liveDataChartUC4.StyleId == null)
			{
				liveDataChartUC4.StyleId = "Model3Grid";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.gridButtons = grid;
			this.btnBack = linkButton;
			this.btn1 = linkButton2;
			this.btn2 = linkButton3;
			this.btn3 = linkButton4;
			this.btn4 = linkButton5;
			this.btnPause = linkButton6;
			this.btnPlay = linkButton7;
			this.btnInfo = linkButton8;
			this.LayoutRoot = grid3;
			this.BackgroudGrid = grid2;
			this.Model0Grid = liveDataChartUC;
			this.Model1Grid = liveDataChartUC2;
			this.Model2Grid = liveDataChartUC3;
			this.Model3Grid = liveDataChartUC4;
			this.ad = complexAdView;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
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
			xmlNamespaceResolver.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
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
			xmlNamespaceResolver2.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Back";
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
			xmlNamespaceResolver3.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			grid.Children.Add(linkButton);
			linkButton2.SetValue(Grid.ColumnProperty, 1);
			linkButton2.SetValue(View.MarginProperty, new Thickness(0.0));
			dynamicResourceExtension3.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = linkButton2;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			linkButton2.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton2.Clicked += this.btnShow1_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension4.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = linkButton2;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 17)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			linkButton2.SetValue(Button.TextProperty, " 1 ");
			dynamicResourceExtension5.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = linkButton2;
			array6[1] = grid;
			array6[2] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, Button.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 17)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			linkButton2.SetDynamicResource(Button.TextColorProperty, dynamicResource5.Key);
			grid.Children.Add(linkButton2);
			linkButton3.SetValue(Grid.ColumnProperty, 2);
			linkButton3.SetValue(View.MarginProperty, new Thickness(0.0));
			dynamicResourceExtension6.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton3;
			array7[1] = grid;
			array7[2] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array7, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 17)));
			DynamicResource dynamicResource6 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			linkButton3.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton3.Clicked += this.btnShow2_Clicked;
			linkButton3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension7.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = linkButton3;
			array8[1] = grid;
			array8[2] = this;
			object obj9;
			xamlServiceProvider8.Add(typeFromHandle15, obj9 = new SimpleValueTargetProvider(array8, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 17)));
			DynamicResource dynamicResource7 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton3.SetDynamicResource(VisualElement.StyleProperty, dynamicResource7.Key);
			linkButton3.SetValue(Button.TextProperty, " 2 ");
			dynamicResourceExtension8.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = linkButton3;
			array9[1] = grid;
			array9[2] = this;
			object obj10;
			xamlServiceProvider9.Add(typeFromHandle17, obj10 = new SimpleValueTargetProvider(array9, Button.TextColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 17)));
			DynamicResource dynamicResource8 = markupExtension9.ProvideValue(xamlServiceProvider9);
			linkButton3.SetDynamicResource(Button.TextColorProperty, dynamicResource8.Key);
			grid.Children.Add(linkButton3);
			linkButton4.SetValue(Grid.ColumnProperty, 3);
			linkButton4.SetValue(View.MarginProperty, new Thickness(0.0));
			dynamicResourceExtension9.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = linkButton4;
			array10[1] = grid;
			array10[2] = this;
			object obj11;
			xamlServiceProvider10.Add(typeFromHandle19, obj11 = new SimpleValueTargetProvider(array10, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 17)));
			DynamicResource dynamicResource9 = markupExtension10.ProvideValue(xamlServiceProvider10);
			linkButton4.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource9.Key);
			linkButton4.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton4.Clicked += this.btnShow3_Clicked;
			linkButton4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension10.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = linkButton4;
			array11[1] = grid;
			array11[2] = this;
			object obj12;
			xamlServiceProvider11.Add(typeFromHandle21, obj12 = new SimpleValueTargetProvider(array11, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 17)));
			DynamicResource dynamicResource10 = markupExtension11.ProvideValue(xamlServiceProvider11);
			linkButton4.SetDynamicResource(VisualElement.StyleProperty, dynamicResource10.Key);
			linkButton4.SetValue(Button.TextProperty, " 3 ");
			dynamicResourceExtension11.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = linkButton4;
			array12[1] = grid;
			array12[2] = this;
			object obj13;
			xamlServiceProvider12.Add(typeFromHandle23, obj13 = new SimpleValueTargetProvider(array12, Button.TextColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 17)));
			DynamicResource dynamicResource11 = markupExtension12.ProvideValue(xamlServiceProvider12);
			linkButton4.SetDynamicResource(Button.TextColorProperty, dynamicResource11.Key);
			grid.Children.Add(linkButton4);
			linkButton5.SetValue(Grid.ColumnProperty, 4);
			linkButton5.SetValue(View.MarginProperty, new Thickness(0.0));
			dynamicResourceExtension12.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = linkButton5;
			array13[1] = grid;
			array13[2] = this;
			object obj14;
			xamlServiceProvider13.Add(typeFromHandle25, obj14 = new SimpleValueTargetProvider(array13, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 17)));
			DynamicResource dynamicResource12 = markupExtension13.ProvideValue(xamlServiceProvider13);
			linkButton5.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource12.Key);
			linkButton5.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton5.Clicked += this.btnShow4_Clicked;
			linkButton5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension13.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = linkButton5;
			array14[1] = grid;
			array14[2] = this;
			object obj15;
			xamlServiceProvider14.Add(typeFromHandle27, obj15 = new SimpleValueTargetProvider(array14, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 17)));
			DynamicResource dynamicResource13 = markupExtension14.ProvideValue(xamlServiceProvider14);
			linkButton5.SetDynamicResource(VisualElement.StyleProperty, dynamicResource13.Key);
			linkButton5.SetValue(Button.TextProperty, " 4 ");
			dynamicResourceExtension14.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 3];
			array15[0] = linkButton5;
			array15[1] = grid;
			array15[2] = this;
			object obj16;
			xamlServiceProvider15.Add(typeFromHandle29, obj16 = new SimpleValueTargetProvider(array15, Button.TextColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 17)));
			DynamicResource dynamicResource14 = markupExtension15.ProvideValue(xamlServiceProvider15);
			linkButton5.SetDynamicResource(Button.TextColorProperty, dynamicResource14.Key);
			grid.Children.Add(linkButton5);
			linkButton6.SetValue(Grid.ColumnProperty, 5);
			linkButton6.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton6.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton6.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton6.Clicked += this.btnPause_Clicked;
			linkButton6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			linkButton6.SetValue(Button.ImageProperty, new FileImageSourceConverter().ConvertFromInvariantString("icons8_pause_white.png"));
			linkButton6.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton6);
			linkButton7.SetValue(Grid.ColumnProperty, 5);
			linkButton7.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton7.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton7.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton7.Clicked += this.btnPlay_Clicked;
			linkButton7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			linkButton7.SetValue(Button.ImageProperty, new FileImageSourceConverter().ConvertFromInvariantString("icons8_play_white.png"));
			linkButton7.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			linkButton7.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton7);
			linkButton8.SetValue(Grid.ColumnProperty, 6);
			linkButton8.Clicked += this.btnInfo_Clicked;
			linkButton8.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension15.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 3];
			array16[0] = linkButton8;
			array16[1] = grid;
			array16[2] = this;
			object obj17;
			xamlServiceProvider16.Add(typeFromHandle31, obj17 = new SimpleValueTargetProvider(array16, Button.ImageProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 17)));
			DynamicResource dynamicResource15 = markupExtension16.ProvideValue(xamlServiceProvider16);
			linkButton8.SetDynamicResource(Button.ImageProperty, dynamicResource15.Key);
			dynamicResourceExtension16.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 3];
			array17[0] = linkButton8;
			array17[1] = grid;
			array17[2] = this;
			object obj18;
			xamlServiceProvider17.Add(typeFromHandle33, obj18 = new SimpleValueTargetProvider(array17, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(LiveDataChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(124, 17)));
			DynamicResource dynamicResource16 = markupExtension17.ProvideValue(xamlServiceProvider17);
			linkButton8.SetDynamicResource(VisualElement.StyleProperty, dynamicResource16.Key);
			grid.Children.Add(linkButton8);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform2.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform2.WinPhone = new Thickness(0.0);
			onPlatform2.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid3.SetValue(View.MarginProperty, onPlatform2);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			grid2.SetValue(Grid.RowProperty, 1);
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			columnDefinition9.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition9);
			liveDataChartUC.SetValue(Grid.RowProperty, 0);
			liveDataChartUC.SetValue(Grid.ColumnProperty, 0);
			liveDataChartUC.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			liveDataChartUC.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
			grid2.Children.Add(liveDataChartUC);
			liveDataChartUC2.SetValue(Grid.RowProperty, 1);
			liveDataChartUC2.SetValue(Grid.ColumnProperty, 0);
			grid2.Children.Add(liveDataChartUC2);
			liveDataChartUC3.SetValue(Grid.RowProperty, 0);
			liveDataChartUC3.SetValue(Grid.ColumnProperty, 1);
			bindingExtension.Path = "Settings.AdsProductPurchased";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			liveDataChartUC3.SetBinding(VisualElement.IsEnabledProperty, bindingBase);
			grid2.Children.Add(liveDataChartUC3);
			liveDataChartUC4.SetValue(Grid.RowProperty, 1);
			liveDataChartUC4.SetValue(Grid.ColumnProperty, 1);
			bindingExtension2.Path = "Settings.AdsProductPurchased";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			liveDataChartUC4.SetBinding(VisualElement.IsEnabledProperty, bindingBase2);
			grid2.Children.Add(liveDataChartUC4);
			grid3.Children.Add(grid2);
			complexAdView.SetValue(Grid.RowProperty, 2);
			complexAdView.SetValue(View.MarginProperty, new Thickness(-5.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 50.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid3.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0003DF47 File Offset: 0x0003C147
		[CompilerGenerated]
		private bool <Init>b__11_0(PID x)
		{
			return x.Id == this.LDPS.PIDId0;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0003DF47 File Offset: 0x0003C147
		[CompilerGenerated]
		private bool <Init>b__11_1(PID x)
		{
			return x.Id == this.LDPS.PIDId0;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0003DF5C File Offset: 0x0003C15C
		[CompilerGenerated]
		private bool <Init>b__11_2(PID x)
		{
			return x.Id == this.LDPS.PIDId1;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0003DF47 File Offset: 0x0003C147
		[CompilerGenerated]
		private bool <Init>b__11_3(PID x)
		{
			return x.Id == this.LDPS.PIDId0;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0003DF5C File Offset: 0x0003C15C
		[CompilerGenerated]
		private bool <Init>b__11_4(PID x)
		{
			return x.Id == this.LDPS.PIDId1;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0003DF71 File Offset: 0x0003C171
		[CompilerGenerated]
		private bool <Init>b__11_5(PID x)
		{
			return x.Id == this.LDPS.PIDId2;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0003DF47 File Offset: 0x0003C147
		[CompilerGenerated]
		private bool <Init>b__11_6(PID x)
		{
			return x.Id == this.LDPS.PIDId0;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0003DF5C File Offset: 0x0003C15C
		[CompilerGenerated]
		private bool <Init>b__11_7(PID x)
		{
			return x.Id == this.LDPS.PIDId1;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0003DF71 File Offset: 0x0003C171
		[CompilerGenerated]
		private bool <Init>b__11_8(PID x)
		{
			return x.Id == this.LDPS.PIDId2;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0003DF86 File Offset: 0x0003C186
		[CompilerGenerated]
		private bool <Init>b__11_9(PID x)
		{
			return x.Id == this.LDPS.PIDId3;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0003DF9C File Offset: 0x0003C19C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LiveDataChartPage>(this, typeof(LiveDataChartPage));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.btnBack = NameScopeExtensions.FindByName<LinkButton>(this, "btnBack");
			this.btn1 = NameScopeExtensions.FindByName<LinkButton>(this, "btn1");
			this.btn2 = NameScopeExtensions.FindByName<LinkButton>(this, "btn2");
			this.btn3 = NameScopeExtensions.FindByName<LinkButton>(this, "btn3");
			this.btn4 = NameScopeExtensions.FindByName<LinkButton>(this, "btn4");
			this.btnPause = NameScopeExtensions.FindByName<LinkButton>(this, "btnPause");
			this.btnPlay = NameScopeExtensions.FindByName<LinkButton>(this, "btnPlay");
			this.btnInfo = NameScopeExtensions.FindByName<LinkButton>(this, "btnInfo");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.BackgroudGrid = NameScopeExtensions.FindByName<Grid>(this, "BackgroudGrid");
			this.Model0Grid = NameScopeExtensions.FindByName<LiveDataChartUC>(this, "Model0Grid");
			this.Model1Grid = NameScopeExtensions.FindByName<LiveDataChartUC>(this, "Model1Grid");
			this.Model2Grid = NameScopeExtensions.FindByName<LiveDataChartUC>(this, "Model2Grid");
			this.Model3Grid = NameScopeExtensions.FindByName<LiveDataChartUC>(this, "Model3Grid");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x040002F9 RID: 761
		private bool isPortrait = true;

		// Token: 0x040002FA RID: 762
		private bool isPaused;

		// Token: 0x040002FB RID: 763
		private LiveDataPIDModel _Model0;

		// Token: 0x040002FC RID: 764
		private LiveDataPIDModel _Model1;

		// Token: 0x040002FD RID: 765
		private LiveDataPIDModel _Model2;

		// Token: 0x040002FE RID: 766
		private LiveDataPIDModel _Model3;

		// Token: 0x040002FF RID: 767
		private ObservableCollection<LiveDataPIDModel> models = new ObservableCollection<LiveDataPIDModel>();

		// Token: 0x04000300 RID: 768
		private LiveDataPageSettings LDPS;

		// Token: 0x04000301 RID: 769
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04000302 RID: 770
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnBack;

		// Token: 0x04000303 RID: 771
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btn1;

		// Token: 0x04000304 RID: 772
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btn2;

		// Token: 0x04000305 RID: 773
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btn3;

		// Token: 0x04000306 RID: 774
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btn4;

		// Token: 0x04000307 RID: 775
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPause;

		// Token: 0x04000308 RID: 776
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPlay;

		// Token: 0x04000309 RID: 777
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnInfo;

		// Token: 0x0400030A RID: 778
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x0400030B RID: 779
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid BackgroudGrid;

		// Token: 0x0400030C RID: 780
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LiveDataChartUC Model0Grid;

		// Token: 0x0400030D RID: 781
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LiveDataChartUC Model1Grid;

		// Token: 0x0400030E RID: 782
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LiveDataChartUC Model2Grid;

		// Token: 0x0400030F RID: 783
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LiveDataChartUC Model3Grid;

		// Token: 0x04000310 RID: 784
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x020000DA RID: 218
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AskForPurchase>d__35 : IAsyncStateMachine
		{
			// Token: 0x0600044C RID: 1100 RVA: 0x0003E0CC File Offset: 0x0003C2CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataChartPage liveDataChartPage = this;
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
							goto IL_0102;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							goto IL_0109;
						}
						taskAwaiter5 = liveDataChartPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, LiveDataChartPage.<AskForPurchase>d__35>(ref taskAwaiter5, ref this);
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
						goto IL_0109;
					}
					taskAwaiter3 = liveDataChartPage.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataChartPage.<AskForPurchase>d__35>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0102:
					taskAwaiter3.GetResult();
					IL_0109:;
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

			// Token: 0x0600044D RID: 1101 RVA: 0x0003E220 File Offset: 0x0003C420
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000311 RID: 785
			public int <>1__state;

			// Token: 0x04000312 RID: 786
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000313 RID: 787
			public LiveDataChartPage <>4__this;

			// Token: 0x04000314 RID: 788
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000315 RID: 789
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020000DB RID: 219
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Init>d__11 : IAsyncStateMachine
		{
			// Token: 0x0600044E RID: 1102 RVA: 0x0003E230 File Offset: 0x0003C430
			void IAsyncStateMachine.MoveNext()
			{
				LiveDataChartPage liveDataChartPage = this;
				try
				{
					MainAppRequestProducer.Delegate = new AddRequestsDelegate(liveDataChartPage.UpdateRequestsDelegate);
					liveDataChartPage.Model0.SelectedPID = PID.Empty;
					liveDataChartPage.Model1.SelectedPID = PID.Empty;
					liveDataChartPage.Model2.SelectedPID = PID.Empty;
					liveDataChartPage.Model3.SelectedPID = PID.Empty;
					liveDataChartPage.LDPS = LiveDataPageSettings.Load();
					switch (liveDataChartPage.LDPS.ChartsVisible)
					{
					case 1:
					{
						liveDataChartPage.ShowOneChart();
						PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId0);
						if (pid != null)
						{
							liveDataChartPage.Model0.SelectedPID = pid;
						}
						break;
					}
					case 2:
					{
						liveDataChartPage.Show2Charts();
						PID pid2 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId0);
						if (pid2 != null)
						{
							liveDataChartPage.Model0.SelectedPID = pid2;
						}
						PID pid3 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId1);
						if (pid3 != null)
						{
							liveDataChartPage.Model1.SelectedPID = pid3;
						}
						break;
					}
					case 3:
					{
						liveDataChartPage.Show3Charts();
						PID pid4 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId0);
						if (pid4 != null)
						{
							liveDataChartPage.Model0.SelectedPID = pid4;
						}
						PID pid5 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId1);
						if (pid5 != null)
						{
							liveDataChartPage.Model1.SelectedPID = pid5;
						}
						PID pid6 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId2);
						if (pid6 != null)
						{
							liveDataChartPage.Model2.SelectedPID = pid6;
						}
						break;
					}
					case 4:
					{
						liveDataChartPage.Show4Charts();
						PID pid7 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId0);
						if (pid7 != null)
						{
							liveDataChartPage.Model0.SelectedPID = pid7;
						}
						PID pid8 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId1);
						if (pid8 != null)
						{
							liveDataChartPage.Model1.SelectedPID = pid8;
						}
						PID pid9 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId2);
						if (pid9 != null)
						{
							liveDataChartPage.Model2.SelectedPID = pid9;
						}
						PID pid10 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == liveDataChartPage.LDPS.PIDId3);
						if (pid10 != null)
						{
							liveDataChartPage.Model3.SelectedPID = pid10;
						}
						break;
					}
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

			// Token: 0x0600044F RID: 1103 RVA: 0x0003E4D8 File Offset: 0x0003C6D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000316 RID: 790
			public int <>1__state;

			// Token: 0x04000317 RID: 791
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000318 RID: 792
			public LiveDataChartPage <>4__this;
		}

		// Token: 0x020000DC RID: 220
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Model_PidChanged>d__31 : IAsyncStateMachine
		{
			// Token: 0x06000450 RID: 1104 RVA: 0x0003E4E8 File Offset: 0x0003C6E8
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
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

			// Token: 0x06000451 RID: 1105 RVA: 0x0003E538 File Offset: 0x0003C738
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000319 RID: 793
			public int <>1__state;

			// Token: 0x0400031A RID: 794
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x020000DD RID: 221
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__13 : IAsyncStateMachine
		{
			// Token: 0x06000452 RID: 1106 RVA: 0x0003E548 File Offset: 0x0003C748
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataChartPage liveDataChartPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						MainAppRequestProducer.Delegate = null;
						liveDataChartPage.Model0.Unsubscribe();
						liveDataChartPage.Model1.Unsubscribe();
						liveDataChartPage.Model2.Unsubscribe();
						liveDataChartPage.Model3.Unsubscribe();
						liveDataChartPage._Model0.PIDChanged -= liveDataChartPage.Model_PidChanged;
						liveDataChartPage._Model1.PIDChanged -= liveDataChartPage.Model_PidChanged;
						liveDataChartPage._Model2.PIDChanged -= liveDataChartPage.Model_PidChanged;
						liveDataChartPage._Model3.PIDChanged -= liveDataChartPage.Model_PidChanged;
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
						taskAwaiter = liveDataChartPage.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, LiveDataChartPage.<btnBack_Clicked>d__13>(ref taskAwaiter, ref this);
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

			// Token: 0x06000453 RID: 1107 RVA: 0x0003E6D8 File Offset: 0x0003C8D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400031B RID: 795
			public int <>1__state;

			// Token: 0x0400031C RID: 796
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400031D RID: 797
			public LiveDataChartPage <>4__this;

			// Token: 0x0400031E RID: 798
			private TaskAwaiter<Page> <>u__1;
		}
	}
}
