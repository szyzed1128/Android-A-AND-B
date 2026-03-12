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
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.Settings;
using FFImageLoading;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020001D1 RID: 465
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml")]
	public class SettingsDashboard : ContentPage
	{
		// Token: 0x060018FF RID: 6399 RVA: 0x000E9724 File Offset: 0x000E7924
		public SettingsDashboard()
		{
			this.InitializeComponent();
			try
			{
				switch (SharedSettings.Current.DashboardTheme)
				{
				case 0:
					this.btnDark.IsChecked = new bool?(true);
					this.btnLight.IsChecked = new bool?(false);
					this.btnCarScanner.IsChecked = new bool?(false);
					break;
				case 1:
					this.btnDark.IsChecked = new bool?(false);
					this.btnLight.IsChecked = new bool?(true);
					this.btnCarScanner.IsChecked = new bool?(false);
					break;
				case 2:
					this.btnDark.IsChecked = new bool?(false);
					this.btnLight.IsChecked = new bool?(false);
					this.btnCarScanner.IsChecked = new bool?(true);
					break;
				}
			}
			catch (Exception)
			{
			}
			try
			{
				if (File.Exists(FileSystemHelper.GetLocalFilePath("dashbg")))
				{
					this.btnResetImage.IsVisible = true;
				}
				else
				{
					this.btnResetImage.IsVisible = false;
				}
			}
			catch (Exception)
			{
			}
			base.BindingContext = SharedSettings.Current;
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x000E985C File Offset: 0x000E7A5C
		private async void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x000E988B File Offset: 0x000E7A8B
		private void DarkThemeSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			base.ApplyBindings();
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x000E9894 File Offset: 0x000E7A94
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
				SettingsDashboard.<>c__DisplayClass4_0 CS$<>8__locals1 = new SettingsDashboard.<>c__DisplayClass4_0();
				Button button = sender as Button;
				if (button != null)
				{
					button.IsEnabled = false;
				}
				DashboardListViewModel.Current.ClearDashboard();
				await this.DeleteDashboardBackgroundImage();
				CS$<>8__locals1.profile = null;
				await Task.Run(delegate
				{
					SettingsDashboard.<>c__DisplayClass4_0.<<btnResetDashboard_Clicked>b__0>d <<btnResetDashboard_Clicked>b__0>d;
					<<btnResetDashboard_Clicked>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<btnResetDashboard_Clicked>b__0>d.<>4__this = CS$<>8__locals1;
					<<btnResetDashboard_Clicked>b__0>d.<>1__state = -1;
					<<btnResetDashboard_Clicked>b__0>d.<>t__builder.Start<SettingsDashboard.<>c__DisplayClass4_0.<<btnResetDashboard_Clicked>b__0>d>(ref <<btnResetDashboard_Clicked>b__0>d);
					return <<btnResetDashboard_Clicked>b__0>d.<>t__builder.Task;
				});
				if (CS$<>8__locals1.profile != null)
				{
					CS$<>8__locals1.profile.ApplyTweaks(true);
				}
				Button button2 = sender as Button;
				if (button2 != null)
				{
					button2.IsEnabled = true;
				}
				CS$<>8__locals1 = null;
			}
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x000E98D4 File Offset: 0x000E7AD4
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
			base.DisplayAlert(Translate.GetString("ios_DashboardThemeApplyed"), "", "OK");
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x000E990C File Offset: 0x000E7B0C
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

		// Token: 0x06001906 RID: 6406 RVA: 0x000E9944 File Offset: 0x000E7B44
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

		// Token: 0x06001907 RID: 6407 RVA: 0x000E997C File Offset: 0x000E7B7C
		private void RbTheme_StateChanged(object sender, StateChangedEventArgs e)
		{
			if (this.btnDark.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.DashboardTheme = 0;
				return;
			}
			if (this.btnLight.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.DashboardTheme = 1;
				return;
			}
			if (this.btnCarScanner.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.DashboardTheme = 2;
			}
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x000E99EC File Offset: 0x000E7BEC
		private async void btnPickBackgroundImage_Clicked(object sender, EventArgs e)
		{
			(sender as Button).IsEnabled = false;
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
			(sender as Button).IsEnabled = true;
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x000E9A2C File Offset: 0x000E7C2C
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

		// Token: 0x0600190A RID: 6410 RVA: 0x000E9A64 File Offset: 0x000E7C64
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

		// Token: 0x0600190B RID: 6411 RVA: 0x000E9AA8 File Offset: 0x000E7CA8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsDashboard).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsDashboard.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 22);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 36);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 92);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 26);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 36);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 106);
			OnPlatform<bool> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 36);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 92);
			OnPlatform<bool> onPlatform4;
			VisualDiagnostics.RegisterSourceInfo(onPlatform4 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 26);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 24);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 22);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 25);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 25);
			SfRadioButton sfRadioButton3;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton3 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 22);
			SfRadioGroup sfRadioGroup;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup = new SfRadioGroup(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 24);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 18);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 66);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 18);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 36);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 100);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 18);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 36);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 88);
			LabelSwitch labelSwitch5;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch5 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 18);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 18);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 36);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 103);
			LabelSwitch labelSwitch6;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch6 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 18);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 24);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 18);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 61);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 18);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 24);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 18);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 21);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 21);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsDashboard.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("dashThemeGroup", sfRadioGroup);
			if (sfRadioGroup.StyleId == null)
			{
				sfRadioGroup.StyleId = "dashThemeGroup";
			}
			nameScope.RegisterName("btnDark", sfRadioButton);
			if (sfRadioButton.StyleId == null)
			{
				sfRadioButton.StyleId = "btnDark";
			}
			nameScope.RegisterName("btnLight", sfRadioButton2);
			if (sfRadioButton2.StyleId == null)
			{
				sfRadioButton2.StyleId = "btnLight";
			}
			nameScope.RegisterName("btnCarScanner", sfRadioButton3);
			if (sfRadioButton3.StyleId == null)
			{
				sfRadioButton3.StyleId = "btnCarScanner";
			}
			nameScope.RegisterName("btnPickBackgroundImage", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnPickBackgroundImage";
			}
			nameScope.RegisterName("btnResetImage", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnResetImage";
			}
			nameScope.RegisterName("btnBackup", button5);
			if (button5.StyleId == null)
			{
				button5.StyleId = "btnBackup";
			}
			nameScope.RegisterName("btnRestore", button6);
			if (button6.StyleId == null)
			{
				button6.StyleId = "btnRestore";
			}
			this.dashThemeGroup = sfRadioGroup;
			this.btnDark = sfRadioButton;
			this.btnLight = sfRadioButton2;
			this.btnCarScanner = sfRadioButton3;
			this.btnPickBackgroundImage = button2;
			this.btnResetImage = button3;
			this.btnBackup = button5;
			this.btnRestore = button6;
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			stackLayout.SetValue(View.MarginProperty, onPlatform);
			bindingExtension.Mode = 1;
			bindingExtension.Path = "AndroidUseFullscreen";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase);
			translate2.Text = "droid_Fullscreen";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = labelSwitch;
			array3[1] = stackLayout;
			array3[2] = scrollView;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(27, 92)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			labelSwitch.Text = obj5;
			onPlatform2.Android = true;
			onPlatform2.iOS = false;
			labelSwitch.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			stackLayout.Children.Add(labelSwitch);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "AndroidRecolorStatusBarInDarkTheme";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase2);
			translate3.Text = "Dashboard_AndroidRecolorStatusBarInDarkTheme";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = labelSwitch2;
			array4[1] = stackLayout;
			array4[2] = scrollView;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 106)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			labelSwitch2.Text = obj7;
			onPlatform3.Android = true;
			onPlatform3.iOS = false;
			labelSwitch2.SetValue(VisualElement.IsVisibleProperty, onPlatform3);
			stackLayout.Children.Add(labelSwitch2);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "AndroidRecolorNavBar";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase3);
			translate4.Text = "droid_ChangeNavigationBarColor";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = labelSwitch3;
			array5[1] = stackLayout;
			array5[2] = scrollView;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 92)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			labelSwitch3.Text = obj9;
			onPlatform4.Android = true;
			onPlatform4.iOS = false;
			labelSwitch3.SetValue(VisualElement.IsVisibleProperty, onPlatform4);
			stackLayout.Children.Add(labelSwitch3);
			translate5.Text = "ios_DashboardChooseTheme";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label;
			array6[1] = stackLayout;
			array6[2] = scrollView;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 24)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.Text = obj11;
			stackLayout.Children.Add(label);
			sfRadioGroup.SetValue(StackLayout.OrientationProperty, 0);
			sfRadioButton.StateChanged += this.RbTheme_StateChanged;
			translate6.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension7 = translate6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = sfRadioButton;
			array7[1] = sfRadioGroup;
			array7[2] = stackLayout;
			array7[3] = scrollView;
			array7[4] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 25)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			sfRadioButton.Text = obj13;
			sfRadioGroup.Children.Add(sfRadioButton);
			sfRadioButton2.StateChanged += this.RbTheme_StateChanged;
			translate7.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension8 = translate7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = sfRadioButton2;
			array8[1] = sfRadioGroup;
			array8[2] = stackLayout;
			array8[3] = scrollView;
			array8[4] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 25)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			sfRadioButton2.Text = obj15;
			sfRadioGroup.Children.Add(sfRadioButton2);
			sfRadioButton3.StateChanged += this.RbTheme_StateChanged;
			translate8.Text = "ios_DashboardThemeCarScanner";
			IMarkupExtension markupExtension9 = translate8;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = sfRadioButton3;
			array9[1] = sfRadioGroup;
			array9[2] = stackLayout;
			array9[3] = scrollView;
			array9[4] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array9, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 25)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			sfRadioButton3.Text = obj17;
			sfRadioGroup.Children.Add(sfRadioButton3);
			stackLayout.Children.Add(sfRadioGroup);
			translate9.Text = "ios_ApplyDashboardThemeDescription";
			IMarkupExtension markupExtension10 = translate9;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = label2;
			array10[1] = stackLayout;
			array10[2] = scrollView;
			array10[3] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle19, obj18 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 24)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label2.Text = obj19;
			stackLayout.Children.Add(label2);
			button.Clicked += this.btnApplyDashboardTheme_Clicked;
			translate10.Text = "ios_ApplyDashboardTheme";
			IMarkupExtension markupExtension11 = translate10;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = button;
			array11[1] = stackLayout;
			array11[2] = scrollView;
			array11[3] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle21, obj20 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 66)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button.Text = obj21;
			stackLayout.Children.Add(button);
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "DashboardRearrangeOnRotation";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase4);
			translate11.Text = "ios_DashboardRearrangeOnRotation";
			IMarkupExtension markupExtension12 = translate11;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = labelSwitch4;
			array12[1] = stackLayout;
			array12[2] = scrollView;
			array12[3] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle23, obj22 = new SimpleValueTargetProvider(array12, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 100)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			labelSwitch4.Text = obj23;
			stackLayout.Children.Add(labelSwitch4);
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "DashboardHUDMode";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			labelSwitch5.SetBinding(LabelSwitch.IsToggledProperty, bindingBase5);
			translate12.Text = "dashboard_HudMode";
			IMarkupExtension markupExtension13 = translate12;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = labelSwitch5;
			array13[1] = stackLayout;
			array13[2] = scrollView;
			array13[3] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle25, obj24 = new SimpleValueTargetProvider(array13, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 88)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			labelSwitch5.Text = obj25;
			stackLayout.Children.Add(labelSwitch5);
			button2.Clicked += this.btnPickBackgroundImage_Clicked;
			translate13.Text = "ios_ChooseDashboardBackgroundImage";
			IMarkupExtension markupExtension14 = translate13;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = button2;
			array14[1] = stackLayout;
			array14[2] = scrollView;
			array14[3] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle27, obj26 = new SimpleValueTargetProvider(array14, Button.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 21)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			button2.Text = obj27;
			stackLayout.Children.Add(button2);
			button3.Clicked += this.btnResetImage_Clicked;
			translate14.Text = "ios_ResetDashboardBackgroundImage";
			IMarkupExtension markupExtension15 = translate14;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = button3;
			array15[1] = stackLayout;
			array15[2] = scrollView;
			array15[3] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle29, obj28 = new SimpleValueTargetProvider(array15, Button.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 21)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			button3.Text = obj29;
			stackLayout.Children.Add(button3);
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "DashboardCircularGaugeAnimation";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			labelSwitch6.SetBinding(LabelSwitch.IsToggledProperty, bindingBase6);
			translate15.Text = "DashboardCircularGaugeAnimation";
			IMarkupExtension markupExtension16 = translate15;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = labelSwitch6;
			array16[1] = stackLayout;
			array16[2] = scrollView;
			array16[3] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle31, obj30 = new SimpleValueTargetProvider(array16, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 103)));
			object obj31 = markupExtension16.ProvideValue(xamlServiceProvider16);
			labelSwitch6.Text = obj31;
			stackLayout.Children.Add(labelSwitch6);
			translate16.Text = "Settings_Control_tbResetDashboardToDefault.Text";
			IMarkupExtension markupExtension17 = translate16;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = label3;
			array17[1] = stackLayout;
			array17[2] = scrollView;
			array17[3] = this;
			object obj32;
			xamlServiceProvider17.Add(typeFromHandle33, obj32 = new SimpleValueTargetProvider(array17, Label.TextProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 24)));
			object obj33 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label3.Text = obj33;
			stackLayout.Children.Add(label3);
			button4.Clicked += this.btnResetDashboard_Clicked;
			translate17.Text = "Settings_Control_btnDashReset.Content";
			IMarkupExtension markupExtension18 = translate17;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = button4;
			array18[1] = stackLayout;
			array18[2] = scrollView;
			array18[3] = this;
			object obj34;
			xamlServiceProvider18.Add(typeFromHandle35, obj34 = new SimpleValueTargetProvider(array18, Button.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 61)));
			object obj35 = markupExtension18.ProvideValue(xamlServiceProvider18);
			button4.Text = obj35;
			stackLayout.Children.Add(button4);
			translate18.Text = "dashboard_BackupRestoreHint";
			IMarkupExtension markupExtension19 = translate18;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = label4;
			array19[1] = stackLayout;
			array19[2] = scrollView;
			array19[3] = this;
			object obj36;
			xamlServiceProvider19.Add(typeFromHandle37, obj36 = new SimpleValueTargetProvider(array19, Label.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 24)));
			object obj37 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label4.Text = obj37;
			stackLayout.Children.Add(label4);
			button5.Clicked += this.btnBackup_Clicked;
			button5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate19.Text = "dashboard_Backup";
			IMarkupExtension markupExtension20 = translate19;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = button5;
			array20[1] = stackLayout;
			array20[2] = scrollView;
			array20[3] = this;
			object obj38;
			xamlServiceProvider20.Add(typeFromHandle39, obj38 = new SimpleValueTargetProvider(array20, Button.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 21)));
			object obj39 = markupExtension20.ProvideValue(xamlServiceProvider20);
			button5.Text = obj39;
			stackLayout.Children.Add(button5);
			button6.Clicked += this.btnRestore_Clicked;
			button6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate20.Text = "dashboard_Restore";
			IMarkupExtension markupExtension21 = translate20;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = button6;
			array21[1] = stackLayout;
			array21[2] = scrollView;
			array21[3] = this;
			object obj40;
			xamlServiceProvider21.Add(typeFromHandle41, obj40 = new SimpleValueTargetProvider(array21, Button.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsDashboard).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 21)));
			object obj41 = markupExtension21.ProvideValue(xamlServiceProvider21);
			button6.Text = obj41;
			stackLayout.Children.Add(button6);
			scrollView.Content = stackLayout;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x000EC028 File Offset: 0x000EA228
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsDashboard>(this, typeof(SettingsDashboard));
			this.dashThemeGroup = NameScopeExtensions.FindByName<SfRadioGroup>(this, "dashThemeGroup");
			this.btnDark = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnDark");
			this.btnLight = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnLight");
			this.btnCarScanner = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnCarScanner");
			this.btnPickBackgroundImage = NameScopeExtensions.FindByName<Button>(this, "btnPickBackgroundImage");
			this.btnResetImage = NameScopeExtensions.FindByName<Button>(this, "btnResetImage");
			this.btnBackup = NameScopeExtensions.FindByName<Button>(this, "btnBackup");
			this.btnRestore = NameScopeExtensions.FindByName<Button>(this, "btnRestore");
		}

		// Token: 0x04000AB1 RID: 2737
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioGroup dashThemeGroup;

		// Token: 0x04000AB2 RID: 2738
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnDark;

		// Token: 0x04000AB3 RID: 2739
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnLight;

		// Token: 0x04000AB4 RID: 2740
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnCarScanner;

		// Token: 0x04000AB5 RID: 2741
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnPickBackgroundImage;

		// Token: 0x04000AB6 RID: 2742
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnResetImage;

		// Token: 0x04000AB7 RID: 2743
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnBackup;

		// Token: 0x04000AB8 RID: 2744
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestore;

		// Token: 0x020001D2 RID: 466
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600190D RID: 6413 RVA: 0x000EC0CE File Offset: 0x000EA2CE
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600190E RID: 6414 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600190F RID: 6415 RVA: 0x000EC0DA File Offset: 0x000EA2DA
			internal bool <btnPickBackgroundImage_Clicked>b__9_0(DashboardPage page)
			{
				return page.Items.Any((DashboardItem item) => item.BackgroundColor != Color.Transparent);
			}

			// Token: 0x06001910 RID: 6416 RVA: 0x000EC106 File Offset: 0x000EA306
			internal bool <btnPickBackgroundImage_Clicked>b__9_1(DashboardItem item)
			{
				return item.BackgroundColor != Color.Transparent;
			}

			// Token: 0x04000AB9 RID: 2745
			public static readonly SettingsDashboard.<>c <>9 = new SettingsDashboard.<>c();

			// Token: 0x04000ABA RID: 2746
			public static Func<DashboardItem, bool> <>9__9_1;

			// Token: 0x04000ABB RID: 2747
			public static Func<DashboardPage, bool> <>9__9_0;
		}

		// Token: 0x020001D3 RID: 467
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06001911 RID: 6417 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06001912 RID: 6418 RVA: 0x000EC118 File Offset: 0x000EA318
			internal async Task <btnResetDashboard_Clicked>b__0()
			{
				ProfileV2Model profilesModel = new ProfileV2Model();
				await profilesModel.LoadProfilesAsync();
				this.profile = profilesModel.GetProfileForUpdate(SharedSettings.Current.ProfileUpdateAlias);
			}

			// Token: 0x04000ABC RID: 2748
			public OBDReaderProfileV2 profile;

			// Token: 0x020001D4 RID: 468
			[StructLayout(LayoutKind.Auto)]
			private struct <<btnResetDashboard_Clicked>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06001913 RID: 6419 RVA: 0x000EC15C File Offset: 0x000EA35C
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SettingsDashboard.<>c__DisplayClass4_0 CS$<>8__locals1 = this;
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<>c__DisplayClass4_0.<<btnResetDashboard_Clicked>b__0>d>(ref taskAwaiter, ref this);
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

				// Token: 0x06001914 RID: 6420 RVA: 0x000EC248 File Offset: 0x000EA448
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000ABD RID: 2749
				public int <>1__state;

				// Token: 0x04000ABE RID: 2750
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000ABF RID: 2751
				public SettingsDashboard.<>c__DisplayClass4_0 <>4__this;

				// Token: 0x04000AC0 RID: 2752
				private ProfileV2Model <profilesModel>5__2;

				// Token: 0x04000AC1 RID: 2753
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x020001D5 RID: 469
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DeleteDashboardBackgroundImage>d__11 : IAsyncStateMachine
		{
			// Token: 0x06001915 RID: 6421 RVA: 0x000EC258 File Offset: 0x000EA458
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboard settingsDashboard = this;
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
								settingsDashboard.btnResetImage.IsVisible = false;
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
						taskAwaiter = settingsDashboard.DisplayAlert("Error", (ex != null) ? ex.ToString() : "", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<DeleteDashboardBackgroundImage>d__11>(ref taskAwaiter, ref this);
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

			// Token: 0x06001916 RID: 6422 RVA: 0x000EC378 File Offset: 0x000EA578
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AC2 RID: 2754
			public int <>1__state;

			// Token: 0x04000AC3 RID: 2755
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000AC4 RID: 2756
			public SettingsDashboard <>4__this;

			// Token: 0x04000AC5 RID: 2757
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001D6 RID: 470
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Page_Disappearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06001917 RID: 6423 RVA: 0x000EC388 File Offset: 0x000EA588
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
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

			// Token: 0x06001918 RID: 6424 RVA: 0x000EC3D4 File Offset: 0x000EA5D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AC6 RID: 2758
			public int <>1__state;

			// Token: 0x04000AC7 RID: 2759
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x020001D7 RID: 471
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApplyDashboardTheme_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001919 RID: 6425 RVA: 0x000EC3E4 File Offset: 0x000EA5E4
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				SettingsDashboard settingsDashboard = this;
				try
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
					settingsDashboard.DisplayAlert(Translate.GetString("ios_DashboardThemeApplyed"), "", "OK");
				}
				catch (Exception ex3)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex3);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600191A RID: 6426 RVA: 0x000EC508 File Offset: 0x000EA708
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AC8 RID: 2760
			public int <>1__state;

			// Token: 0x04000AC9 RID: 2761
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000ACA RID: 2762
			public SettingsDashboard <>4__this;
		}

		// Token: 0x020001D8 RID: 472
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBackup_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600191B RID: 6427 RVA: 0x000EC518 File Offset: 0x000EA718
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboard settingsDashboard = this;
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnBackup_Clicked>d__6>(ref taskAwaiter, ref this);
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
					taskAwaiter = settingsDashboard.DisplayAlert("Dashboard is empty!", "", "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnBackup_Clicked>d__6>(ref taskAwaiter, ref this);
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

			// Token: 0x0600191C RID: 6428 RVA: 0x000EC724 File Offset: 0x000EA924
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000ACB RID: 2763
			public int <>1__state;

			// Token: 0x04000ACC RID: 2764
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000ACD RID: 2765
			public SettingsDashboard <>4__this;

			// Token: 0x04000ACE RID: 2766
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001D9 RID: 473
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPickBackgroundImage_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x0600191D RID: 6429 RVA: 0x000EC734 File Offset: 0x000EA934
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SettingsDashboard settingsDashboard = this;
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
							goto IL_03E2;
						}
						(sender as Button).IsEnabled = false;
						num = 0;
					}
					try
					{
						TaskAwaiter<Stream> taskAwaiter5;
						if (num2 != 0)
						{
							if (num2 - 1 <= 2)
							{
								goto IL_00A3;
							}
							taskAwaiter5 = DependencyService.Get<IPhotoPickerService>(0).GetImageStreamAsync().GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = (num3 = 0);
								TaskAwaiter<Stream> taskAwaiter6 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, SettingsDashboard.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter5, ref this);
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
						IL_00A3:
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
								goto IL_01C7;
							case 3:
								taskAwaiter7 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num2 = (num3 = -1);
								goto IL_0296;
							default:
							{
								if (img_stream == null)
								{
									goto IL_031F;
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
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter3, ref this);
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter3, ref this);
								return;
							}
							IL_01C7:
							taskAwaiter3.GetResult();
							settingsDashboard.btnResetImage.IsVisible = true;
							DashboardListViewModel.Current.LoadDashboardFromSettings();
							if (!DashboardListViewModel.Current.Pages.Any((DashboardPage page) => page.Items.Any((DashboardItem item) => item.BackgroundColor != Color.Transparent)))
							{
								goto IL_031F;
							}
							taskAwaiter7 = settingsDashboard.DisplayAlert(Translate.GetString("ios_DashSetItemsToTransparentTitle"), Translate.GetString("ios_DashSetItemsToTransparentText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter7.IsCompleted)
							{
								num2 = (num3 = 3);
								taskAwaiter2 = taskAwaiter7;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsDashboard.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter7, ref this);
								return;
							}
							IL_0296:
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
							IL_031F:;
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
						goto IL_03E9;
					}
					Exception ex2 = (Exception)obj;
					taskAwaiter3 = settingsDashboard.DisplayAlert("Error!", (ex2 != null) ? ex2.ToString() : "", "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = (num3 = 4);
						taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnPickBackgroundImage_Clicked>d__9>(ref taskAwaiter3, ref this);
						return;
					}
					IL_03E2:
					taskAwaiter3.GetResult();
					IL_03E9:
					obj = null;
					(sender as Button).IsEnabled = true;
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

			// Token: 0x0600191E RID: 6430 RVA: 0x000ECC04 File Offset: 0x000EAE04
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000ACF RID: 2767
			public int <>1__state;

			// Token: 0x04000AD0 RID: 2768
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000AD1 RID: 2769
			public object sender;

			// Token: 0x04000AD2 RID: 2770
			public SettingsDashboard <>4__this;

			// Token: 0x04000AD3 RID: 2771
			private object <>7__wrap1;

			// Token: 0x04000AD4 RID: 2772
			private int <>7__wrap2;

			// Token: 0x04000AD5 RID: 2773
			private Stream <img_stream>5__4;

			// Token: 0x04000AD6 RID: 2774
			private TaskAwaiter<Stream> <>u__1;

			// Token: 0x04000AD7 RID: 2775
			private FileStream <fstream>5__5;

			// Token: 0x04000AD8 RID: 2776
			private TaskAwaiter <>u__2;

			// Token: 0x04000AD9 RID: 2777
			private TaskAwaiter<bool> <>u__3;
		}

		// Token: 0x020001DA RID: 474
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetDashboard_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600191F RID: 6431 RVA: 0x000ECC14 File Offset: 0x000EAE14
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboard settingsDashboard = this;
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
						goto IL_012A;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01A4;
					}
					default:
						taskAwaiter3 = settingsDashboard.DisplayAlert(Translate.GetString("ios_ResetDashboard_Title"), Translate.GetString("ios_ResetDashboard_Text"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsDashboard.<btnResetDashboard_Clicked>d__4>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_01E9;
					}
					CS$<>8__locals1 = new SettingsDashboard.<>c__DisplayClass4_0();
					Button button = sender as Button;
					if (button != null)
					{
						button.IsEnabled = false;
					}
					DashboardListViewModel.Current.ClearDashboard();
					taskAwaiter4 = settingsDashboard.DeleteDashboardBackgroundImage().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnResetDashboard_Clicked>d__4>(ref taskAwaiter4, ref this);
						return;
					}
					IL_012A:
					taskAwaiter4.GetResult();
					CS$<>8__locals1.profile = null;
					taskAwaiter4 = Task.Run(delegate
					{
						SettingsDashboard.<>c__DisplayClass4_0.<<btnResetDashboard_Clicked>b__0>d <<btnResetDashboard_Clicked>b__0>d;
						<<btnResetDashboard_Clicked>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<btnResetDashboard_Clicked>b__0>d.<>4__this = CS$<>8__locals1;
						<<btnResetDashboard_Clicked>b__0>d.<>1__state = -1;
						<<btnResetDashboard_Clicked>b__0>d.<>t__builder.Start<SettingsDashboard.<>c__DisplayClass4_0.<<btnResetDashboard_Clicked>b__0>d>(ref <<btnResetDashboard_Clicked>b__0>d);
						return <<btnResetDashboard_Clicked>b__0>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnResetDashboard_Clicked>d__4>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01A4:
					taskAwaiter4.GetResult();
					if (CS$<>8__locals1.profile != null)
					{
						CS$<>8__locals1.profile.ApplyTweaks(true);
					}
					Button button2 = sender as Button;
					if (button2 != null)
					{
						button2.IsEnabled = true;
					}
					CS$<>8__locals1 = null;
					IL_01E9:;
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

			// Token: 0x06001920 RID: 6432 RVA: 0x000ECE54 File Offset: 0x000EB054
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000ADA RID: 2778
			public int <>1__state;

			// Token: 0x04000ADB RID: 2779
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000ADC RID: 2780
			public SettingsDashboard <>4__this;

			// Token: 0x04000ADD RID: 2781
			public object sender;

			// Token: 0x04000ADE RID: 2782
			private SettingsDashboard.<>c__DisplayClass4_0 <>8__1;

			// Token: 0x04000ADF RID: 2783
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000AE0 RID: 2784
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020001DB RID: 475
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetImage_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x06001921 RID: 6433 RVA: 0x000ECE64 File Offset: 0x000EB064
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDashboard settingsDashboard = this;
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
						taskAwaiter5 = settingsDashboard.DisplayAlert(Translate.GetString("ios_ResetDashboardBackgroundImage") + "?", "", Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsDashboard.<btnResetImage_Clicked>d__10>(ref taskAwaiter5, ref this);
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
					taskAwaiter3 = settingsDashboard.DeleteDashboardBackgroundImage().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnResetImage_Clicked>d__10>(ref taskAwaiter3, ref this);
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

			// Token: 0x06001922 RID: 6434 RVA: 0x000ECFAC File Offset: 0x000EB1AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AE1 RID: 2785
			public int <>1__state;

			// Token: 0x04000AE2 RID: 2786
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000AE3 RID: 2787
			public SettingsDashboard <>4__this;

			// Token: 0x04000AE4 RID: 2788
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000AE5 RID: 2789
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020001DC RID: 476
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestore_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06001923 RID: 6435 RVA: 0x000ECFBC File Offset: 0x000EB1BC
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SettingsDashboard settingsDashboard = this;
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, SettingsDashboard.<btnRestore_Clicked>d__7>(ref taskAwaiter3, ref this);
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
								taskAwaiter = settingsDashboard.DisplayAlert("Error!", "Wrong JSON Dashboard format!", "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = (num3 = 1);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnRestore_Clicked>d__7>(ref taskAwaiter, ref this);
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
					taskAwaiter = settingsDashboard.DisplayAlert("Error!", "", "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = (num3 = 2);
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsDashboard.<btnRestore_Clicked>d__7>(ref taskAwaiter, ref this);
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

			// Token: 0x06001924 RID: 6436 RVA: 0x000ED284 File Offset: 0x000EB484
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AE6 RID: 2790
			public int <>1__state;

			// Token: 0x04000AE7 RID: 2791
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000AE8 RID: 2792
			public SettingsDashboard <>4__this;

			// Token: 0x04000AE9 RID: 2793
			private object <>7__wrap1;

			// Token: 0x04000AEA RID: 2794
			private int <>7__wrap2;

			// Token: 0x04000AEB RID: 2795
			private TaskAwaiter<FileResult> <>u__1;

			// Token: 0x04000AEC RID: 2796
			private StreamReader <sr>5__4;

			// Token: 0x04000AED RID: 2797
			private TaskAwaiter <>u__2;
		}
	}
}
