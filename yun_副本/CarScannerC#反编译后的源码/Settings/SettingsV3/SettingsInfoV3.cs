using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.UserControls;
using MR.Gestures;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x020002A0 RID: 672
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsInfoV3.xaml")]
	public class SettingsInfoV3 : ContentPage
	{
		// Token: 0x060020F5 RID: 8437 RVA: 0x00181DA0 File Offset: 0x0017FFA0
		public SettingsInfoV3()
		{
			this.InitializeComponent();
			int num = 2024;
			try
			{
				num = DateTimeNowHelper.NowSafe.Year;
			}
			catch (Exception)
			{
			}
			if (num < 2024)
			{
				num = 2024;
			}
			this.labelCopyright.Text = "Copyright (c) 0vZ 2016-" + num.ToString("0000");
			if (PlatformHelper.AppMarket == Markets.GooglePlay)
			{
				this.cellPublisher.IsVisible = true;
			}
			if (PlatformHelper.AppMarket == Markets.GooglePlay || PlatformHelper.AppMarket == Markets.AppStore)
			{
				if (SharedSettings.Current.AdsProductPurchased)
				{
					this.adsSettingsButton.IsVisible = false;
				}
				else
				{
					this.adsSettingsButton.IsVisible = true;
				}
			}
			if (PlatformHelper.AppMarket == Markets.GooglePlay)
			{
				this.btnDataDeletion.IsVisible = true;
			}
			base.BindingContext = SharedSettings.Current;
			base.Appearing += this.SettingsPage_Appearing;
			base.Disappearing += this.SettingsPage_Disappearing;
			this.labelProfilesVersion.ValueText = ProfileV2Model.GetCurrentVersion();
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				this.webLinkCell.Title = "Ru.CarScanner.Info";
			}
			base.Title = App.AppTitle;
			if (SharedSettings.Current.DeveloperMode)
			{
				base.Title += " :)";
			}
			switch (PlatformHelper.AppMarket)
			{
			case Markets.AppStore:
				this.labelBuild.Description = "/iOS";
				return;
			case Markets.GooglePlay:
				this.labelBuild.Description = "/GP";
				return;
			case Markets.HMS:
				this.labelBuild.Description = "/HMS";
				return;
			case Markets.Rustore:
				this.labelBuild.Description = "/RuStore";
				return;
			case Markets.RUS:
				this.labelBuild.Description = "/RUS";
				return;
			case Markets.Sideload:
				this.labelBuild.Description = "/SL";
				return;
			default:
				return;
			}
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00181F7C File Offset: 0x0018017C
		private void LabelProfilesVersion_Tapped(object sender, EventArgs e)
		{
			if (SharedSettings.Current.ShowExperimental)
			{
				this.profilesVersionTapCounter++;
				if (this.profilesVersionTapCounter > 5)
				{
					SharedSettings.Current.LastTimeDBUpdateChecked = 0L;
					SharedSettings.Current.LastTimeNewVersionChecked = 0L;
					SharedSettings.Current.LastTimePatchUpdateChecked = 0L;
				}
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00181FD0 File Offset: 0x001801D0
		private void SettingsPage_Disappearing(object sender, EventArgs e)
		{
			SharedSettings.Current.PropertyChanged -= this.Current_PropertyChanged;
			try
			{
				base.BindingContext = null;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00182010 File Offset: 0x00180210
		private void SetDisablePro()
		{
			if (SharedSettings.Current.ShowExperimental)
			{
				this.noProCounter++;
				if (this.noProCounter >= 30)
				{
					SharedSettings.Current.AdsProductPurchased = false;
					SharedSettings.Current.PurchaseOrderId = "";
					SharedSettings.Current.PurchaseProductId = "";
					SharedSettings.Current.PurchaseResponseCached = "";
					SharedSettings.Current.PurchaseToken = "";
					SharedSettings.Current.WhitelistDeviceActivated = false;
					SharedSettings.Current.WhitelistDeviceSN = "";
					base.Title += " :-(";
				}
			}
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x001820BC File Offset: 0x001802BC
		private void SettingsPage_Appearing(object sender, EventArgs e)
		{
			if (base.BindingContext == null)
			{
				base.BindingContext = SharedSettings.Current;
			}
			SharedSettings.Current.PropertyChanged -= this.Current_PropertyChanged;
			SharedSettings.Current.PropertyChanged += this.Current_PropertyChanged;
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00182108 File Offset: 0x00180308
		private void Current_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "AllowProfilesBackgroundUpdate")
			{
				this.labelProfilesVersion.ValueText = ProfileV2Model.GetCurrentVersion();
			}
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x0018212C File Offset: 0x0018032C
		private void Image_CellTapped(object sender, EventArgs e)
		{
			this.SetDevMode();
			this.SetDisablePro();
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x0018212C File Offset: 0x0018032C
		private void Image_Tapped(object sender, TapEventArgs e)
		{
			this.SetDevMode();
			this.SetDisablePro();
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x0018213A File Offset: 0x0018033A
		private void Image_DoubleTapped(object sender, TapEventArgs e)
		{
			if (PlatformHelper.IsAndroid && PlatformHelper.AppMarket == Markets.Sideload && !SharedSettings.Current.AdsProductPurchased)
			{
				this.panelKeyInput.IsVisible = true;
			}
			this.SetDevMode();
			this.SetDevMode();
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x00182170 File Offset: 0x00180370
		private void SetDevMode()
		{
			if (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.ShowExperimental && !SharedSettings.Current.DeveloperMode)
			{
				this.tapcounter++;
				if (this.tapcounter >= 10)
				{
					SharedSettings.Current.DeveloperMode = true;
					base.Title += " :)";
				}
			}
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x001821DC File Offset: 0x001803DC
		private void MrImage_LongPressing(object sender, LongPressEventArgs e)
		{
			if (SharedSettings.Current.ShowExperimental && e.Duration > 5000L && !SharedSettings.Current.DeveloperMode)
			{
				SharedSettings.Current.DeveloperMode = true;
				base.Title += " :)";
			}
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00182230 File Offset: 0x00180430
		private async void BtnKeyActivation_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid)
			{
				this.panelKeyInput.IsVisible = false;
				await PlatformHelper.DroidService.CustomKeyActivator_CheckKeyAsync(this.entryKey.Text, delegate
				{
					SharedSettings.Current.AdsProductPurchased = true;
					base.Navigation.PopToRootAsync();
				}, delegate
				{
				});
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00182267 File Offset: 0x00180467
		private void CarScannerInfo_Tapped(object sender, object arg)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info");
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00182274 File Offset: 0x00180474
		private void iOSTermsOfUse_Tapped(object sender, object arg)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info/ios-tos/");
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00182281 File Offset: 0x00180481
		private void PrivacyPolicy_Tapped(object sender, object arg)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info/privacy-policy/");
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0018228E File Offset: 0x0018048E
		private void icons8Button_Tapped(object sender, object arg)
		{
			Launcher.TryOpenAsync("http://www.icons8.com");
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x0018229C File Offset: 0x0018049C
		private async void adsSettingsButton_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid && PlatformHelper.AppMarket == Markets.GooglePlay)
			{
				DependencyService.Get<IUMPConsent>(0).ForceDisplayConsentForm();
			}
			if (PlatformHelper.IsiOS)
			{
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0))
				{
					PlatformHelper.CommonService.OpenPermissionsSettings();
				}
				DependencyService.Get<IUMPConsent>(0).ForceDisplayConsentForm();
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x001822CC File Offset: 0x001804CC
		private void Copyright_Tapped(object sender, object arg)
		{
			this.copyrightClicks++;
			if (this.copyrightClicks == 20)
			{
				string text = "Q29weXJpZ2h0IChjKSBTdmlzdHVub3YgU3RhbmlzbGF2IDIwMTYtMjAyNSwgZS1tYWlsOiBzdGFuaXNsYXYuc3Zpc3RAZ21haWwuY29t";
				this.labelCopyright.Text = Encoding.ASCII.GetString(Convert.FromBase64String(text));
			}
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00182314 File Offset: 0x00180514
		private async void btnProfilesUpdate_Tapped(object sender, EventArgs e)
		{
			ProfilesUpdatePage profilesUpdatePage = new ProfilesUpdatePage();
			await base.Navigation.PushAsync(profilesUpdatePage);
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x0018234C File Offset: 0x0018054C
		private async void btnDataDeletion_Tapped(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("settings_info_dataDeletionTitle"), (PlatformHelper.AppMarket == Markets.GooglePlay) ? Translate.GetString("gplay_info_dataDeletionText") : Translate.GetString("settings_info_dataDeletionText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				SettingsInfoV3.RecursiveDelete(FileSystemHelper.LocalStoragePath);
				SettingsInfoV3.RecursiveDelete(FileSystemHelper.LocalCachePath);
				if (PlatformHelper.IsAndroid)
				{
					PlatformHelper.CommonService.QuitApp();
				}
				if (PlatformHelper.IsiOS)
				{
					throw new UserRefusedEULAException("app restart");
				}
			}
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00182384 File Offset: 0x00180584
		private static void RecursiveDelete(string baseDir)
		{
			if (!Directory.Exists(baseDir))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(baseDir);
			for (int i = 0; i < directories.Length; i++)
			{
				SettingsInfoV3.RecursiveDelete(directories[i]);
			}
			try
			{
				Directory.Delete(baseDir, true);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x001823D4 File Offset: 0x001805D4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsInfoV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsInfoV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 29);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 33);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 33);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 30);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			string version;
			VisualDiagnostics.RegisterSourceInfo(version = App.Version, new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 21);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 21);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			string build;
			VisualDiagnostics.RegisterSourceInfo(build = App.Build, new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 21);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 18);
			LabelCell labelCell3;
			VisualDiagnostics.RegisterSourceInfo(labelCell3 = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 21);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 21);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 21);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 30);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 30);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 26);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 21);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 21);
			ButtonCell buttonCell4;
			VisualDiagnostics.RegisterSourceInfo(buttonCell4 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 21);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 21);
			ButtonCell buttonCell5;
			VisualDiagnostics.RegisterSourceInfo(buttonCell5 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 18);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 21);
			ButtonCell buttonCell6;
			VisualDiagnostics.RegisterSourceInfo(buttonCell6 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 21);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 49);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 109);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 18);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 22);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 18);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 22);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsInfoV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("mrImage", image);
			if (image.StyleId == null)
			{
				image.StyleId = "mrImage";
			}
			nameScope.RegisterName("tbTitle", label);
			if (label.StyleId == null)
			{
				label.StyleId = "tbTitle";
			}
			nameScope.RegisterName("panelKeyInput", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "panelKeyInput";
			}
			nameScope.RegisterName("entryKey", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryKey";
			}
			nameScope.RegisterName("btnKeyActivation", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnKeyActivation";
			}
			nameScope.RegisterName("labelVersion", labelCell);
			if (labelCell.StyleId == null)
			{
				labelCell.StyleId = "labelVersion";
			}
			nameScope.RegisterName("labelBuild", labelCell2);
			if (labelCell2.StyleId == null)
			{
				labelCell2.StyleId = "labelBuild";
			}
			nameScope.RegisterName("labelProfilesVersion", labelCell3);
			if (labelCell3.StyleId == null)
			{
				labelCell3.StyleId = "labelProfilesVersion";
			}
			nameScope.RegisterName("webLinkCell", buttonCell);
			if (buttonCell.StyleId == null)
			{
				buttonCell.StyleId = "webLinkCell";
			}
			nameScope.RegisterName("adsSettingsButton", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "adsSettingsButton";
			}
			nameScope.RegisterName("btnDataDeletion", buttonCell4);
			if (buttonCell4.StyleId == null)
			{
				buttonCell4.StyleId = "btnDataDeletion";
			}
			nameScope.RegisterName("btnPrivacyPolicy", buttonCell5);
			if (buttonCell5.StyleId == null)
			{
				buttonCell5.StyleId = "btnPrivacyPolicy";
			}
			nameScope.RegisterName("icons8Button", buttonCell6);
			if (buttonCell6.StyleId == null)
			{
				buttonCell6.StyleId = "icons8Button";
			}
			nameScope.RegisterName("cbAllowProfileUpdates", settingsCheckBoxCellPatched);
			if (settingsCheckBoxCellPatched.StyleId == null)
			{
				settingsCheckBoxCellPatched.StyleId = "cbAllowProfileUpdates";
			}
			nameScope.RegisterName("labelCopyright", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "labelCopyright";
			}
			nameScope.RegisterName("cellPublisher", customCell3);
			if (customCell3.StyleId == null)
			{
				customCell3.StyleId = "cellPublisher";
			}
			nameScope.RegisterName("labelPublisher", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "labelPublisher";
			}
			this.settingsLayoutRoot = settingsView;
			this.mrImage = image;
			this.tbTitle = label;
			this.panelKeyInput = grid;
			this.entryKey = entry;
			this.btnKeyActivation = button;
			this.labelVersion = labelCell;
			this.labelBuild = labelCell2;
			this.labelProfilesVersion = labelCell3;
			this.webLinkCell = buttonCell;
			this.adsSettingsButton = buttonCell2;
			this.btnDataDeletion = buttonCell4;
			this.btnPrivacyPolicy = buttonCell5;
			this.icons8Button = buttonCell6;
			this.cbAllowProfileUpdates = settingsCheckBoxCellPatched;
			this.labelCopyright = label2;
			this.cellPublisher = customCell3;
			this.labelPublisher = label3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "SettingsPage_itemHelpInfo.Content";
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
			xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
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
			xmlNamespaceResolver2.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			customCell.Tapped += this.Image_CellTapped;
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			image.SetValue(Image.AspectProperty, 0);
			image.DoubleTapped += this.Image_DoubleTapped;
			image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			image.LongPressed += this.MrImage_LongPressing;
			image.LongPressing += this.MrImage_LongPressing;
			dynamicResourceExtension2.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 6];
			array3[0] = image;
			array3[1] = stackLayout;
			array3[2] = customCell;
			array3[3] = section;
			array3[4] = settingsView;
			array3[5] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Image.SourceProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource2.Key);
			image.Tapped += this.Image_Tapped;
			image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout.Children.Add(image);
			staticResourceExtension.Key = "BaseFontSize++";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = label;
			array4[1] = stackLayout;
			array4[2] = customCell;
			array4[3] = section;
			array4[4] = settingsView;
			array4[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 29)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.FontSize = (double)obj6;
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(Label.LineBreakModeProperty, 0);
			stackLayout.Children.Add(label);
			grid.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			entry.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension3.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 7];
			array5[0] = entry;
			array5[1] = grid;
			array5[2] = stackLayout;
			array5[3] = customCell;
			array5[4] = section;
			array5[5] = settingsView;
			array5[6] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 33)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			entry.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 7];
			array6[0] = entry;
			array6[1] = grid;
			array6[2] = stackLayout;
			array6[3] = customCell;
			array6[4] = section;
			array6[5] = settingsView;
			array6[6] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Entry.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver6.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 33)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			entry.SetDynamicResource(Entry.TextColorProperty, dynamicResource4.Key);
			grid.Children.Add(entry);
			button.SetValue(Grid.ColumnProperty, 1);
			button.Clicked += this.BtnKeyActivation_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			grid.Children.Add(button);
			stackLayout.Children.Add(grid);
			customCell.SetValue(CustomCell.ContentProperty, stackLayout);
			section.Add(customCell);
			labelCell.SetValue(CellBase.TitleProperty, "Version:");
			bindingExtension.Source = version;
			bindingExtension.Path = ".";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			labelCell.SetBinding(LabelCell.ValueTextProperty, bindingBase);
			section.Add(labelCell);
			labelCell2.SetValue(CellBase.TitleProperty, "Build:");
			bindingExtension2.Source = build;
			bindingExtension2.Path = ".";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelCell2.SetBinding(LabelCell.ValueTextProperty, bindingBase2);
			section.Add(labelCell2);
			labelCell3.SetValue(CellBase.TitleProperty, "Profiles DB:");
			labelCell3.Tapped += this.LabelProfilesVersion_Tapped;
			section.Add(labelCell3);
			buttonCell.SetValue(CellBase.TitleProperty, "CarScanner.Info");
			buttonCell.Tapped += new EventHandler(this.CarScannerInfo_Tapped);
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = buttonCell;
			array7[1] = section;
			array7[2] = settingsView;
			array7[3] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver7.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 21)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource5.Key);
			section.Add(buttonCell);
			translate2.Text = "GDPR_ChangeAdsSettings";
			IMarkupExtension markupExtension8 = translate2;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = buttonCell2;
			array8[1] = section;
			array8[2] = settingsView;
			array8[3] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, CellBase.TitleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver8.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 21)));
			object obj11 = markupExtension8.ProvideValue(xamlServiceProvider8);
			buttonCell2.Title = obj11;
			buttonCell2.SetValue(CellBase.IsVisibleProperty, false);
			buttonCell2.Tapped += this.adsSettingsButton_Clicked;
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = buttonCell2;
			array9[1] = section;
			array9[2] = settingsView;
			array9[3] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver9.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 21)));
			DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource6.Key);
			section.Add(buttonCell2);
			translate3.Text = "ios_TermsOfUse";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = buttonCell3;
			array10[1] = section;
			array10[2] = settingsView;
			array10[3] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, CellBase.TitleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver10.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 21)));
			object obj14 = markupExtension10.ProvideValue(xamlServiceProvider10);
			buttonCell3.Title = obj14;
			buttonCell3.Tapped += new EventHandler(this.iOSTermsOfUse_Tapped);
			dynamicResourceExtension7.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = buttonCell3;
			array11[1] = section;
			array11[2] = settingsView;
			array11[3] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array11, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver11.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 21)));
			DynamicResource dynamicResource7 = markupExtension11.ProvideValue(xamlServiceProvider11);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource7.Key);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "True";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "False";
			onPlatform.Platforms.Add(on2);
			buttonCell3.SetValue(CellBase.IsVisibleProperty, onPlatform);
			section.Add(buttonCell3);
			translate4.Text = "settings_info_dataDeletionBtn";
			IMarkupExtension markupExtension12 = translate4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = buttonCell4;
			array12[1] = section;
			array12[2] = settingsView;
			array12[3] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver12.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 21)));
			object obj17 = markupExtension12.ProvideValue(xamlServiceProvider12);
			buttonCell4.Title = obj17;
			buttonCell4.SetValue(CellBase.IsVisibleProperty, false);
			buttonCell4.Tapped += this.btnDataDeletion_Tapped;
			dynamicResourceExtension8.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = buttonCell4;
			array13[1] = section;
			array13[2] = settingsView;
			array13[3] = this;
			object obj18;
			xamlServiceProvider13.Add(typeFromHandle25, obj18 = new SimpleValueTargetProvider(array13, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver13.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 21)));
			DynamicResource dynamicResource8 = markupExtension13.ProvideValue(xamlServiceProvider13);
			buttonCell4.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource8.Key);
			section.Add(buttonCell4);
			translate5.Text = "ios_PrivacyPolicy";
			IMarkupExtension markupExtension14 = translate5;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = buttonCell5;
			array14[1] = section;
			array14[2] = settingsView;
			array14[3] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array14, CellBase.TitleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver14.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 21)));
			object obj20 = markupExtension14.ProvideValue(xamlServiceProvider14);
			buttonCell5.Title = obj20;
			buttonCell5.Tapped += new EventHandler(this.PrivacyPolicy_Tapped);
			dynamicResourceExtension9.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = buttonCell5;
			array15[1] = section;
			array15[2] = settingsView;
			array15[3] = this;
			object obj21;
			xamlServiceProvider15.Add(typeFromHandle29, obj21 = new SimpleValueTargetProvider(array15, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver15.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 21)));
			DynamicResource dynamicResource9 = markupExtension15.ProvideValue(xamlServiceProvider15);
			buttonCell5.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource9.Key);
			section.Add(buttonCell5);
			buttonCell6.SetValue(CellBase.TitleProperty, "Icons from Icons8");
			buttonCell6.Tapped += new EventHandler(this.icons8Button_Tapped);
			dynamicResourceExtension10.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = buttonCell6;
			array16[1] = section;
			array16[2] = settingsView;
			array16[3] = this;
			object obj22;
			xamlServiceProvider16.Add(typeFromHandle31, obj22 = new SimpleValueTargetProvider(array16, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver16.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 21)));
			DynamicResource dynamicResource10 = markupExtension16.ProvideValue(xamlServiceProvider16);
			buttonCell6.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource10.Key);
			section.Add(buttonCell6);
			translate6.Text = "settings_AllowBackgroudUpdates";
			IMarkupExtension markupExtension17 = translate6;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = settingsCheckBoxCellPatched;
			array17[1] = section;
			array17[2] = settingsView;
			array17[3] = this;
			object obj23;
			xamlServiceProvider17.Add(typeFromHandle33, obj23 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver17.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 21)));
			object obj24 = markupExtension17.ProvideValue(xamlServiceProvider17);
			settingsCheckBoxCellPatched.Title = obj24;
			bindingExtension3.Path = "AllowProfilesBackgroundUpdate";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AllowProfilesBackgroundUpdate, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AllowProfilesBackgroundUpdate = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AllowProfilesBackgroundUpdate")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase3);
			section.Add(settingsCheckBoxCellPatched);
			translate7.Text = "ios_ShowExperimentalFeatures";
			IMarkupExtension markupExtension18 = translate7;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = settingsCheckBoxCellPatched2;
			array18[1] = section;
			array18[2] = settingsView;
			array18[3] = this;
			object obj25;
			xamlServiceProvider18.Add(typeFromHandle35, obj25 = new SimpleValueTargetProvider(array18, CellBase.TitleProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xmlNamespaceResolver18.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsInfoV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 49)));
			object obj26 = markupExtension18.ProvideValue(xamlServiceProvider18);
			settingsCheckBoxCellPatched2.Title = obj26;
			bindingExtension4.Path = "ShowExperimental";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowExperimental = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowExperimental")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase4);
			section.Add(settingsCheckBoxCellPatched2);
			customCell2.Tapped += new EventHandler(this.Copyright_Tapped);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label2.SetValue(Label.LineBreakModeProperty, 1);
			label2.SetValue(Label.TextProperty, "Copyright (c) 0vZ 2016-2025");
			customCell2.SetValue(CustomCell.ContentProperty, label2);
			section.Add(customCell2);
			customCell3.SetValue(CellBase.IsVisibleProperty, false);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label3.SetValue(Label.LineBreakModeProperty, 1);
			label3.SetValue(Label.TextProperty, "Publishing: LLC Car Scanner");
			customCell3.SetValue(CustomCell.ContentProperty, label3);
			section.Add(customCell3);
			settingsView.Root.Add(section);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00095328 File Offset: 0x00093528
		[CompilerGenerated]
		private void <BtnKeyActivation_Clicked>b__14_0()
		{
			SharedSettings.Current.AdsProductPurchased = true;
			base.Navigation.PopToRootAsync();
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00185170 File Offset: 0x00183370
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsInfoV3>(this, typeof(SettingsInfoV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.mrImage = NameScopeExtensions.FindByName<Image>(this, "mrImage");
			this.tbTitle = NameScopeExtensions.FindByName<Label>(this, "tbTitle");
			this.panelKeyInput = NameScopeExtensions.FindByName<Grid>(this, "panelKeyInput");
			this.entryKey = NameScopeExtensions.FindByName<Entry>(this, "entryKey");
			this.btnKeyActivation = NameScopeExtensions.FindByName<Button>(this, "btnKeyActivation");
			this.labelVersion = NameScopeExtensions.FindByName<LabelCell>(this, "labelVersion");
			this.labelBuild = NameScopeExtensions.FindByName<LabelCell>(this, "labelBuild");
			this.labelProfilesVersion = NameScopeExtensions.FindByName<LabelCell>(this, "labelProfilesVersion");
			this.webLinkCell = NameScopeExtensions.FindByName<ButtonCell>(this, "webLinkCell");
			this.adsSettingsButton = NameScopeExtensions.FindByName<ButtonCell>(this, "adsSettingsButton");
			this.btnDataDeletion = NameScopeExtensions.FindByName<ButtonCell>(this, "btnDataDeletion");
			this.btnPrivacyPolicy = NameScopeExtensions.FindByName<ButtonCell>(this, "btnPrivacyPolicy");
			this.icons8Button = NameScopeExtensions.FindByName<ButtonCell>(this, "icons8Button");
			this.cbAllowProfileUpdates = NameScopeExtensions.FindByName<SettingsCheckBoxCellPatched>(this, "cbAllowProfileUpdates");
			this.labelCopyright = NameScopeExtensions.FindByName<Label>(this, "labelCopyright");
			this.cellPublisher = NameScopeExtensions.FindByName<CustomCell>(this, "cellPublisher");
			this.labelPublisher = NameScopeExtensions.FindByName<Label>(this, "labelPublisher");
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x001852C0 File Offset: 0x001834C0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2022(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AllowProfilesBackgroundUpdate, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x001852F0 File Offset: 0x001834F0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2023(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AllowProfilesBackgroundUpdate = A_1;
				return;
			}
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0018530C File Offset: 0x0018350C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2024(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x0018531C File Offset: 0x0018351C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2025(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0018534C File Offset: 0x0018354C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2026(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00185368 File Offset: 0x00183568
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2027(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000FB6 RID: 4022
		private int profilesVersionTapCounter;

		// Token: 0x04000FB7 RID: 4023
		private int noProCounter;

		// Token: 0x04000FB8 RID: 4024
		private int tapcounter;

		// Token: 0x04000FB9 RID: 4025
		private int copyrightClicks;

		// Token: 0x04000FBA RID: 4026
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04000FBB RID: 4027
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image mrImage;

		// Token: 0x04000FBC RID: 4028
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbTitle;

		// Token: 0x04000FBD RID: 4029
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelKeyInput;

		// Token: 0x04000FBE RID: 4030
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryKey;

		// Token: 0x04000FBF RID: 4031
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnKeyActivation;

		// Token: 0x04000FC0 RID: 4032
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelCell labelVersion;

		// Token: 0x04000FC1 RID: 4033
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelCell labelBuild;

		// Token: 0x04000FC2 RID: 4034
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelCell labelProfilesVersion;

		// Token: 0x04000FC3 RID: 4035
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell webLinkCell;

		// Token: 0x04000FC4 RID: 4036
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell adsSettingsButton;

		// Token: 0x04000FC5 RID: 4037
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnDataDeletion;

		// Token: 0x04000FC6 RID: 4038
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnPrivacyPolicy;

		// Token: 0x04000FC7 RID: 4039
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell icons8Button;

		// Token: 0x04000FC8 RID: 4040
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsCheckBoxCellPatched cbAllowProfileUpdates;

		// Token: 0x04000FC9 RID: 4041
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelCopyright;

		// Token: 0x04000FCA RID: 4042
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CustomCell cellPublisher;

		// Token: 0x04000FCB RID: 4043
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelPublisher;

		// Token: 0x020002A1 RID: 673
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002113 RID: 8467 RVA: 0x00185376 File Offset: 0x00183576
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002114 RID: 8468 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002115 RID: 8469 RVA: 0x000027D4 File Offset: 0x000009D4
			internal void <BtnKeyActivation_Clicked>b__14_1()
			{
			}

			// Token: 0x04000FCC RID: 4044
			public static readonly SettingsInfoV3.<>c <>9 = new SettingsInfoV3.<>c();

			// Token: 0x04000FCD RID: 4045
			public static Action <>9__14_1;
		}

		// Token: 0x020002A2 RID: 674
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnKeyActivation_Clicked>d__14 : IAsyncStateMachine
		{
			// Token: 0x06002116 RID: 8470 RVA: 0x00185384 File Offset: 0x00183584
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsInfoV3 settingsInfoV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_00BC;
						}
						settingsInfoV.panelKeyInput.IsVisible = false;
						taskAwaiter = PlatformHelper.DroidService.CustomKeyActivator_CheckKeyAsync(settingsInfoV.entryKey.Text, delegate
						{
							SharedSettings.Current.AdsProductPurchased = true;
							base.Navigation.PopToRootAsync();
						}, delegate
						{
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsInfoV3.<BtnKeyActivation_Clicked>d__14>(ref taskAwaiter, ref this);
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
					IL_00BC:;
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

			// Token: 0x06002117 RID: 8471 RVA: 0x0018548C File Offset: 0x0018368C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FCE RID: 4046
			public int <>1__state;

			// Token: 0x04000FCF RID: 4047
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FD0 RID: 4048
			public SettingsInfoV3 <>4__this;

			// Token: 0x04000FD1 RID: 4049
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002A3 RID: 675
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <adsSettingsButton_Clicked>d__19 : IAsyncStateMachine
		{
			// Token: 0x06002118 RID: 8472 RVA: 0x0018549C File Offset: 0x0018369C
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					if (PlatformHelper.IsAndroid && PlatformHelper.AppMarket == Markets.GooglePlay)
					{
						DependencyService.Get<IUMPConsent>(0).ForceDisplayConsentForm();
					}
					if (PlatformHelper.IsiOS)
					{
						if (PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0))
						{
							PlatformHelper.CommonService.OpenPermissionsSettings();
						}
						DependencyService.Get<IUMPConsent>(0).ForceDisplayConsentForm();
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

			// Token: 0x06002119 RID: 8473 RVA: 0x00185528 File Offset: 0x00183728
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FD2 RID: 4050
			public int <>1__state;

			// Token: 0x04000FD3 RID: 4051
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x020002A4 RID: 676
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDataDeletion_Tapped>d__23 : IAsyncStateMachine
		{
			// Token: 0x0600211A RID: 8474 RVA: 0x00185538 File Offset: 0x00183738
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsInfoV3 settingsInfoV = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsInfoV.DisplayAlert(Translate.GetString("settings_info_dataDeletionTitle"), (PlatformHelper.AppMarket == Markets.GooglePlay) ? Translate.GetString("gplay_info_dataDeletionText") : Translate.GetString("settings_info_dataDeletionText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsInfoV3.<btnDataDeletion_Tapped>d__23>(ref taskAwaiter3, ref this);
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
						SettingsInfoV3.RecursiveDelete(FileSystemHelper.LocalStoragePath);
						SettingsInfoV3.RecursiveDelete(FileSystemHelper.LocalCachePath);
						if (PlatformHelper.IsAndroid)
						{
							PlatformHelper.CommonService.QuitApp();
						}
						if (PlatformHelper.IsiOS)
						{
							throw new UserRefusedEULAException("app restart");
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

			// Token: 0x0600211B RID: 8475 RVA: 0x00185660 File Offset: 0x00183860
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FD4 RID: 4052
			public int <>1__state;

			// Token: 0x04000FD5 RID: 4053
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FD6 RID: 4054
			public SettingsInfoV3 <>4__this;

			// Token: 0x04000FD7 RID: 4055
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020002A5 RID: 677
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnProfilesUpdate_Tapped>d__22 : IAsyncStateMachine
		{
			// Token: 0x0600211C RID: 8476 RVA: 0x00185670 File Offset: 0x00183870
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsInfoV3 settingsInfoV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						ProfilesUpdatePage profilesUpdatePage = new ProfilesUpdatePage();
						taskAwaiter = settingsInfoV.Navigation.PushAsync(profilesUpdatePage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsInfoV3.<btnProfilesUpdate_Tapped>d__22>(ref taskAwaiter, ref this);
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

			// Token: 0x0600211D RID: 8477 RVA: 0x00185730 File Offset: 0x00183930
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FD8 RID: 4056
			public int <>1__state;

			// Token: 0x04000FD9 RID: 4057
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FDA RID: 4058
			public SettingsInfoV3 <>4__this;

			// Token: 0x04000FDB RID: 4059
			private TaskAwaiter <>u__1;
		}
	}
}
