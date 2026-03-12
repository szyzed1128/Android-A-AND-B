using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using FFImageLoading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x02000290 RID: 656
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsDashboardV3.xaml")]
	public class SettingsDashboardV3 : ContentPage
	{
		// Token: 0x0600202D RID: 8237 RVA: 0x0016D768 File Offset: 0x0016B968
		public SettingsDashboardV3()
		{
			this.InitializeComponent();
			base.BindingContext = SharedSettings.Current;
			base.Appearing += this.SettingsPage_Appearing;
			base.Disappearing += this.SettingsPage_Disappearing;
			this.UpdatePreview();
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0016D7B8 File Offset: 0x0016B9B8
		private void SettingsPage_Disappearing(object sender, EventArgs e)
		{
			try
			{
				base.BindingContext = null;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x0016D7E4 File Offset: 0x0016B9E4
		private void SettingsPage_Appearing(object sender, EventArgs e)
		{
			if (base.BindingContext == null)
			{
				base.BindingContext = SharedSettings.Current;
			}
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x0016D7FC File Offset: 0x0016B9FC
		private async void btnApplyDashboardTheme_Clicked(object sender, EventArgs e)
		{
			DashboardListViewModel.Current.LoadDashboardFromSettings();
			foreach (DashboardPage dashboardPage in DashboardListViewModel.Current.Pages)
			{
				foreach (DashboardItem dashboardItem in dashboardPage.Items)
				{
					try
					{
						DashboardItem.ApplyThemeToItem(dashboardItem);
						dashboardItem.SelectAndAddControl();
					}
					catch (Exception ex)
					{
						ObjectDisposedException ex2 = ex as ObjectDisposedException;
					}
				}
			}
			DashboardListViewModel.Current.SaveDashboardToSettings();
			await base.DisplayAlert(Translate.GetString("ios_DashboardThemeApplyed"), "", "OK");
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0016D834 File Offset: 0x0016BA34
		private async void DashboardThemeItem_Tapped(object sender, EventArgs e)
		{
			this.UpdatePreview();
			await Task.Delay(300);
			this.UpdatePreview();
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0016D86C File Offset: 0x0016BA6C
		private async Task UpdatePreview()
		{
			PIDWithFloatValueFormula pid = PIDWithFloatValueFormula.PID010C_EngineRPM();
			this.gridDashPreview.Children.Clear();
			await Task.Run(delegate
			{
				try
				{
					this.dashItem = new DashboardItem();
					this.dashItem.HeightRequest = 150.0;
					this.dashItem.ItemType = DashboardItemTypes.Gauge;
					this.dashItem.ShowAvg = false;
					this.dashItem.Minimum = 0.0;
					this.dashItem.Maximum = 7000.0;
					this.dashItem.GaugeShowRedLine = true;
					this.dashItem.GaugeRedLineStart = 6000.0;
					this.dashItem.GaugeRedLineFinish = 7000.0;
					this.dashItem.HorizontalOptions = LayoutOptions.Center;
					this.dashItem.Model = new LiveDataPIDModel();
					this.dashItem.Model.SelectedPID = pid;
					this.dashItem.TitleFontSize = 0.0;
					this.dashItem.UnitsFontSize = 0.0;
					this.dashItem.SelectAndAddControl();
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
					{
						pid.SetValue(3000.0);
					}
				}
				catch (Exception)
				{
				}
			});
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				this.gridDashPreview.Children.Add(this.dashItem);
				DashboardItem.ApplyThemeToItem(this.dashItem);
				pid.SetValue(3000.0);
			});
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0016D8B0 File Offset: 0x0016BAB0
		private async void btnBackup_Clicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(SharedSettings.Current.Dashboard))
			{
				string localFilePath = FileSystemHelper.GetLocalFilePath("dashboard.json");
				using (FileStream fileStream = File.Open(localFilePath, FileMode.Create))
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream))
					{
						streamWriter.Write(SharedSettings.Current.Dashboard);
						streamWriter.Flush();
					}
				}
				try
				{
					await Share.RequestAsync(new ShareFileRequest
					{
						Title = "Dashboard backup",
						File = new ShareFile(localFilePath)
					});
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			await base.DisplayAlert("Dashboard is empty!", "", "OK");
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x0016D8E8 File Offset: 0x0016BAE8
		private async void btnRestore_Clicked(object sender, EventArgs e)
		{
			int num = 0;
			int num2;
			try
			{
				FileResult fileResult = await FilePicker.PickAsync(null);
				if (fileResult != null && fileResult.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
				{
					using (StreamReader sr = new StreamReader(fileResult.FullPath, Encoding.UTF8))
					{
						string text = sr.ReadToEnd();
						num2 = 0;
						try
						{
							Json.DeserializeObject<List<ProxyPage>>(text);
							SharedSettings.Current.Dashboard = text;
						}
						catch (Exception obj)
						{
							num2 = 1;
						}
						if (num2 == 1)
						{
							object obj;
							Exception ex = (Exception)obj;
							await base.DisplayAlert("Error!", "Wrong JSON Dashboard format!", "OK");
						}
					}
					StreamReader sr = null;
				}
			}
			catch (Exception obj2)
			{
				num = 1;
			}
			num2 = num;
			object obj2;
			if (num2 == 1)
			{
				Exception ex2 = (Exception)obj2;
				await base.DisplayAlert("Error!", "", "OK");
			}
			obj2 = null;
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x0016D920 File Offset: 0x0016BB20
		private async void btnPickBackgroundImage_Clicked(object sender, EventArgs e)
		{
			int num = 0;
			try
			{
				Stream stream = await DependencyService.Get<IPhotoPickerService>(0).GetImageStreamAsync();
				using (Stream img_stream = stream)
				{
					if (img_stream != null)
					{
						using (FileStream fstream = File.Create(FileSystemHelper.GetLocalFilePath("dashbg")))
						{
							await img_stream.CopyToAsync(fstream);
						}
						FileStream fstream = null;
						await ImageService.Instance.InvalidateCacheAsync(2);
						this.btnResetImage.IsVisible = true;
						DashboardListViewModel.Current.LoadDashboardFromSettings();
						if (DashboardListViewModel.Current.Pages.Any((DashboardPage page) => page.Items.Any((DashboardItem item) => item.BackgroundColor != Color.Transparent)))
						{
							TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_DashSetItemsToTransparentTitle"), Translate.GetString("ios_DashSetItemsToTransparentText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								TaskAwaiter<bool> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter.GetResult())
							{
								foreach (DashboardPage dashboardPage in DashboardListViewModel.Current.Pages)
								{
									foreach (DashboardItem dashboardItem in dashboardPage.Items)
									{
										dashboardItem.BackgroundColor = Color.Transparent;
										dashboardItem.ShowDefaultBackground = false;
									}
								}
							}
							DashboardListViewModel.Current.SaveDashboardToSettings();
						}
					}
				}
				Stream img_stream = null;
			}
			catch (Exception obj)
			{
				num = 1;
			}
			object obj;
			if (num == 1)
			{
				Exception ex = (Exception)obj;
				await base.DisplayAlert("Error!", (ex != null) ? ex.ToString() : "", "OK");
			}
			obj = null;
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0016D958 File Offset: 0x0016BB58
		private async void btnResetImage_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ResetDashboardBackgroundImage") + "?", "", Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				await this.DeleteDashboardBackgroundImage();
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x0016D990 File Offset: 0x0016BB90
		private async Task DeleteDashboardBackgroundImage()
		{
			string localFilePath = FileSystemHelper.GetLocalFilePath("dashbg");
			int num = 0;
			try
			{
				if (File.Exists(localFilePath))
				{
					File.Delete(localFilePath);
					this.btnResetImage.IsVisible = false;
				}
			}
			catch (Exception obj)
			{
				num = 1;
			}
			if (num == 1)
			{
				object obj;
				Exception ex = (Exception)obj;
				await base.DisplayAlert("Error", (ex != null) ? ex.ToString() : "", "OK");
			}
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0016D9D4 File Offset: 0x0016BBD4
		private async void btnResetDashboard_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ResetDashboard_Title"), Translate.GetString("ios_ResetDashboard_Text"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				SettingsDashboardV3.<>c__DisplayClass12_0 CS$<>8__locals1 = new SettingsDashboardV3.<>c__DisplayClass12_0();
				(sender as ButtonCell).IsEnabled = false;
				DashboardListViewModel.Current.ClearDashboard();
				await this.DeleteDashboardBackgroundImage();
				CS$<>8__locals1.profile = null;
				await Task.Run(delegate
				{
					SettingsDashboardV3.<>c__DisplayClass12_0.<<btnResetDashboard_Clicked>b__0>d <<btnResetDashboard_Clicked>b__0>d;
					<<btnResetDashboard_Clicked>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<btnResetDashboard_Clicked>b__0>d.<>4__this = CS$<>8__locals1;
					<<btnResetDashboard_Clicked>b__0>d.<>1__state = -1;
					<<btnResetDashboard_Clicked>b__0>d.<>t__builder.Start<SettingsDashboardV3.<>c__DisplayClass12_0.<<btnResetDashboard_Clicked>b__0>d>(ref <<btnResetDashboard_Clicked>b__0>d);
					return <<btnResetDashboard_Clicked>b__0>d.<>t__builder.Task;
				});
				if (CS$<>8__locals1.profile != null)
				{
					CS$<>8__locals1.profile.ApplyTweaks(true);
				}
				(sender as ButtonCell).IsEnabled = true;
				CS$<>8__locals1 = null;
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0016DA14 File Offset: 0x0016BC14
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsDashboardV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsDashboardV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			IntToStringInItemsConverter intToStringInItemsConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringInItemsConverter = new IntToStringInItemsConverter(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 81);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 26);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 31);
			int num = 0;
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 31);
			int num2 = 1;
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 31);
			int num3 = 2;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 25);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 21);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 14);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 25);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 49);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 113);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 18);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 49);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 98);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 49);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 114);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched3;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched3 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 18);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 49);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 112);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched4;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched4 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 18);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 21);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 18);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 49);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 97);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched5;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched5 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 18);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 49);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 97);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched6;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched6 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 18);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 49);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 108);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched7;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched7 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 18);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 49);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 125);
			OnPlatform<bool> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched8;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched8 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 18);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 49);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 111);
			OnPlatform<bool> onPlatform4;
			VisualDiagnostics.RegisterSourceInfo(onPlatform4 = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched9;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched9 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 14);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 25);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 21);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 21);
			ButtonCell buttonCell4;
			VisualDiagnostics.RegisterSourceInfo(buttonCell4 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 18);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 21);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 21);
			ButtonCell buttonCell5;
			VisualDiagnostics.RegisterSourceInfo(buttonCell5 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 14);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 25);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 21);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 21);
			ButtonCell buttonCell6;
			VisualDiagnostics.RegisterSourceInfo(buttonCell6 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsDashboardV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("gridDashPreview", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridDashPreview";
			}
			nameScope.RegisterName("btnPickBackgroundImage", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "btnPickBackgroundImage";
			}
			nameScope.RegisterName("btnResetImage", buttonCell3);
			if (buttonCell3.StyleId == null)
			{
				buttonCell3.StyleId = "btnResetImage";
			}
			nameScope.RegisterName("btnBackup", buttonCell4);
			if (buttonCell4.StyleId == null)
			{
				buttonCell4.StyleId = "btnBackup";
			}
			nameScope.RegisterName("btnRestore", buttonCell5);
			if (buttonCell5.StyleId == null)
			{
				buttonCell5.StyleId = "btnRestore";
			}
			this.settingsLayoutRoot = settingsView;
			this.gridDashPreview = grid;
			this.btnPickBackgroundImage = buttonCell2;
			this.btnResetImage = buttonCell3;
			this.btnBackup = buttonCell4;
			this.btnRestore = buttonCell5;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			resourceDictionary.Add("IntToStringInItemsConverter", intToStringInItemsConverter);
			translate.Text = "ios_DashboardSettings";
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
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
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
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "ios_DashboardChooseTheme";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = section;
			array3[1] = settingsView;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, SectionBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 25)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section.Title = obj5;
			bindingExtension.Mode = 1;
			bindingExtension.Path = "DashboardTheme";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.DashboardTheme, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.DashboardTheme = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DashboardTheme")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(RadioCell.SelectedValueProperty, bindingBase);
			frame.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			frame.SetValue(Frame.BorderColorProperty, Color.Transparent);
			frame.SetValue(VisualElement.HeightRequestProperty, 150.0);
			frame.SetValue(VisualElement.WidthRequestProperty, 150.0);
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			frame.SetValue(ContentView.ContentProperty, grid);
			customCell.SetValue(CustomCell.ContentProperty, frame);
			section.Add(customCell);
			translate3.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = radioCell;
			array4[1] = section;
			array4[2] = settingsView;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, CellBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 31)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			radioCell.Title = obj7;
			radioCell.Tapped += this.DashboardThemeItem_Tapped;
			radioCell.SetValue(RadioCell.ValueProperty, num);
			section.Add(radioCell);
			translate4.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = radioCell2;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, CellBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 31)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			radioCell2.Title = obj9;
			radioCell2.Tapped += this.DashboardThemeItem_Tapped;
			radioCell2.SetValue(RadioCell.ValueProperty, num2);
			section.Add(radioCell2);
			translate5.Text = "ios_DashboardThemeCarScanner";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = radioCell3;
			array6[1] = section;
			array6[2] = settingsView;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, CellBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
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
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 31)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			radioCell3.Title = obj11;
			radioCell3.Tapped += this.DashboardThemeItem_Tapped;
			radioCell3.SetValue(RadioCell.ValueProperty, num3);
			section.Add(radioCell3);
			settingsView.Root.Add(section);
			translate6.Text = "ios_ApplyDashboardThemeDescription";
			IMarkupExtension markupExtension7 = translate6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = section2;
			array7[1] = settingsView;
			array7[2] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
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
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 25)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			section2.Title = obj13;
			translate7.Text = "ios_ApplyDashboardTheme";
			IMarkupExtension markupExtension8 = translate7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = buttonCell;
			array8[1] = section2;
			array8[2] = settingsView;
			array8[3] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, CellBase.TitleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 21)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			buttonCell.Title = obj15;
			buttonCell.Tapped += this.btnApplyDashboardTheme_Clicked;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = buttonCell;
			array9[1] = section2;
			array9[2] = settingsView;
			array9[3] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array9, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 21)));
			DynamicResource dynamicResource2 = markupExtension9.ProvideValue(xamlServiceProvider9);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource2.Key);
			section2.Add(buttonCell);
			settingsView.Root.Add(section2);
			translate8.Text = "settings_Appearance";
			IMarkupExtension markupExtension10 = translate8;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = section3;
			array10[1] = settingsView;
			array10[2] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 25)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			section3.Title = obj18;
			translate9.Text = "ios_DashboardRearrangeOnRotation";
			IMarkupExtension markupExtension11 = translate9;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = settingsCheckBoxCellPatched;
			array11[1] = section3;
			array11[2] = settingsView;
			array11[3] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, CellBase.TitleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 49)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			settingsCheckBoxCellPatched.Title = obj20;
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "DashboardRearrangeOnRotation";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DashboardRearrangeOnRotation, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DashboardRearrangeOnRotation = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DashboardRearrangeOnRotation")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase2);
			section3.Add(settingsCheckBoxCellPatched);
			translate10.Text = "dashboard_HudMode";
			IMarkupExtension markupExtension12 = translate10;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = settingsCheckBoxCellPatched2;
			array12[1] = section3;
			array12[2] = settingsView;
			array12[3] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 49)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			settingsCheckBoxCellPatched2.Title = obj22;
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "DashboardHUDMode";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DashboardHUDMode, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DashboardHUDMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DashboardHUDMode")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase3);
			section3.Add(settingsCheckBoxCellPatched2);
			translate11.Text = "settings_DashboardHideTopControls";
			IMarkupExtension markupExtension13 = translate11;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = settingsCheckBoxCellPatched3;
			array13[1] = section3;
			array13[2] = settingsView;
			array13[3] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, CellBase.TitleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 49)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			settingsCheckBoxCellPatched3.Title = obj24;
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "DashboardHideTopControls";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DashboardHideTopControls, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DashboardHideTopControls = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DashboardHideTopControls")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(CheckboxCell.CheckedProperty, bindingBase4);
			section3.Add(settingsCheckBoxCellPatched3);
			translate12.Text = "DashboardCircularGaugeAnimation";
			IMarkupExtension markupExtension14 = translate12;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = settingsCheckBoxCellPatched4;
			array14[1] = section3;
			array14[2] = settingsView;
			array14[3] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array14, CellBase.TitleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 49)));
			object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
			settingsCheckBoxCellPatched4.Title = obj26;
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "DashboardCircularGaugeAnimation";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DashboardCircularGaugeAnimation, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DashboardCircularGaugeAnimation = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DashboardCircularGaugeAnimation")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			settingsCheckBoxCellPatched4.SetBinding(CheckboxCell.CheckedProperty, bindingBase5);
			section3.Add(settingsCheckBoxCellPatched4);
			translate13.Text = "ios_ChooseDashboardBackgroundImage";
			IMarkupExtension markupExtension15 = translate13;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = buttonCell2;
			array15[1] = section3;
			array15[2] = settingsView;
			array15[3] = this;
			object obj27;
			xamlServiceProvider15.Add(typeFromHandle29, obj27 = new SimpleValueTargetProvider(array15, CellBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 21)));
			object obj28 = markupExtension15.ProvideValue(xamlServiceProvider15);
			buttonCell2.Title = obj28;
			buttonCell2.Tapped += this.btnPickBackgroundImage_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = buttonCell2;
			array16[1] = section3;
			array16[2] = settingsView;
			array16[3] = this;
			object obj29;
			xamlServiceProvider16.Add(typeFromHandle31, obj29 = new SimpleValueTargetProvider(array16, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 21)));
			DynamicResource dynamicResource3 = markupExtension16.ProvideValue(xamlServiceProvider16);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section3.Add(buttonCell2);
			translate14.Text = "ios_ResetDashboardBackgroundImage";
			IMarkupExtension markupExtension17 = translate14;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = buttonCell3;
			array17[1] = section3;
			array17[2] = settingsView;
			array17[3] = this;
			object obj30;
			xamlServiceProvider17.Add(typeFromHandle33, obj30 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 21)));
			object obj31 = markupExtension17.ProvideValue(xamlServiceProvider17);
			buttonCell3.Title = obj31;
			buttonCell3.Tapped += this.btnResetImage_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = buttonCell3;
			array18[1] = section3;
			array18[2] = settingsView;
			array18[3] = this;
			object obj32;
			xamlServiceProvider18.Add(typeFromHandle35, obj32 = new SimpleValueTargetProvider(array18, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 21)));
			DynamicResource dynamicResource4 = markupExtension18.ProvideValue(xamlServiceProvider18);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section3.Add(buttonCell3);
			translate15.Text = "droid_Fullscreen";
			IMarkupExtension markupExtension19 = translate15;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = settingsCheckBoxCellPatched5;
			array19[1] = section3;
			array19[2] = settingsView;
			array19[3] = this;
			object obj33;
			xamlServiceProvider19.Add(typeFromHandle37, obj33 = new SimpleValueTargetProvider(array19, CellBase.TitleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 49)));
			object obj34 = markupExtension19.ProvideValue(xamlServiceProvider19);
			settingsCheckBoxCellPatched5.Title = obj34;
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "AndroidUseFullscreen";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidUseFullscreen, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidUseFullscreen = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidUseFullscreen")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			settingsCheckBoxCellPatched5.SetBinding(CheckboxCell.CheckedProperty, bindingBase6);
			onPlatform.Android = true;
			onPlatform.iOS = false;
			settingsCheckBoxCellPatched5.SetValue(CellBase.IsVisibleProperty, onPlatform);
			section3.Add(settingsCheckBoxCellPatched5);
			translate16.Text = "droid_Fullscreen";
			IMarkupExtension markupExtension20 = translate16;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = settingsCheckBoxCellPatched6;
			array20[1] = section3;
			array20[2] = settingsView;
			array20[3] = this;
			object obj35;
			xamlServiceProvider20.Add(typeFromHandle39, obj35 = new SimpleValueTargetProvider(array20, CellBase.TitleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 49)));
			object obj36 = markupExtension20.ProvideValue(xamlServiceProvider20);
			settingsCheckBoxCellPatched6.Title = obj36;
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "iOSDashboardUseFullscreen";
			bindingExtension7.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.iOSDashboardUseFullscreen, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.iOSDashboardUseFullscreen = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "iOSDashboardUseFullscreen")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			settingsCheckBoxCellPatched6.SetBinding(CheckboxCell.CheckedProperty, bindingBase7);
			onPlatform2.Android = false;
			onPlatform2.iOS = true;
			settingsCheckBoxCellPatched6.SetValue(CellBase.IsVisibleProperty, onPlatform2);
			section3.Add(settingsCheckBoxCellPatched6);
			translate17.Text = "settings_DashboardAnimation";
			IMarkupExtension markupExtension21 = translate17;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = settingsCheckBoxCellPatched7;
			array21[1] = section3;
			array21[2] = settingsView;
			array21[3] = this;
			object obj37;
			xamlServiceProvider21.Add(typeFromHandle41, obj37 = new SimpleValueTargetProvider(array21, CellBase.TitleProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 49)));
			object obj38 = markupExtension21.ProvideValue(xamlServiceProvider21);
			settingsCheckBoxCellPatched7.Title = obj38;
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "DashboardAnimation";
			bindingExtension8.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DashboardAnimation, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DashboardAnimation = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DashboardAnimation")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CheckboxCell.CheckedProperty, bindingBase8);
			section3.Add(settingsCheckBoxCellPatched7);
			translate18.Text = "Dashboard_AndroidRecolorStatusBarInDarkTheme";
			IMarkupExtension markupExtension22 = translate18;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = settingsCheckBoxCellPatched8;
			array22[1] = section3;
			array22[2] = settingsView;
			array22[3] = this;
			object obj39;
			xamlServiceProvider22.Add(typeFromHandle43, obj39 = new SimpleValueTargetProvider(array22, CellBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(107, 49)));
			object obj40 = markupExtension22.ProvideValue(xamlServiceProvider22);
			settingsCheckBoxCellPatched8.Title = obj40;
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "AndroidRecolorStatusBarInDarkTheme";
			bindingExtension9.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidRecolorStatusBarInDarkTheme, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidRecolorStatusBarInDarkTheme = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidRecolorStatusBarInDarkTheme")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			settingsCheckBoxCellPatched8.SetBinding(CheckboxCell.CheckedProperty, bindingBase9);
			onPlatform3.Android = true;
			onPlatform3.iOS = false;
			settingsCheckBoxCellPatched8.SetValue(CellBase.IsVisibleProperty, onPlatform3);
			section3.Add(settingsCheckBoxCellPatched8);
			translate19.Text = "droid_ChangeNavigationBarColor";
			IMarkupExtension markupExtension23 = translate19;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = settingsCheckBoxCellPatched9;
			array23[1] = section3;
			array23[2] = settingsView;
			array23[3] = this;
			object obj41;
			xamlServiceProvider23.Add(typeFromHandle45, obj41 = new SimpleValueTargetProvider(array23, CellBase.TitleProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 49)));
			object obj42 = markupExtension23.ProvideValue(xamlServiceProvider23);
			settingsCheckBoxCellPatched9.Title = obj42;
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "AndroidRecolorNavBar";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidRecolorNavBar, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidRecolorNavBar = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidRecolorNavBar")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			settingsCheckBoxCellPatched9.SetBinding(CheckboxCell.CheckedProperty, bindingBase10);
			onPlatform4.Android = true;
			onPlatform4.iOS = false;
			settingsCheckBoxCellPatched9.SetValue(CellBase.IsVisibleProperty, onPlatform4);
			section3.Add(settingsCheckBoxCellPatched9);
			settingsView.Root.Add(section3);
			translate20.Text = "ios_BackupTitle";
			IMarkupExtension markupExtension24 = translate20;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 3];
			array24[0] = section4;
			array24[1] = settingsView;
			array24[2] = this;
			object obj43;
			xamlServiceProvider24.Add(typeFromHandle47, obj43 = new SimpleValueTargetProvider(array24, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 25)));
			object obj44 = markupExtension24.ProvideValue(xamlServiceProvider24);
			section4.Title = obj44;
			translate21.Text = "dashboard_Backup";
			IMarkupExtension markupExtension25 = translate21;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = buttonCell4;
			array25[1] = section4;
			array25[2] = settingsView;
			array25[3] = this;
			object obj45;
			xamlServiceProvider25.Add(typeFromHandle49, obj45 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 21)));
			object obj46 = markupExtension25.ProvideValue(xamlServiceProvider25);
			buttonCell4.Title = obj46;
			buttonCell4.Tapped += this.btnBackup_Clicked;
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension26 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = buttonCell4;
			array26[1] = section4;
			array26[2] = settingsView;
			array26[3] = this;
			object obj47;
			xamlServiceProvider26.Add(typeFromHandle51, obj47 = new SimpleValueTargetProvider(array26, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 21)));
			DynamicResource dynamicResource5 = markupExtension26.ProvideValue(xamlServiceProvider26);
			buttonCell4.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource5.Key);
			section4.Add(buttonCell4);
			translate22.Text = "dashboard_Restore";
			IMarkupExtension markupExtension27 = translate22;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 4];
			array27[0] = buttonCell5;
			array27[1] = section4;
			array27[2] = settingsView;
			array27[3] = this;
			object obj48;
			xamlServiceProvider27.Add(typeFromHandle53, obj48 = new SimpleValueTargetProvider(array27, CellBase.TitleProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 21)));
			object obj49 = markupExtension27.ProvideValue(xamlServiceProvider27);
			buttonCell5.Title = obj49;
			buttonCell5.Tapped += this.btnRestore_Clicked;
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = buttonCell5;
			array28[1] = section4;
			array28[2] = settingsView;
			array28[3] = this;
			object obj50;
			xamlServiceProvider28.Add(typeFromHandle55, obj50 = new SimpleValueTargetProvider(array28, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(137, 21)));
			DynamicResource dynamicResource6 = markupExtension28.ProvideValue(xamlServiceProvider28);
			buttonCell5.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource6.Key);
			section4.Add(buttonCell5);
			settingsView.Root.Add(section4);
			translate23.Text = "Settings_Control_tbResetDashboardToDefault.Text";
			IMarkupExtension markupExtension29 = translate23;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 3];
			array29[0] = section5;
			array29[1] = settingsView;
			array29[2] = this;
			object obj51;
			xamlServiceProvider29.Add(typeFromHandle57, obj51 = new SimpleValueTargetProvider(array29, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 25)));
			object obj52 = markupExtension29.ProvideValue(xamlServiceProvider29);
			section5.Title = obj52;
			translate24.Text = "Settings_Control_btnDashReset.Content";
			IMarkupExtension markupExtension30 = translate24;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = buttonCell6;
			array30[1] = section5;
			array30[2] = settingsView;
			array30[3] = this;
			object obj53;
			xamlServiceProvider30.Add(typeFromHandle59, obj53 = new SimpleValueTargetProvider(array30, CellBase.TitleProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 21)));
			object obj54 = markupExtension30.ProvideValue(xamlServiceProvider30);
			buttonCell6.Title = obj54;
			buttonCell6.Tapped += this.btnResetDashboard_Clicked;
			dynamicResourceExtension7.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension31 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 4];
			array31[0] = buttonCell6;
			array31[1] = section5;
			array31[2] = settingsView;
			array31[3] = this;
			object obj55;
			xamlServiceProvider31.Add(typeFromHandle61, obj55 = new SimpleValueTargetProvider(array31, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsDashboardV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 21)));
			DynamicResource dynamicResource7 = markupExtension31.ProvideValue(xamlServiceProvider31);
			buttonCell6.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource7.Key);
			section5.Add(buttonCell6);
			settingsView.Root.Add(section5);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00171928 File Offset: 0x0016FB28
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsDashboardV3>(this, typeof(SettingsDashboardV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.gridDashPreview = NameScopeExtensions.FindByName<Grid>(this, "gridDashPreview");
			this.btnPickBackgroundImage = NameScopeExtensions.FindByName<ButtonCell>(this, "btnPickBackgroundImage");
			this.btnResetImage = NameScopeExtensions.FindByName<ButtonCell>(this, "btnResetImage");
			this.btnBackup = NameScopeExtensions.FindByName<ButtonCell>(this, "btnBackup");
			this.btnRestore = NameScopeExtensions.FindByName<ButtonCell>(this, "btnRestore");
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x001719AC File Offset: 0x0016FBAC
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1874(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.DashboardTheme, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x001719DC File Offset: 0x0016FBDC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1875(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.DashboardTheme = A_1;
				return;
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x001719F8 File Offset: 0x0016FBF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1876(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00171A08 File Offset: 0x0016FC08
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1877(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DashboardRearrangeOnRotation, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00171A38 File Offset: 0x0016FC38
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1878(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DashboardRearrangeOnRotation = A_1;
				return;
			}
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00171A54 File Offset: 0x0016FC54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1879(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00171A64 File Offset: 0x0016FC64
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1880(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DashboardHUDMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00171A94 File Offset: 0x0016FC94
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1881(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DashboardHUDMode = A_1;
				return;
			}
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x00171AB0 File Offset: 0x0016FCB0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1882(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x00171AC0 File Offset: 0x0016FCC0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1883(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DashboardHideTopControls, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00171AF0 File Offset: 0x0016FCF0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1884(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DashboardHideTopControls = A_1;
				return;
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00171B0C File Offset: 0x0016FD0C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1885(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00171B1C File Offset: 0x0016FD1C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1886(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DashboardCircularGaugeAnimation, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00171B4C File Offset: 0x0016FD4C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1887(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DashboardCircularGaugeAnimation = A_1;
				return;
			}
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00171B68 File Offset: 0x0016FD68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1888(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00171B78 File Offset: 0x0016FD78
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1889(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidUseFullscreen, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00171BA8 File Offset: 0x0016FDA8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1890(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidUseFullscreen = A_1;
				return;
			}
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00171BC4 File Offset: 0x0016FDC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1891(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00171BD4 File Offset: 0x0016FDD4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1892(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.iOSDashboardUseFullscreen, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00171C04 File Offset: 0x0016FE04
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1893(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.iOSDashboardUseFullscreen = A_1;
				return;
			}
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00171C20 File Offset: 0x0016FE20
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1894(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00171C30 File Offset: 0x0016FE30
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1895(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DashboardAnimation, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x00171C60 File Offset: 0x0016FE60
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1896(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DashboardAnimation = A_1;
				return;
			}
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00171C7C File Offset: 0x0016FE7C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1897(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x00171C8C File Offset: 0x0016FE8C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1898(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidRecolorStatusBarInDarkTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x00171CBC File Offset: 0x0016FEBC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1899(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidRecolorStatusBarInDarkTheme = A_1;
				return;
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x00171CD8 File Offset: 0x0016FED8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1900(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00171CE8 File Offset: 0x0016FEE8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1901(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidRecolorNavBar, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x00171D18 File Offset: 0x0016FF18
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1902(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidRecolorNavBar = A_1;
				return;
			}
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00171D34 File Offset: 0x0016FF34
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1903(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000F6B RID: 3947
		private DashboardItem dashItem;

		// Token: 0x04000F6C RID: 3948
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04000F6D RID: 3949
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridDashPreview;

		// Token: 0x04000F6E RID: 3950
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnPickBackgroundImage;

		// Token: 0x04000F6F RID: 3951
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnResetImage;

		// Token: 0x04000F70 RID: 3952
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnBackup;

		// Token: 0x04000F71 RID: 3953
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnRestore;

		// Token: 0x02000291 RID: 657
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002059 RID: 8281 RVA: 0x00171D42 File Offset: 0x0016FF42
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600205A RID: 8282 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600205B RID: 8283 RVA: 0x00171D4E File Offset: 0x0016FF4E
			internal bool <btnPickBackgroundImage_Clicked>b__9_0(DashboardPage page)
			{
				return page.Items.Any((DashboardItem item) => item.BackgroundColor != Color.Transparent);
			}

			// Token: 0x0600205C RID: 8284 RVA: 0x000EC106 File Offset: 0x000EA306
			internal bool <btnPickBackgroundImage_Clicked>b__9_1(DashboardItem item)
			{
				return item.BackgroundColor != Color.Transparent;
			}

			// Token: 0x04000F72 RID: 3954
			public static readonly SettingsDashboardV3.<>c <>9 = new SettingsDashboardV3.<>c();

			// Token: 0x04000F73 RID: 3955
			public static Func<DashboardItem, bool> <>9__9_1;

			// Token: 0x04000F74 RID: 3956
			public static Func<DashboardPage, bool> <>9__9_0;
		}

		// Token: 0x02000292 RID: 658
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x0600205D RID: 8285 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x0600205E RID: 8286 RVA: 0x00171D7C File Offset: 0x0016FF7C
			internal async Task <btnResetDashboard_Clicked>b__0()
			{
				ProfileV2Model profilesModel = new ProfileV2Model();
				await profilesModel.LoadProfilesAsync();
				this.profile = profilesModel.GetProfileForUpdate(SharedSettings.Current.ProfileUpdateAlias);
			}

			// Token: 0x04000F75 RID: 3957
			public OBDReaderProfileV2 profile;

			// Token: 0x02000293 RID: 659
			[StructLayout(LayoutKind.Auto)]
			private struct <<btnResetDashboard_Clicked>b__0>d : IAsyncStateMachine
			{
				// Token: 0x0600205F RID: 8287 RVA: 0x00171DC0 File Offset: 0x0016FFC0
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SettingsDashboardV3.<>c__DisplayClass12_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							profilesModel = new ProfileV2Model();
							taskAwaiter = profilesModel.LoadProfilesAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<>c__DisplayClass12_0.<<btnResetDashboard_Clicked>b__0>d>(ref taskAwaiter, ref this);
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
						CS$<>8__locals1.profile = profilesModel.GetProfileForUpdate(SharedSettings.Current.ProfileUpdateAlias);
					}
					catch (Exception ex)
					{
						num2 = -2;
						profilesModel = null;
						this.<>t__builder.SetException(ex);
						return;
					}
					num2 = -2;
					profilesModel = null;
					this.<>t__builder.SetResult();
				}

				// Token: 0x06002060 RID: 8288 RVA: 0x00171EAC File Offset: 0x001700AC
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000F76 RID: 3958
				public int <>1__state;

				// Token: 0x04000F77 RID: 3959
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000F78 RID: 3960
				public SettingsDashboardV3.<>c__DisplayClass12_0 <>4__this;

				// Token: 0x04000F79 RID: 3961
				private ProfileV2Model <profilesModel>5__2;

				// Token: 0x04000F7A RID: 3962
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x02000294 RID: 660
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06002061 RID: 8289 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06002062 RID: 8290 RVA: 0x00171EBC File Offset: 0x001700BC
			internal void <UpdatePreview>b__0()
			{
				try
				{
					this.<>4__this.dashItem = new DashboardItem();
					this.<>4__this.dashItem.HeightRequest = 150.0;
					this.<>4__this.dashItem.ItemType = DashboardItemTypes.Gauge;
					this.<>4__this.dashItem.ShowAvg = false;
					this.<>4__this.dashItem.Minimum = 0.0;
					this.<>4__this.dashItem.Maximum = 7000.0;
					this.<>4__this.dashItem.GaugeShowRedLine = true;
					this.<>4__this.dashItem.GaugeRedLineStart = 6000.0;
					this.<>4__this.dashItem.GaugeRedLineFinish = 7000.0;
					this.<>4__this.dashItem.HorizontalOptions = LayoutOptions.Center;
					this.<>4__this.dashItem.Model = new LiveDataPIDModel();
					this.<>4__this.dashItem.Model.SelectedPID = this.pid;
					this.<>4__this.dashItem.TitleFontSize = 0.0;
					this.<>4__this.dashItem.UnitsFontSize = 0.0;
					this.<>4__this.dashItem.SelectAndAddControl();
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
					{
						this.pid.SetValue(3000.0);
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x06002063 RID: 8291 RVA: 0x00172054 File Offset: 0x00170254
			internal void <UpdatePreview>b__1()
			{
				this.<>4__this.gridDashPreview.Children.Add(this.<>4__this.dashItem);
				DashboardItem.ApplyThemeToItem(this.<>4__this.dashItem);
				this.pid.SetValue(3000.0);
			}

			// Token: 0x04000F7B RID: 3963
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F7C RID: 3964
			public PIDWithFloatValueFormula pid;
		}

		// Token: 0x02000295 RID: 661
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DashboardThemeItem_Tapped>d__5 : IAsyncStateMachine
		{
			// Token: 0x06002064 RID: 8292 RVA: 0x001720A8 File Offset: 0x001702A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboardV3 settingsDashboardV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						settingsDashboardV.UpdatePreview();
						taskAwaiter = Task.Delay(300).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<DashboardThemeItem_Tapped>d__5>(ref taskAwaiter, ref this);
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
					settingsDashboardV.UpdatePreview();
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

			// Token: 0x06002065 RID: 8293 RVA: 0x0017216C File Offset: 0x0017036C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F7D RID: 3965
			public int <>1__state;

			// Token: 0x04000F7E RID: 3966
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F7F RID: 3967
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F80 RID: 3968
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000296 RID: 662
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DeleteDashboardBackgroundImage>d__11 : IAsyncStateMachine
		{
			// Token: 0x06002066 RID: 8294 RVA: 0x0017217C File Offset: 0x0017037C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboardV3 settingsDashboardV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						string localFilePath = FileSystemHelper.GetLocalFilePath("dashbg");
						int num3 = 0;
						try
						{
							if (File.Exists(localFilePath))
							{
								File.Delete(localFilePath);
								settingsDashboardV.btnResetImage.IsVisible = false;
							}
						}
						catch (Exception obj)
						{
							num3 = 1;
						}
						if (num3 != 1)
						{
							goto IL_00C8;
						}
						object obj;
						Exception ex = (Exception)obj;
						taskAwaiter = settingsDashboardV.DisplayAlert("Error", (ex != null) ? ex.ToString() : "", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<DeleteDashboardBackgroundImage>d__11>(ref taskAwaiter, ref this);
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
					IL_00C8:;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002067 RID: 8295 RVA: 0x0017229C File Offset: 0x0017049C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F81 RID: 3969
			public int <>1__state;

			// Token: 0x04000F82 RID: 3970
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000F83 RID: 3971
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F84 RID: 3972
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000297 RID: 663
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdatePreview>d__6 : IAsyncStateMachine
		{
			// Token: 0x06002068 RID: 8296 RVA: 0x001722AC File Offset: 0x001704AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboardV3 settingsDashboardV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new SettingsDashboardV3.<>c__DisplayClass6_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.pid = PIDWithFloatValueFormula.PID010C_EngineRPM();
						settingsDashboardV.gridDashPreview.Children.Clear();
						taskAwaiter = Task.Run(delegate
						{
							try
							{
								CS$<>8__locals1.<>4__this.dashItem = new DashboardItem();
								CS$<>8__locals1.<>4__this.dashItem.HeightRequest = 150.0;
								CS$<>8__locals1.<>4__this.dashItem.ItemType = DashboardItemTypes.Gauge;
								CS$<>8__locals1.<>4__this.dashItem.ShowAvg = false;
								CS$<>8__locals1.<>4__this.dashItem.Minimum = 0.0;
								CS$<>8__locals1.<>4__this.dashItem.Maximum = 7000.0;
								CS$<>8__locals1.<>4__this.dashItem.GaugeShowRedLine = true;
								CS$<>8__locals1.<>4__this.dashItem.GaugeRedLineStart = 6000.0;
								CS$<>8__locals1.<>4__this.dashItem.GaugeRedLineFinish = 7000.0;
								CS$<>8__locals1.<>4__this.dashItem.HorizontalOptions = LayoutOptions.Center;
								CS$<>8__locals1.<>4__this.dashItem.Model = new LiveDataPIDModel();
								CS$<>8__locals1.<>4__this.dashItem.Model.SelectedPID = CS$<>8__locals1.pid;
								CS$<>8__locals1.<>4__this.dashItem.TitleFontSize = 0.0;
								CS$<>8__locals1.<>4__this.dashItem.UnitsFontSize = 0.0;
								CS$<>8__locals1.<>4__this.dashItem.SelectAndAddControl();
								if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
								{
									CS$<>8__locals1.pid.SetValue(3000.0);
								}
							}
							catch (Exception)
							{
							}
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<UpdatePreview>d__6>(ref taskAwaiter, ref this);
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
					MainThreadHelper.InvokeOnMainThread(delegate
					{
						CS$<>8__locals1.<>4__this.gridDashPreview.Children.Add(CS$<>8__locals1.<>4__this.dashItem);
						DashboardItem.ApplyThemeToItem(CS$<>8__locals1.<>4__this.dashItem);
						CS$<>8__locals1.pid.SetValue(3000.0);
					});
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

			// Token: 0x06002069 RID: 8297 RVA: 0x001723D4 File Offset: 0x001705D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F85 RID: 3973
			public int <>1__state;

			// Token: 0x04000F86 RID: 3974
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000F87 RID: 3975
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F88 RID: 3976
			private SettingsDashboardV3.<>c__DisplayClass6_0 <>8__1;

			// Token: 0x04000F89 RID: 3977
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000298 RID: 664
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApplyDashboardTheme_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600206A RID: 8298 RVA: 0x001723E4 File Offset: 0x001705E4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboardV3 settingsDashboardV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						DashboardListViewModel.Current.LoadDashboardFromSettings();
						IEnumerator<DashboardPage> enumerator = DashboardListViewModel.Current.Pages.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DashboardPage dashboardPage = enumerator.Current;
								IEnumerator<DashboardItem> enumerator2 = dashboardPage.Items.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										DashboardItem dashboardItem = enumerator2.Current;
										try
										{
											DashboardItem.ApplyThemeToItem(dashboardItem);
											dashboardItem.SelectAndAddControl();
										}
										catch (Exception ex)
										{
											ObjectDisposedException ex2 = ex as ObjectDisposedException;
										}
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
						DashboardListViewModel.Current.SaveDashboardToSettings();
						taskAwaiter = settingsDashboardV.DisplayAlert(Translate.GetString("ios_DashboardThemeApplyed"), "", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnApplyDashboardTheme_Clicked>d__4>(ref taskAwaiter, ref this);
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
				}
				catch (Exception ex3)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex3);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600206B RID: 8299 RVA: 0x00172590 File Offset: 0x00170790
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F8A RID: 3978
			public int <>1__state;

			// Token: 0x04000F8B RID: 3979
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F8C RID: 3980
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F8D RID: 3981
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000299 RID: 665
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBackup_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x0600206C RID: 8300 RVA: 0x001725A0 File Offset: 0x001707A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboardV3 settingsDashboardV = this;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					string localFilePath;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0166;
						}
						if (string.IsNullOrEmpty(SharedSettings.Current.Dashboard))
						{
							goto IL_0103;
						}
						localFilePath = FileSystemHelper.GetLocalFilePath("dashboard.json");
						FileStream fileStream = File.Open(localFilePath, FileMode.Create);
						try
						{
							StreamWriter streamWriter = new StreamWriter(fileStream);
							try
							{
								streamWriter.Write(SharedSettings.Current.Dashboard);
								streamWriter.Flush();
							}
							finally
							{
								if (num < 0 && streamWriter != null)
								{
									((IDisposable)streamWriter).Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && fileStream != null)
							{
								((IDisposable)fileStream).Dispose();
							}
						}
					}
					try
					{
						if (num != 0)
						{
							taskAwaiter = Share.RequestAsync(new ShareFileRequest
							{
								Title = "Dashboard backup",
								File = new ShareFile(localFilePath)
							}).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnBackup_Clicked>d__7>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
						goto IL_016D;
					}
					catch (Exception)
					{
						goto IL_016D;
					}
					IL_0103:
					taskAwaiter = settingsDashboardV.DisplayAlert("Dashboard is empty!", "", "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnBackup_Clicked>d__7>(ref taskAwaiter, ref this);
						return;
					}
					IL_0166:
					taskAwaiter.GetResult();
					IL_016D:;
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

			// Token: 0x0600206D RID: 8301 RVA: 0x001727AC File Offset: 0x001709AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F8E RID: 3982
			public int <>1__state;

			// Token: 0x04000F8F RID: 3983
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F90 RID: 3984
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F91 RID: 3985
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200029A RID: 666
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPickBackgroundImage_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x0600206E RID: 8302 RVA: 0x001727BC File Offset: 0x001709BC
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SettingsDashboardV3 settingsDashboardV = this;
				try
				{
					TaskAwaiter taskAwaiter4;
					TaskAwaiter taskAwaiter3;
					if (num2 > 3)
					{
						if (num2 == 4)
						{
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = (num3 = -1);
							goto IL_03D1;
						}
						num = 0;
					}
					try
					{
						TaskAwaiter<Stream> taskAwaiter5;
						if (num2 != 0)
						{
							if (num2 - 1 <= 2)
							{
								goto IL_0092;
							}
							taskAwaiter5 = DependencyService.Get<IPhotoPickerService>(0).GetImageStreamAsync().GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = (num3 = 0);
								TaskAwaiter<Stream> taskAwaiter6 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, SettingsDashboardV3.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter5, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<Stream> taskAwaiter6;
							taskAwaiter5 = taskAwaiter6;
							taskAwaiter6 = default(TaskAwaiter<Stream>);
							num2 = (num3 = -1);
						}
						Stream result = taskAwaiter5.GetResult();
						img_stream = result;
						IL_0092:
						try
						{
							TaskAwaiter<bool> taskAwaiter7;
							switch (num2)
							{
							case 1:
								break;
							case 2:
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num2 = (num3 = -1);
								goto IL_01B6;
							case 3:
								taskAwaiter7 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num2 = (num3 = -1);
								goto IL_0285;
							default:
							{
								if (img_stream == null)
								{
									goto IL_030E;
								}
								string localFilePath = FileSystemHelper.GetLocalFilePath("dashbg");
								fstream = File.Create(localFilePath);
								break;
							}
							}
							try
							{
								if (num2 != 1)
								{
									taskAwaiter3 = img_stream.CopyToAsync(fstream).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = (num3 = 1);
										taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter3, ref this);
										return;
									}
								}
								else
								{
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter);
									num2 = (num3 = -1);
								}
								taskAwaiter3.GetResult();
							}
							finally
							{
								if (num2 < 0 && fstream != null)
								{
									((IDisposable)fstream).Dispose();
								}
							}
							fstream = null;
							taskAwaiter3 = ImageService.Instance.InvalidateCacheAsync(2).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = (num3 = 2);
								taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter3, ref this);
								return;
							}
							IL_01B6:
							taskAwaiter3.GetResult();
							settingsDashboardV.btnResetImage.IsVisible = true;
							DashboardListViewModel.Current.LoadDashboardFromSettings();
							if (!DashboardListViewModel.Current.Pages.Any((DashboardPage page) => page.Items.Any((DashboardItem item) => item.BackgroundColor != Color.Transparent)))
							{
								goto IL_030E;
							}
							taskAwaiter7 = settingsDashboardV.DisplayAlert(Translate.GetString("ios_DashSetItemsToTransparentTitle"), Translate.GetString("ios_DashSetItemsToTransparentText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter7.IsCompleted)
							{
								num2 = (num3 = 3);
								taskAwaiter2 = taskAwaiter7;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsDashboardV3.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter7, ref this);
								return;
							}
							IL_0285:
							if (taskAwaiter7.GetResult())
							{
								IEnumerator<DashboardPage> enumerator = DashboardListViewModel.Current.Pages.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										DashboardPage dashboardPage = enumerator.Current;
										IEnumerator<DashboardItem> enumerator2 = dashboardPage.Items.GetEnumerator();
										try
										{
											while (enumerator2.MoveNext())
											{
												DashboardItem dashboardItem = enumerator2.Current;
												dashboardItem.BackgroundColor = Color.Transparent;
												dashboardItem.ShowDefaultBackground = false;
											}
										}
										finally
										{
											if (num2 < 0 && enumerator2 != null)
											{
												enumerator2.Dispose();
											}
										}
									}
								}
								finally
								{
									if (num2 < 0 && enumerator != null)
									{
										enumerator.Dispose();
									}
								}
							}
							DashboardListViewModel.Current.SaveDashboardToSettings();
							IL_030E:;
						}
						finally
						{
							if (num2 < 0 && img_stream != null)
							{
								((IDisposable)img_stream).Dispose();
							}
						}
						img_stream = null;
					}
					catch (Exception ex)
					{
						obj = ex;
						num = 1;
					}
					int num4 = num;
					if (num4 != 1)
					{
						goto IL_03D8;
					}
					Exception ex2 = (Exception)obj;
					taskAwaiter3 = settingsDashboardV.DisplayAlert("Error!", (ex2 != null) ? ex2.ToString() : "", "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = (num3 = 4);
						taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter3, ref this);
						return;
					}
					IL_03D1:
					taskAwaiter3.GetResult();
					IL_03D8:
					obj = null;
				}
				catch (Exception ex3)
				{
					num3 = -2;
					this.<>t__builder.SetException(ex3);
					return;
				}
				num3 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600206F RID: 8303 RVA: 0x00172C6C File Offset: 0x00170E6C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F92 RID: 3986
			public int <>1__state;

			// Token: 0x04000F93 RID: 3987
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F94 RID: 3988
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F95 RID: 3989
			private object <>7__wrap1;

			// Token: 0x04000F96 RID: 3990
			private int <>7__wrap2;

			// Token: 0x04000F97 RID: 3991
			private Stream <img_stream>5__4;

			// Token: 0x04000F98 RID: 3992
			private TaskAwaiter<Stream> <>u__1;

			// Token: 0x04000F99 RID: 3993
			private FileStream <fstream>5__5;

			// Token: 0x04000F9A RID: 3994
			private TaskAwaiter <>u__2;

			// Token: 0x04000F9B RID: 3995
			private TaskAwaiter<bool> <>u__3;
		}

		// Token: 0x0200029B RID: 667
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetDashboard_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x06002070 RID: 8304 RVA: 0x00172C7C File Offset: 0x00170E7C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboardV3 settingsDashboardV = this;
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
						goto IL_0122;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0199;
					}
					default:
						taskAwaiter3 = settingsDashboardV.DisplayAlert(Translate.GetString("ios_ResetDashboard_Title"), Translate.GetString("ios_ResetDashboard_Text"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsDashboardV3.<btnResetDashboard_Clicked>d__12>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_01D6;
					}
					CS$<>8__locals1 = new SettingsDashboardV3.<>c__DisplayClass12_0();
					(sender as ButtonCell).IsEnabled = false;
					DashboardListViewModel.Current.ClearDashboard();
					taskAwaiter4 = settingsDashboardV.DeleteDashboardBackgroundImage().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnResetDashboard_Clicked>d__12>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0122:
					taskAwaiter4.GetResult();
					CS$<>8__locals1.profile = null;
					taskAwaiter4 = Task.Run(delegate
					{
						SettingsDashboardV3.<>c__DisplayClass12_0.<<btnResetDashboard_Clicked>b__0>d <<btnResetDashboard_Clicked>b__0>d;
						<<btnResetDashboard_Clicked>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<btnResetDashboard_Clicked>b__0>d.<>4__this = CS$<>8__locals1;
						<<btnResetDashboard_Clicked>b__0>d.<>1__state = -1;
						<<btnResetDashboard_Clicked>b__0>d.<>t__builder.Start<SettingsDashboardV3.<>c__DisplayClass12_0.<<btnResetDashboard_Clicked>b__0>d>(ref <<btnResetDashboard_Clicked>b__0>d);
						return <<btnResetDashboard_Clicked>b__0>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnResetDashboard_Clicked>d__12>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0199:
					taskAwaiter4.GetResult();
					if (CS$<>8__locals1.profile != null)
					{
						CS$<>8__locals1.profile.ApplyTweaks(true);
					}
					(sender as ButtonCell).IsEnabled = true;
					CS$<>8__locals1 = null;
					IL_01D6:;
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

			// Token: 0x06002071 RID: 8305 RVA: 0x00172EAC File Offset: 0x001710AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F9C RID: 3996
			public int <>1__state;

			// Token: 0x04000F9D RID: 3997
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F9E RID: 3998
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000F9F RID: 3999
			public object sender;

			// Token: 0x04000FA0 RID: 4000
			private SettingsDashboardV3.<>c__DisplayClass12_0 <>8__1;

			// Token: 0x04000FA1 RID: 4001
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000FA2 RID: 4002
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200029C RID: 668
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetImage_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x06002072 RID: 8306 RVA: 0x00172EBC File Offset: 0x001710BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboardV3 settingsDashboardV = this;
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
							goto IL_00F3;
						}
						taskAwaiter5 = settingsDashboardV.DisplayAlert(Translate.GetString("ios_ResetDashboardBackgroundImage") + "?", "", Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsDashboardV3.<btnResetImage_Clicked>d__10>(ref taskAwaiter5, ref this);
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
						goto IL_00FA;
					}
					taskAwaiter3 = settingsDashboardV.DeleteDashboardBackgroundImage().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnResetImage_Clicked>d__10>(ref taskAwaiter3, ref this);
						return;
					}
					IL_00F3:
					taskAwaiter3.GetResult();
					IL_00FA:;
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

			// Token: 0x06002073 RID: 8307 RVA: 0x00173004 File Offset: 0x00171204
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FA3 RID: 4003
			public int <>1__state;

			// Token: 0x04000FA4 RID: 4004
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FA5 RID: 4005
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000FA6 RID: 4006
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000FA7 RID: 4007
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200029D RID: 669
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestore_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x06002074 RID: 8308 RVA: 0x00173014 File Offset: 0x00171214
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SettingsDashboardV3 settingsDashboardV = this;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					if (num2 > 1)
					{
						if (num2 == 2)
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
							goto IL_021A;
						}
						num = 0;
					}
					int num4;
					try
					{
						TaskAwaiter<FileResult> taskAwaiter3;
						if (num2 != 0)
						{
							if (num2 == 1)
							{
								goto IL_00B9;
							}
							taskAwaiter3 = FilePicker.PickAsync(null).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = (num3 = 0);
								TaskAwaiter<FileResult> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, SettingsDashboardV3.<btnRestore_Clicked>d__8>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<FileResult> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<FileResult>);
							num2 = (num3 = -1);
						}
						FileResult result = taskAwaiter3.GetResult();
						if (result == null || !result.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
						{
							goto IL_0189;
						}
						sr = new StreamReader(result.FullPath, Encoding.UTF8);
						IL_00B9:
						try
						{
							if (num2 != 1)
							{
								string text = sr.ReadToEnd();
								num4 = 0;
								try
								{
									Json.DeserializeObject<List<ProxyPage>>(text);
									SharedSettings.Current.Dashboard = text;
								}
								catch (Exception obj3)
								{
									num4 = 1;
								}
								if (num4 != 1)
								{
									goto IL_0168;
								}
								object obj3;
								Exception ex = (Exception)obj3;
								taskAwaiter = settingsDashboardV.DisplayAlert("Error!", "Wrong JSON Dashboard format!", "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = (num3 = 1);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnRestore_Clicked>d__8>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num2 = (num3 = -1);
							}
							taskAwaiter.GetResult();
							IL_0168:;
						}
						finally
						{
							if (num2 < 0 && sr != null)
							{
								((IDisposable)sr).Dispose();
							}
						}
						sr = null;
						IL_0189:;
					}
					catch (Exception ex2)
					{
						obj2 = ex2;
						num = 1;
					}
					num4 = num;
					if (num4 != 1)
					{
						goto IL_0221;
					}
					Exception ex3 = (Exception)obj2;
					taskAwaiter = settingsDashboardV.DisplayAlert("Error!", "", "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = (num3 = 2);
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboardV3.<btnRestore_Clicked>d__8>(ref taskAwaiter, ref this);
						return;
					}
					IL_021A:
					taskAwaiter.GetResult();
					IL_0221:
					obj2 = null;
				}
				catch (Exception ex4)
				{
					num3 = -2;
					this.<>t__builder.SetException(ex4);
					return;
				}
				num3 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002075 RID: 8309 RVA: 0x001732DC File Offset: 0x001714DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FA8 RID: 4008
			public int <>1__state;

			// Token: 0x04000FA9 RID: 4009
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FAA RID: 4010
			public SettingsDashboardV3 <>4__this;

			// Token: 0x04000FAB RID: 4011
			private object <>7__wrap1;

			// Token: 0x04000FAC RID: 4012
			private int <>7__wrap2;

			// Token: 0x04000FAD RID: 4013
			private TaskAwaiter<FileResult> <>u__1;

			// Token: 0x04000FAE RID: 4014
			private StreamReader <sr>5__4;

			// Token: 0x04000FAF RID: 4015
			private TaskAwaiter <>u__2;
		}
	}
}
