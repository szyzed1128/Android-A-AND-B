using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.Styles;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.Licensing;
using Syncfusion.SfChart.XForms;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000045 RID: 69
	[XamlFilePath("App.xaml")]
	public class App : Application
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000AE9C File Offset: 0x0000909C
		public static string AppTitle
		{
			get
			{
				if (SharedSettings.Current.AdsProductPurchased)
				{
					if (SharedSettings.Current.WhitelistDeviceActivated)
					{
						return "Car Scanner // " + SharedSettings.Current.BTLEDeviceName;
					}
					if (PlatformHelper.AppMarket == Markets.RUS)
					{
						return "Car Scanner Rus Pro";
					}
					return "Car Scanner Pro";
				}
				else
				{
					if (PlatformHelper.AppMarket == Markets.RUS)
					{
						return "Car Scanner Rus";
					}
					return "Car Scanner";
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000AEFD File Offset: 0x000090FD
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000AF05 File Offset: 0x00009105
		public RootNavigationPage RootPage
		{
			[CompilerGenerated]
			get
			{
				return this.<RootPage>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RootPage>k__BackingField = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000AF0E File Offset: 0x0000910E
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000AF15 File Offset: 0x00009115
		public static bool UseLegacyUI
		{
			[CompilerGenerated]
			get
			{
				return App.<UseLegacyUI>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				App.<UseLegacyUI>k__BackingField = value;
			}
		} = false;

		// Token: 0x060001AE RID: 430 RVA: 0x0000AF1D File Offset: 0x0000911D
		private static void SetUseLegacyUI()
		{
			if (PlatformHelper.IsAndroid)
			{
				if (!PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0))
				{
					App.UseLegacyUI = true;
					return;
				}
			}
			else if (PlatformHelper.IsiOS && !PlatformHelper.IsPlatformVersionNewerOrEqual(11, 4))
			{
				App.UseLegacyUI = true;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000AF4E File Offset: 0x0000914E
		public static OBDReaderSimulator OBDSimulator
		{
			get
			{
				return OBDReaderSimulator.Current;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000AF55 File Offset: 0x00009155
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000AF5C File Offset: 0x0000915C
		public static App Instance
		{
			[CompilerGenerated]
			get
			{
				return App.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				App.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000AF64 File Offset: 0x00009164
		public App()
		{
			App.Instance = this;
			App.SetUseLegacyUI();
			SyncfusionLicenseProvider.RegisterLicense("MTIzMzU3N0AzMjMwMmUzNDJlMzBkc05JakJobEJQK2NyTitGMkd0ZzdJRGNucXVTUHJCTFNHLzl3ZFJ3b0xNPQ==");
			if (PlatformHelper.IsiOS)
			{
				DependencyService.Get<IStatusBar>(0).ShowStatusBar();
			}
			Language.SetOrigCulture();
			Language.SetLanguage();
			this.InitializeComponent();
			foreach (string text in base.Resources.Keys)
			{
				if (text != null && text.StartsWith("BaseFontSize"))
				{
					App.DefaultFontSizes[text] = (double)base.Resources[text];
				}
			}
			App.ApplyFontSizePatch();
			if (PlatformHelper.IsiOS)
			{
				((Style)base.Resources["Syncfusion.XForms.Buttons.SfRadioButton"]).Setters.Add(new Setter
				{
					Property = ToggleButton.LineBreakModeProperty,
					Value = 1
				});
			}
			if (PlatformHelper.IsAndroid && PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
			{
				try
				{
					App.OriginalNavBarColor = PlatformHelper.DroidService.Window_NavigationBarColor;
				}
				catch (Exception)
				{
					App.OriginalNavBarColor = -16777216;
				}
			}
			App.SetTheme();
			this.RootPage = new RootNavigationPage();
			if (SharedSettings.Current.FirstTimeLaunch)
			{
				if (PlatformHelper.AppMarket == Markets.RUS)
				{
					this.ChangeLanguageAndGoToWelcomePage();
				}
				else
				{
					SharedSettings.Current.FirstTimeLaunch = false;
					SharedSettings.Current.LatestVersion = App.Version;
					if (PlatformHelper.IsAndroid && !PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0))
					{
						base.MainPage = new NavigationPage(new LanguageSelectorPage());
						return;
					}
					if (PlatformHelper.IsiOS && !PlatformHelper.IsPlatformVersionNewerOrEqual(11, 4))
					{
						this.RootPage.RealRootPage = new LanguageSelectorPage();
						return;
					}
					this.RootPage.RealRootPage = new LanguageSelectorPage();
				}
			}
			else if (string.IsNullOrEmpty(SharedSettings.Current.LastException))
			{
				if (App.OBDReader == null)
				{
					App.OBDReader = new OBDDataReader();
				}
				this.RootPage.RealRootPage = new SimpleMainPage();
			}
			else
			{
				this.RootPage.RealRootPage = new DebugPage();
			}
			base.MainPage = new NavigationPage(this.RootPage);
			this.PreventLinkerFromStrippingCommonLocalizationReferences();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000B198 File Offset: 0x00009398
		internal static void ApplyFontSizePatch()
		{
			App app = Application.Current as App;
			int fontSizePatch = SharedSettings.Current.FontSizePatch;
			foreach (string text in App.DefaultFontSizes.Keys)
			{
				app.Resources[text] = App.DefaultFontSizes[text] + (double)fontSizePatch;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000B220 File Offset: 0x00009420
		public static void SetTheme()
		{
			if (SharedSettings.Current.AutomaticallySwitchThemeAvailable && SharedSettings.Current.AutomaticallySwitchTheme)
			{
				if (PlatformHelper.IsAndroid)
				{
					OSAppTheme requestedTheme = App.Instance.RequestedTheme;
					if (requestedTheme != 1 && requestedTheme == 2)
					{
						App.Instance.Resources.Add(new DarkTheme());
						SharedSettings.Current.DarkMode = true;
					}
					else
					{
						App.Instance.Resources.Add(new LightTheme());
						SharedSettings.Current.DarkMode = false;
					}
				}
				else if (PlatformHelper.IsiOS)
				{
					SharedSettings.Current.DarkMode = false;
				}
			}
			else if (SharedSettings.Current.DarkMode)
			{
				App.Instance.Resources.Add(new DarkTheme());
			}
			else
			{
				App.Instance.Resources.Add(new LightTheme());
			}
			if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidRecolorNavBar && PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
			{
				Color color = (Color)App.Instance.Resources["NavigationBarBackgroundColor"];
				try
				{
					PlatformHelper.DroidService.SetNavigationBarColor(color);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000B344 File Offset: 0x00009544
		public void ChangeLanguageAndGoToWelcomePage()
		{
			Language.SetLanguage();
			if (App.UseLegacyUI && PlatformHelper.IsiOS)
			{
				RootNavigationPage.ChangeRootPage(new WelcomePage1V3());
				return;
			}
			RootNavigationPage.ChangeRootPage(new WelcomePage1V4());
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000B36E File Offset: 0x0000956E
		public static string CurrentLanguageCode
		{
			get
			{
				return Language.CurrentLanguageCode;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B378 File Offset: 0x00009578
		public async void ChangeLanguage()
		{
			Language.SetLanguage();
			await Task.Run(delegate
			{
				if (App.OBDReader == null)
				{
					App.OBDReader = new OBDDataReader();
				}
				OBDReaderSimulator.Reload();
			});
			RootNavigationPage.ChangeRootPage(new SimpleMainPage());
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B3A8 File Offset: 0x000095A8
		protected override void OnStart()
		{
			if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
			{
				PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnStart:Start]\r\n");
			}
			this.CheckRP();
			if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
			{
				PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnStart:Finished]\r\n");
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000B43C File Offset: 0x0000963C
		protected override async void OnSleep()
		{
			if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
			{
				PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnSleep:Start]\r\n");
			}
			SharedSettings.Current.LastException = "";
			DriveCycle.SaveAndReset();
			App.SaveCurrentData();
			if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
			{
				PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnSleep:Finished]\r\n");
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B46C File Offset: 0x0000966C
		public static void SaveCurrentData()
		{
			try
			{
				OBDDataReader obdreader = App.OBDReader;
				if (obdreader != null)
				{
					CarData currentCarData = obdreader.CurrentCarData;
					if (currentCarData != null)
					{
						DataRecorderV2 recorder = currentCarData.Recorder;
						if (recorder != null)
						{
							recorder.Save();
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B4B4 File Offset: 0x000096B4
		protected override void OnResume()
		{
			if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
			{
				PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnResume:Start]\r\n");
			}
			DriveCycle.SaveAndReset();
			this.CheckRP();
			if (PlatformHelper.IsAndroid && SharedSettings.Current.AutomaticallySwitchThemeAvailable && SharedSettings.Current.AutomaticallySwitchTheme)
			{
				OSAppTheme requestedTheme = Application.Current.RequestedTheme;
				if (requestedTheme != 1)
				{
					if (requestedTheme == 2)
					{
						SharedSettings.Current.DarkMode = true;
					}
				}
				else
				{
					SharedSettings.Current.DarkMode = false;
				}
			}
			if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
			{
				PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnResume:Finished]\r\n");
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000B594 File Offset: 0x00009794
		public static string Version
		{
			get
			{
				return PlatformHelper.CommonService.AppVersion;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000B5A0 File Offset: 0x000097A0
		public static string Build
		{
			get
			{
				return PlatformHelper.CommonService.AppBuild;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B5AC File Offset: 0x000097AC
		public static Page GetCurrentPage()
		{
			if (App.Instance == null || App.Instance.RootPage == null)
			{
				return null;
			}
			if (App.Instance.RootPage.Navigation.ModalStack.Count > 0)
			{
				return App.Instance.RootPage.Navigation.ModalStack.Last<Page>();
			}
			if (App.Instance.RootPage.Navigation.NavigationStack.Count > 0)
			{
				return App.Instance.RootPage.Navigation.NavigationStack.Last<Page>();
			}
			return App.Instance.RootPage.RealRootPage;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B64C File Offset: 0x0000984C
		public static string ExceptionToString(Exception exception)
		{
			Console.WriteLine(exception);
			Console.WriteLine(exception.InnerException);
			if (exception.InnerException == null)
			{
				return string.Concat(new string[]
				{
					"Ver.:",
					App.Version,
					"/",
					App.Build,
					"\r\n ",
					exception.ToString(),
					"\r\n"
				});
			}
			return string.Concat(new string[]
			{
				"Ver.:",
				App.Version,
				"/",
				App.Build,
				"\r\n ",
				exception.ToString(),
				"\r\nInnerException:\r\n",
				App.ExceptionToString(exception.InnerException)
			});
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B70C File Offset: 0x0000990C
		private async void CheckRP()
		{
			if (PlatformHelper.IsAndroid)
			{
				this.CheckRP_DROID();
			}
			else if (PlatformHelper.IsiOS)
			{
				this.CheckRP_IOS();
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B744 File Offset: 0x00009944
		private async void CheckRP_DROID()
		{
			if (SharedSettings.Current.AdsProductPurchased || !SharedSettings.Current.DeveloperMode)
			{
				await Task.Delay(5000);
				if (PlatformHelper.AppMarket != Markets.Sideload)
				{
					if (PlatformHelper.AppMarket == Markets.RUS)
					{
						if (SharedSettings.Current.AdsProductPurchased || (!string.IsNullOrEmpty(SharedSettings.Current.PurchaseOrderId) && SharedSettings.Current.PurchaseOrderId.Length == 27))
						{
							if (SharedSettings.Current.PurchaseToken != PlatformHelper.DroidService.DevicePermanentID_DeviceID)
							{
								SharedSettings.Current.AdsProductPurchased = false;
								SharedSettings.Current.PurchaseOrderId = "";
								SharedSettings.Current.PurchaseProductId = "";
								SharedSettings.Current.PurchaseToken = "";
							}
							else
							{
								long ticks = DateTimeNowHelper.NowSafe.Ticks;
								if (new TimeSpan(ticks - SharedSettings.Current.LastTimeLicenceChecked) >= SharedSettings.Current.LicenseCheckPeriod || SharedSettings.Current.LastTimeLicenceChecked > ticks)
								{
									PlatformHelper.DroidService.Droid_InApp_RuKeyActivator_CheckKeyAsync(SharedSettings.Current.PurchaseOrderId, false);
								}
							}
						}
					}
					else if (SharedSettings.Current.WhitelistDeviceActivated)
					{
						if (string.IsNullOrEmpty(SharedSettings.Current.WhitelistDeviceSN))
						{
							SharedSettings.Current.AdsProductPurchased = false;
							InAppManager.GetInstance().Restore();
						}
					}
					else
					{
						InAppManager.GetInstance().Restore();
					}
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000B774 File Offset: 0x00009974
		private void CheckRP_IOS()
		{
			if (!SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.DeveloperMode)
			{
				return;
			}
			if (SharedSettings.Current.WhitelistDeviceActivated)
			{
				if (string.IsNullOrEmpty(SharedSettings.Current.WhitelistDeviceSN))
				{
					SharedSettings.Current.LastTimeLicenceChecked = DateTimeNowHelper.NowSafe.Ticks;
					PlatformHelper.IOSService.StoreReceiptParser_LoadReceipt();
					return;
				}
			}
			else
			{
				long dtTicks = DateTimeNowHelper.NowSafe.Ticks;
				if (new TimeSpan(dtTicks - SharedSettings.Current.LastTimeLicenceChecked) >= SharedSettings.Current.LicenseCheckPeriod || SharedSettings.Current.LastTimeLicenceChecked > dtTicks)
				{
					Task.Run(delegate
					{
						SharedSettings.Current.LastTimeLicenceChecked = dtTicks;
						PlatformHelper.IOSService.StoreReceiptParser_LoadReceipt();
					});
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000B840 File Offset: 0x00009A40
		private void PreventLinkerFromStrippingCommonLocalizationReferences()
		{
			Task.Run(delegate
			{
				try
				{
					new ChineseLunisolarCalendar();
					new GregorianCalendar();
					new HebrewCalendar();
					new HijriCalendar();
					new JapaneseCalendar();
					new JapaneseLunisolarCalendar();
					new JulianCalendar();
					new KoreanCalendar();
					new KoreanLunisolarCalendar();
					new PersianCalendar();
					new TaiwanCalendar();
					new TaiwanLunisolarCalendar();
					new ThaiBuddhistCalendar();
					new UmAlQuraCalendar();
				}
				catch (Exception)
				{
				}
			});
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000B868 File Offset: 0x00009A68
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(App).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "App.xaml",
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
			double num = 12.0;
			double num2 = 14.0;
			double num3 = 17.0;
			double num4 = 19.0;
			double num5 = 28.0;
			double num6 = 11.0;
			double num7 = 9.0;
			double num8 = 7.0;
			bool flag = true;
			bool flag2 = false;
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 55);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 49);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 52);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(NavigationPage)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 46);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			Setter setter5;
			VisualDiagnostics.RegisterSourceInfo(setter5 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 45);
			Setter setter6;
			VisualDiagnostics.RegisterSourceInfo(setter6 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 18);
			Style style2;
			VisualDiagnostics.RegisterSourceInfo(style2 = new Style(typeof(Label)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 14);
			ControlTemplate controlTemplate;
			VisualDiagnostics.RegisterSourceInfo(controlTemplate = new ControlTemplate(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 14);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 52);
			Setter setter7;
			VisualDiagnostics.RegisterSourceInfo(setter7 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 18);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 46);
			Setter setter8;
			VisualDiagnostics.RegisterSourceInfo(setter8 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 18);
			Setter setter9;
			VisualDiagnostics.RegisterSourceInfo(setter9 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 18);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 45);
			Setter setter10;
			VisualDiagnostics.RegisterSourceInfo(setter10 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 18);
			Style style3;
			VisualDiagnostics.RegisterSourceInfo(style3 = new Style(typeof(RadioButtonWithColor)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 14);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 45);
			Setter setter11;
			VisualDiagnostics.RegisterSourceInfo(setter11 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 18);
			Style style4;
			VisualDiagnostics.RegisterSourceInfo(style4 = new Style(typeof(Button)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 14);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 45);
			Setter setter12;
			VisualDiagnostics.RegisterSourceInfo(setter12 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 18);
			Setter setter13;
			VisualDiagnostics.RegisterSourceInfo(setter13 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 18);
			Setter setter14;
			VisualDiagnostics.RegisterSourceInfo(setter14 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 18);
			Setter setter15;
			VisualDiagnostics.RegisterSourceInfo(setter15 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 18);
			Setter setter16;
			VisualDiagnostics.RegisterSourceInfo(setter16 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 18);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 46);
			Setter setter17;
			VisualDiagnostics.RegisterSourceInfo(setter17 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 18);
			Setter setter18;
			VisualDiagnostics.RegisterSourceInfo(setter18 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 18);
			Style style5;
			VisualDiagnostics.RegisterSourceInfo(style5 = new Style(typeof(LinkButton)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 14);
			Setter setter19;
			VisualDiagnostics.RegisterSourceInfo(setter19 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 18);
			Setter setter20;
			VisualDiagnostics.RegisterSourceInfo(setter20 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 18);
			Setter setter21;
			VisualDiagnostics.RegisterSourceInfo(setter21 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 18);
			Setter setter22;
			VisualDiagnostics.RegisterSourceInfo(setter22 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 18);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 46);
			Setter setter23;
			VisualDiagnostics.RegisterSourceInfo(setter23 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 18);
			Setter setter24;
			VisualDiagnostics.RegisterSourceInfo(setter24 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 18);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 45);
			Setter setter25;
			VisualDiagnostics.RegisterSourceInfo(setter25 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 18);
			Style style6;
			VisualDiagnostics.RegisterSourceInfo(style6 = new Style(typeof(LinkButton)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 14);
			Setter setter26;
			VisualDiagnostics.RegisterSourceInfo(setter26 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 18);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 46);
			Setter setter27;
			VisualDiagnostics.RegisterSourceInfo(setter27 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 45);
			Setter setter28;
			VisualDiagnostics.RegisterSourceInfo(setter28 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 18);
			Setter setter29;
			VisualDiagnostics.RegisterSourceInfo(setter29 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 18);
			Setter setter30;
			VisualDiagnostics.RegisterSourceInfo(setter30 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 18);
			Style style7;
			VisualDiagnostics.RegisterSourceInfo(style7 = new Style(typeof(Label)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 14);
			Setter setter31;
			VisualDiagnostics.RegisterSourceInfo(setter31 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 18);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 46);
			Setter setter32;
			VisualDiagnostics.RegisterSourceInfo(setter32 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 18);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 45);
			Setter setter33;
			VisualDiagnostics.RegisterSourceInfo(setter33 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 18);
			Setter setter34;
			VisualDiagnostics.RegisterSourceInfo(setter34 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 18);
			Setter setter35;
			VisualDiagnostics.RegisterSourceInfo(setter35 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 18);
			Style style8;
			VisualDiagnostics.RegisterSourceInfo(style8 = new Style(typeof(NonScalableLabel)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 14);
			Setter setter36;
			VisualDiagnostics.RegisterSourceInfo(setter36 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 18);
			DynamicResourceExtension dynamicResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension18 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 44);
			Setter setter37;
			VisualDiagnostics.RegisterSourceInfo(setter37 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 18);
			Style style9;
			VisualDiagnostics.RegisterSourceInfo(style9 = new Style(typeof(Switch)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 14);
			DynamicResourceExtension dynamicResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension19 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 52);
			Setter setter38;
			VisualDiagnostics.RegisterSourceInfo(setter38 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 18);
			DynamicResourceExtension dynamicResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension20 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 46);
			Setter setter39;
			VisualDiagnostics.RegisterSourceInfo(setter39 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 18);
			DynamicResourceExtension dynamicResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension21 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 45);
			Setter setter40;
			VisualDiagnostics.RegisterSourceInfo(setter40 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 18);
			Style style10;
			VisualDiagnostics.RegisterSourceInfo(style10 = new Style(typeof(Picker)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 14);
			DynamicResourceExtension dynamicResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension22 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 45);
			Setter setter41;
			VisualDiagnostics.RegisterSourceInfo(setter41 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 18);
			DynamicResourceExtension dynamicResourceExtension23;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension23 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 52);
			Setter setter42;
			VisualDiagnostics.RegisterSourceInfo(setter42 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 18);
			DynamicResourceExtension dynamicResourceExtension24;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension24 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 53);
			Setter setter43;
			VisualDiagnostics.RegisterSourceInfo(setter43 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 18);
			DynamicResourceExtension dynamicResourceExtension25;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension25 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 46);
			Setter setter44;
			VisualDiagnostics.RegisterSourceInfo(setter44 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 18);
			Style style11;
			VisualDiagnostics.RegisterSourceInfo(style11 = new Style(typeof(Entry)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 14);
			DynamicResourceExtension dynamicResourceExtension26;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension26 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 45);
			Setter setter45;
			VisualDiagnostics.RegisterSourceInfo(setter45 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 18);
			DynamicResourceExtension dynamicResourceExtension27;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension27 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 46);
			Setter setter46;
			VisualDiagnostics.RegisterSourceInfo(setter46 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 18);
			Style style12;
			VisualDiagnostics.RegisterSourceInfo(style12 = new Style(typeof(HyperLinkLabel)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 14);
			DynamicResourceExtension dynamicResourceExtension28;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension28 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 47);
			Setter setter47;
			VisualDiagnostics.RegisterSourceInfo(setter47 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 18);
			DynamicResourceExtension dynamicResourceExtension29;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension29 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 52);
			Setter setter48;
			VisualDiagnostics.RegisterSourceInfo(setter48 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 18);
			Style style13;
			VisualDiagnostics.RegisterSourceInfo(style13 = new Style(typeof(Slider)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 14);
			DynamicResourceExtension dynamicResourceExtension30;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension30 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 47);
			Setter setter49;
			VisualDiagnostics.RegisterSourceInfo(setter49 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 18);
			DynamicResourceExtension dynamicResourceExtension31;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension31 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 52);
			Setter setter50;
			VisualDiagnostics.RegisterSourceInfo(setter50 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 18);
			Style style14;
			VisualDiagnostics.RegisterSourceInfo(style14 = new Style(typeof(ExtendedSlider)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 14);
			DynamicResourceExtension dynamicResourceExtension32;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension32 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 52);
			Setter setter51;
			VisualDiagnostics.RegisterSourceInfo(setter51 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 18);
			Style style15;
			VisualDiagnostics.RegisterSourceInfo(style15 = new Style(typeof(Stepper)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 14);
			DynamicResourceExtension dynamicResourceExtension33;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension33 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 45);
			Setter setter52;
			VisualDiagnostics.RegisterSourceInfo(setter52 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 18);
			DynamicResourceExtension dynamicResourceExtension34;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension34 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 52);
			Setter setter53;
			VisualDiagnostics.RegisterSourceInfo(setter53 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 18);
			DynamicResourceExtension dynamicResourceExtension35;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension35 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 53);
			Setter setter54;
			VisualDiagnostics.RegisterSourceInfo(setter54 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 18);
			DynamicResourceExtension dynamicResourceExtension36;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension36 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 46);
			Setter setter55;
			VisualDiagnostics.RegisterSourceInfo(setter55 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 18);
			Style style16;
			VisualDiagnostics.RegisterSourceInfo(style16 = new Style(typeof(SearchBar)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 14);
			Setter setter56;
			VisualDiagnostics.RegisterSourceInfo(setter56 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 18);
			DynamicResourceExtension dynamicResourceExtension37;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension37 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 51);
			Setter setter57;
			VisualDiagnostics.RegisterSourceInfo(setter57 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 18);
			Style style17;
			VisualDiagnostics.RegisterSourceInfo(style17 = new Style(typeof(ListView)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 14);
			DynamicResourceExtension dynamicResourceExtension38;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension38 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 45);
			Setter setter58;
			VisualDiagnostics.RegisterSourceInfo(setter58 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 18);
			DynamicResourceExtension dynamicResourceExtension39;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension39 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 49);
			Setter setter59;
			VisualDiagnostics.RegisterSourceInfo(setter59 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 18);
			DynamicResourceExtension dynamicResourceExtension40;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension40 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 51);
			Setter setter60;
			VisualDiagnostics.RegisterSourceInfo(setter60 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 18);
			DynamicResourceExtension dynamicResourceExtension41;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension41 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 46);
			Setter setter61;
			VisualDiagnostics.RegisterSourceInfo(setter61 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 18);
			Setter setter62;
			VisualDiagnostics.RegisterSourceInfo(setter62 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 18);
			Style style18;
			VisualDiagnostics.RegisterSourceInfo(style18 = new Style(typeof(SfRadioButton)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 14);
			DynamicResourceExtension dynamicResourceExtension42;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension42 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 45);
			Setter setter63;
			VisualDiagnostics.RegisterSourceInfo(setter63 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 18);
			DynamicResourceExtension dynamicResourceExtension43;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension43 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 46);
			Setter setter64;
			VisualDiagnostics.RegisterSourceInfo(setter64 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 18);
			DynamicResourceExtension dynamicResourceExtension44;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension44 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 45);
			Setter setter65;
			VisualDiagnostics.RegisterSourceInfo(setter65 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 18);
			Setter setter66;
			VisualDiagnostics.RegisterSourceInfo(setter66 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 18);
			Style style19;
			VisualDiagnostics.RegisterSourceInfo(style19 = new Style(typeof(RadioButton)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 14);
			DynamicResourceExtension dynamicResourceExtension45;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension45 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 48);
			Setter setter67;
			VisualDiagnostics.RegisterSourceInfo(setter67 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 18);
			Style style20;
			VisualDiagnostics.RegisterSourceInfo(style20 = new Style(typeof(ChartLineStyle)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 14);
			DynamicResourceExtension dynamicResourceExtension46;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension46 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 46);
			Setter setter68;
			VisualDiagnostics.RegisterSourceInfo(setter68 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 18);
			Style style21;
			VisualDiagnostics.RegisterSourceInfo(style21 = new Style(typeof(ChartAxisLabelStyle)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 14);
			DynamicResourceExtension dynamicResourceExtension47;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension47 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 46);
			Setter setter69;
			VisualDiagnostics.RegisterSourceInfo(setter69 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 18);
			Style style22;
			VisualDiagnostics.RegisterSourceInfo(style22 = new Style(typeof(ChartLabelStyle)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 14);
			DynamicResourceExtension dynamicResourceExtension48;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension48 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 46);
			Setter setter70;
			VisualDiagnostics.RegisterSourceInfo(setter70 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 18);
			DynamicResourceExtension dynamicResourceExtension49;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension49 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 48);
			Setter setter71;
			VisualDiagnostics.RegisterSourceInfo(setter71 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 18);
			Style style23;
			VisualDiagnostics.RegisterSourceInfo(style23 = new Style(typeof(TextCell)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 14);
			DynamicResourceExtension dynamicResourceExtension50;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension50 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 51);
			Setter setter72;
			VisualDiagnostics.RegisterSourceInfo(setter72 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 18);
			DynamicResourceExtension dynamicResourceExtension51;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension51 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 52);
			Setter setter73;
			VisualDiagnostics.RegisterSourceInfo(setter73 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 18);
			DynamicResourceExtension dynamicResourceExtension52;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension52 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 58);
			Setter setter74;
			VisualDiagnostics.RegisterSourceInfo(setter74 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 18);
			DynamicResourceExtension dynamicResourceExtension53;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension53 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 56);
			Setter setter75;
			VisualDiagnostics.RegisterSourceInfo(setter75 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 18);
			DynamicResourceExtension dynamicResourceExtension54;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension54 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 51);
			Setter setter76;
			VisualDiagnostics.RegisterSourceInfo(setter76 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 18);
			DynamicResourceExtension dynamicResourceExtension55;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension55 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 55);
			Setter setter77;
			VisualDiagnostics.RegisterSourceInfo(setter77 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 18);
			DynamicResourceExtension dynamicResourceExtension56;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension56 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 54);
			Setter setter78;
			VisualDiagnostics.RegisterSourceInfo(setter78 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 18);
			DynamicResourceExtension dynamicResourceExtension57;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension57 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 58);
			Setter setter79;
			VisualDiagnostics.RegisterSourceInfo(setter79 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 18);
			DynamicResourceExtension dynamicResourceExtension58;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension58 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 57);
			Setter setter80;
			VisualDiagnostics.RegisterSourceInfo(setter80 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 18);
			DynamicResourceExtension dynamicResourceExtension59;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension59 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 60);
			Setter setter81;
			VisualDiagnostics.RegisterSourceInfo(setter81 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 18);
			DynamicResourceExtension dynamicResourceExtension60;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension60 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 52);
			Setter setter82;
			VisualDiagnostics.RegisterSourceInfo(setter82 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 18);
			DynamicResourceExtension dynamicResourceExtension61;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension61 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 50);
			Setter setter83;
			VisualDiagnostics.RegisterSourceInfo(setter83 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 18);
			DynamicResourceExtension dynamicResourceExtension62;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension62 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 52);
			Setter setter84;
			VisualDiagnostics.RegisterSourceInfo(setter84 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 18);
			DynamicResourceExtension dynamicResourceExtension63;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension63 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 51);
			Setter setter85;
			VisualDiagnostics.RegisterSourceInfo(setter85 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 18);
			DynamicResourceExtension dynamicResourceExtension64;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension64 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 51);
			Setter setter86;
			VisualDiagnostics.RegisterSourceInfo(setter86 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 18);
			DynamicResourceExtension dynamicResourceExtension65;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension65 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 52);
			Setter setter87;
			VisualDiagnostics.RegisterSourceInfo(setter87 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 18);
			DynamicResourceExtension dynamicResourceExtension66;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension66 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 54);
			Setter setter88;
			VisualDiagnostics.RegisterSourceInfo(setter88 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 18);
			Style style24;
			VisualDiagnostics.RegisterSourceInfo(style24 = new Style(typeof(SettingsView)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 14);
			DynamicResourceExtension dynamicResourceExtension67;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension67 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 47);
			Setter setter89;
			VisualDiagnostics.RegisterSourceInfo(setter89 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 18);
			Style style25;
			VisualDiagnostics.RegisterSourceInfo(style25 = new Style(typeof(ButtonCell)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 14);
			DynamicResourceExtension dynamicResourceExtension68;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension68 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 45);
			Setter setter90;
			VisualDiagnostics.RegisterSourceInfo(setter90 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 18);
			Setter setter91;
			VisualDiagnostics.RegisterSourceInfo(setter91 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 18);
			Setter setter92;
			VisualDiagnostics.RegisterSourceInfo(setter92 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 18);
			Setter setter93;
			VisualDiagnostics.RegisterSourceInfo(setter93 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 18);
			Setter setter94;
			VisualDiagnostics.RegisterSourceInfo(setter94 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 18);
			DynamicResourceExtension dynamicResourceExtension69;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension69 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 277, 46);
			Setter setter95;
			VisualDiagnostics.RegisterSourceInfo(setter95 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 277, 18);
			Style style26;
			VisualDiagnostics.RegisterSourceInfo(style26 = new Style(typeof(Label)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 14);
			Setter setter96;
			VisualDiagnostics.RegisterSourceInfo(setter96 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 18);
			Style style27;
			VisualDiagnostics.RegisterSourceInfo(style27 = new Style(typeof(Entry)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 282, 14);
			ChartLineStyle chartLineStyle;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle = new ChartLineStyle(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 14);
			ChartAxisTickStyle chartAxisTickStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisTickStyle = new ChartAxisTickStyle(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 287, 14);
			DynamicResourceExtension dynamicResourceExtension70;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension70 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 42);
			Setter setter97;
			VisualDiagnostics.RegisterSourceInfo(setter97 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 18);
			Style style28;
			VisualDiagnostics.RegisterSourceInfo(style28 = new Style(typeof(CheckBoxWithColor)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 14);
			DynamicResourceExtension dynamicResourceExtension71;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension71 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 294, 47);
			Setter setter98;
			VisualDiagnostics.RegisterSourceInfo(setter98 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 294, 18);
			Style style29;
			VisualDiagnostics.RegisterSourceInfo(style29 = new Style(typeof(CheckBoxWithLabel)), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 293, 14);
			SolidColorBrush solidColorBrush;
			VisualDiagnostics.RegisterSourceInfo(solidColorBrush = new SolidColorBrush(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 298, 14);
			SolidColorBrush solidColorBrush2;
			VisualDiagnostics.RegisterSourceInfo(solidColorBrush2 = new SolidColorBrush(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 14);
			ResourceDictionary resourceDictionary2;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary2 = new ResourceDictionary(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
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
			NameScope nameScope22 = new NameScope();
			NameScope nameScope23 = new NameScope();
			NameScope nameScope24 = new NameScope();
			NameScope nameScope25 = new NameScope();
			NameScope nameScope26 = new NameScope();
			NameScope nameScope27 = new NameScope();
			NameScope nameScope28 = new NameScope();
			NameScope nameScope29 = new NameScope();
			NameScope nameScope30 = new NameScope();
			NameScope nameScope31 = new NameScope();
			NameScope nameScope32 = new NameScope();
			NameScope nameScope33 = new NameScope();
			NameScope nameScope34 = new NameScope();
			NameScope nameScope35 = new NameScope();
			NameScope nameScope36 = new NameScope();
			NameScope nameScope37 = new NameScope();
			NameScope nameScope38 = new NameScope();
			NameScope nameScope39 = new NameScope();
			NameScope nameScope40 = new NameScope();
			NameScope nameScope41 = new NameScope();
			NameScope nameScope42 = new NameScope();
			NameScope nameScope43 = new NameScope();
			NameScope nameScope44 = new NameScope();
			NameScope nameScope45 = new NameScope();
			NameScope nameScope46 = new NameScope();
			NameScope nameScope47 = new NameScope();
			NameScope nameScope48 = new NameScope();
			NameScope nameScope49 = new NameScope();
			NameScope nameScope50 = new NameScope();
			NameScope nameScope51 = new NameScope();
			NameScope nameScope52 = new NameScope();
			NameScope nameScope53 = new NameScope();
			NameScope nameScope54 = new NameScope();
			NameScope nameScope55 = new NameScope();
			NameScope nameScope56 = new NameScope();
			NameScope nameScope57 = new NameScope();
			NameScope nameScope58 = new NameScope();
			NameScope nameScope59 = new NameScope();
			NameScope nameScope60 = new NameScope();
			NameScope nameScope61 = new NameScope();
			NameScope nameScope62 = new NameScope();
			NameScope nameScope63 = new NameScope();
			NameScope nameScope64 = new NameScope();
			NameScope nameScope65 = new NameScope();
			NameScope nameScope66 = new NameScope();
			NameScope nameScope67 = new NameScope();
			NameScope nameScope68 = new NameScope();
			NameScope nameScope69 = new NameScope();
			NameScope nameScope70 = new NameScope();
			NameScope nameScope71 = new NameScope();
			NameScope nameScope72 = new NameScope();
			NameScope nameScope73 = new NameScope();
			NameScope nameScope74 = new NameScope();
			NameScope nameScope75 = new NameScope();
			NameScope nameScope76 = new NameScope();
			NameScope nameScope77 = new NameScope();
			NameScope nameScope78 = new NameScope();
			NameScope nameScope79 = new NameScope();
			NameScope nameScope80 = new NameScope();
			NameScope nameScope81 = new NameScope();
			NameScope nameScope82 = new NameScope();
			NameScope nameScope83 = new NameScope();
			NameScope nameScope84 = new NameScope();
			NameScope nameScope85 = new NameScope();
			NameScope nameScope86 = new NameScope();
			NameScope nameScope87 = new NameScope();
			NameScope nameScope88 = new NameScope();
			NameScope nameScope89 = new NameScope();
			NameScope nameScope90 = new NameScope();
			NameScope nameScope91 = new NameScope();
			NameScope nameScope92 = new NameScope();
			NameScope nameScope93 = new NameScope();
			NameScope nameScope94 = new NameScope();
			NameScope nameScope95 = new NameScope();
			NameScope nameScope96 = new NameScope();
			NameScope nameScope97 = new NameScope();
			NameScope nameScope98 = new NameScope();
			NameScope nameScope99 = new NameScope();
			this.Resources = resourceDictionary2;
			resourceDictionary2.Add("BaseFontSize", num);
			resourceDictionary2.Add("BaseFontSize+", num2);
			resourceDictionary2.Add("BaseFontSize++", num3);
			resourceDictionary2.Add("BaseFontSize+++", num4);
			resourceDictionary2.Add("BaseFontSize++++", num5);
			resourceDictionary2.Add("BaseFontSize-", num6);
			resourceDictionary2.Add("BaseFontSize--", num7);
			resourceDictionary2.Add("BaseFontSize---", num8);
			resourceDictionary2.Add("TrueValue", flag);
			resourceDictionary2.Add("FalseValue", flag2);
			ResourceDictionary resourceDictionary3 = resourceDictionary;
			Uri uri;
			resourceDictionary.SetAndLoadSource(uri = new Uri("/Styles/LightTheme.xaml;assembly=CarScannerXamarinForms", UriKind.RelativeOrAbsolute), "Styles/LightTheme.xaml", typeof(App).GetTypeInfo().Assembly, new XmlLineInfo(29, 33));
			resourceDictionary3.Source = uri;
			resourceDictionary2.Add(resourceDictionary);
			setter.Property = NavigationPage.BarBackgroundColorProperty;
			dynamicResourceExtension.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = setter;
			array[1] = style;
			array[2] = resourceDictionary2;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(Setter).GetRuntimeProperty("Value"), nameScope2));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 55)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			setter.Value = dynamicResource;
			style.Setters.Add(setter);
			setter2.Property = NavigationPage.BarTextColorProperty;
			dynamicResourceExtension2.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = setter2;
			array2[1] = style;
			array2[2] = resourceDictionary2;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, typeof(Setter).GetRuntimeProperty("Value"), nameScope3));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 49)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			setter2.Value = dynamicResource2;
			style.Setters.Add(setter2);
			setter3.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension3.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = setter3;
			array3[1] = style;
			array3[2] = resourceDictionary2;
			array3[3] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, typeof(Setter).GetRuntimeProperty("Value"), nameScope4));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 52)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			setter3.Value = dynamicResource3;
			style.Setters.Add(setter3);
			resourceDictionary2.Add(style);
			setter4.Property = Label.TextColorProperty;
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = setter4;
			array4[1] = style2;
			array4[2] = resourceDictionary2;
			array4[3] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, typeof(Setter).GetRuntimeProperty("Value"), nameScope5));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 46)));
			DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
			setter4.Value = dynamicResource4;
			style2.Setters.Add(setter4);
			setter5.Property = VisualElement.BackgroundColorProperty;
			setter5.Value = "Transparent";
			setter5.Value = Color.Transparent;
			style2.Setters.Add(setter5);
			setter6.Property = Label.FontSizeProperty;
			dynamicResourceExtension5.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = setter6;
			array5[1] = style2;
			array5[2] = resourceDictionary2;
			array5[3] = this;
			object obj5;
			xamlServiceProvider5.Add(typeFromHandle9, obj5 = new SimpleValueTargetProvider(array5, typeof(Setter).GetRuntimeProperty("Value"), nameScope7));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 45)));
			DynamicResource dynamicResource5 = markupExtension5.ProvideValue(xamlServiceProvider5);
			setter6.Value = dynamicResource5;
			style2.Setters.Add(setter6);
			resourceDictionary2.Add(style2);
			IDataTemplate dataTemplate = controlTemplate;
			App.<InitializeComponent>_anonXamlCDataTemplate_0 <InitializeComponent>_anonXamlCDataTemplate_ = new App.<InitializeComponent>_anonXamlCDataTemplate_0();
			object[] array6 = new object[0 + 3];
			array6[0] = controlTemplate;
			array6[1] = resourceDictionary2;
			array6[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array6;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary2.Add("ThemeRadioTemplate", controlTemplate);
			setter7.Property = TemplatedView.ControlTemplateProperty;
			dynamicResourceExtension6.Key = "ThemeRadioTemplate";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = setter7;
			array7[1] = style3;
			array7[2] = resourceDictionary2;
			array7[3] = this;
			object obj6;
			xamlServiceProvider6.Add(typeFromHandle11, obj6 = new SimpleValueTargetProvider(array7, typeof(Setter).GetRuntimeProperty("Value"), nameScope8));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 52)));
			DynamicResource dynamicResource6 = markupExtension6.ProvideValue(xamlServiceProvider6);
			setter7.Value = dynamicResource6;
			style3.Setters.Add(setter7);
			setter8.Property = RadioButton.TextColorProperty;
			dynamicResourceExtension7.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = setter8;
			array8[1] = style3;
			array8[2] = resourceDictionary2;
			array8[3] = this;
			object obj7;
			xamlServiceProvider7.Add(typeFromHandle13, obj7 = new SimpleValueTargetProvider(array8, typeof(Setter).GetRuntimeProperty("Value"), nameScope9));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 46)));
			DynamicResource dynamicResource7 = markupExtension7.ProvideValue(xamlServiceProvider7);
			setter8.Value = dynamicResource7;
			style3.Setters.Add(setter8);
			setter9.Property = VisualElement.BackgroundColorProperty;
			setter9.Value = "Transparent";
			setter9.Value = Color.Transparent;
			style3.Setters.Add(setter9);
			setter10.Property = RadioButton.FontSizeProperty;
			dynamicResourceExtension8.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = setter10;
			array9[1] = style3;
			array9[2] = resourceDictionary2;
			array9[3] = this;
			object obj8;
			xamlServiceProvider8.Add(typeFromHandle15, obj8 = new SimpleValueTargetProvider(array9, typeof(Setter).GetRuntimeProperty("Value"), nameScope11));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 45)));
			DynamicResource dynamicResource8 = markupExtension8.ProvideValue(xamlServiceProvider8);
			setter10.Value = dynamicResource8;
			style3.Setters.Add(setter10);
			resourceDictionary2.Add(style3);
			setter11.Property = Button.FontSizeProperty;
			dynamicResourceExtension9.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = setter11;
			array10[1] = style4;
			array10[2] = resourceDictionary2;
			array10[3] = this;
			object obj9;
			xamlServiceProvider9.Add(typeFromHandle17, obj9 = new SimpleValueTargetProvider(array10, typeof(Setter).GetRuntimeProperty("Value"), nameScope12));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 45)));
			DynamicResource dynamicResource9 = markupExtension9.ProvideValue(xamlServiceProvider9);
			setter11.Value = dynamicResource9;
			style4.Setters.Add(setter11);
			resourceDictionary2.Add(style4);
			setter12.Property = Button.FontSizeProperty;
			dynamicResourceExtension10.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = setter12;
			array11[1] = style5;
			array11[2] = resourceDictionary2;
			array11[3] = this;
			object obj10;
			xamlServiceProvider10.Add(typeFromHandle19, obj10 = new SimpleValueTargetProvider(array11, typeof(Setter).GetRuntimeProperty("Value"), nameScope13));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 45)));
			DynamicResource dynamicResource10 = markupExtension10.ProvideValue(xamlServiceProvider10);
			setter12.Value = dynamicResource10;
			style5.Setters.Add(setter12);
			setter13.Property = Button.BorderColorProperty;
			setter13.Value = "Transparent";
			setter13.Value = Color.Transparent;
			style5.Setters.Add(setter13);
			setter14.Property = VisualElement.BackgroundColorProperty;
			setter14.Value = "Transparent";
			setter14.Value = Color.Transparent;
			style5.Setters.Add(setter14);
			setter15.Property = View.MarginProperty;
			setter15.Value = "0";
			setter15.Value = new Thickness(0.0);
			style5.Setters.Add(setter15);
			setter16.Property = View.VerticalOptionsProperty;
			setter16.Value = "Center";
			setter16.Value = LayoutOptions.Center;
			style5.Setters.Add(setter16);
			setter17.Property = Button.TextColorProperty;
			dynamicResourceExtension11.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = setter17;
			array12[1] = style5;
			array12[2] = resourceDictionary2;
			array12[3] = this;
			object obj11;
			xamlServiceProvider11.Add(typeFromHandle21, obj11 = new SimpleValueTargetProvider(array12, typeof(Setter).GetRuntimeProperty("Value"), nameScope18));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 46)));
			DynamicResource dynamicResource11 = markupExtension11.ProvideValue(xamlServiceProvider11);
			setter17.Value = dynamicResource11;
			style5.Setters.Add(setter17);
			setter18.Property = LinkButton.IgnoreScalingProperty;
			setter18.Value = "False";
			setter18.Value = false;
			style5.Setters.Add(setter18);
			resourceDictionary2.Add(style5);
			setter19.Property = Button.BorderColorProperty;
			setter19.Value = "Transparent";
			setter19.Value = Color.Transparent;
			style6.Setters.Add(setter19);
			setter20.Property = VisualElement.BackgroundColorProperty;
			setter20.Value = "Transparent";
			setter20.Value = Color.Transparent;
			style6.Setters.Add(setter20);
			setter21.Property = View.MarginProperty;
			setter21.Value = "0";
			setter21.Value = new Thickness(0.0);
			style6.Setters.Add(setter21);
			setter22.Property = View.VerticalOptionsProperty;
			setter22.Value = "Center";
			setter22.Value = LayoutOptions.Center;
			style6.Setters.Add(setter22);
			setter23.Property = Button.TextColorProperty;
			dynamicResourceExtension12.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = setter23;
			array13[1] = style6;
			array13[2] = resourceDictionary2;
			array13[3] = this;
			object obj12;
			xamlServiceProvider12.Add(typeFromHandle23, obj12 = new SimpleValueTargetProvider(array13, typeof(Setter).GetRuntimeProperty("Value"), nameScope24));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 46)));
			DynamicResource dynamicResource12 = markupExtension12.ProvideValue(xamlServiceProvider12);
			setter23.Value = dynamicResource12;
			style6.Setters.Add(setter23);
			setter24.Property = LinkButton.IgnoreScalingProperty;
			setter24.Value = "True";
			setter24.Value = true;
			style6.Setters.Add(setter24);
			setter25.Property = Button.FontSizeProperty;
			dynamicResourceExtension13.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = setter25;
			array14[1] = style6;
			array14[2] = resourceDictionary2;
			array14[3] = this;
			object obj13;
			xamlServiceProvider13.Add(typeFromHandle25, obj13 = new SimpleValueTargetProvider(array14, typeof(Setter).GetRuntimeProperty("Value"), nameScope26));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 45)));
			DynamicResource dynamicResource13 = markupExtension13.ProvideValue(xamlServiceProvider13);
			setter25.Value = dynamicResource13;
			style6.Setters.Add(setter25);
			resourceDictionary2.Add("NavigationBarButton", style6);
			setter26.Property = View.VerticalOptionsProperty;
			setter26.Value = "Center";
			setter26.Value = LayoutOptions.Center;
			style7.Setters.Add(setter26);
			setter27.Property = Label.TextColorProperty;
			dynamicResourceExtension14.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = setter27;
			array15[1] = style7;
			array15[2] = resourceDictionary2;
			array15[3] = this;
			object obj14;
			xamlServiceProvider14.Add(typeFromHandle27, obj14 = new SimpleValueTargetProvider(array15, typeof(Setter).GetRuntimeProperty("Value"), nameScope28));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 46)));
			DynamicResource dynamicResource14 = markupExtension14.ProvideValue(xamlServiceProvider14);
			setter27.Value = dynamicResource14;
			style7.Setters.Add(setter27);
			setter28.Property = Label.FontSizeProperty;
			dynamicResourceExtension15.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = setter28;
			array16[1] = style7;
			array16[2] = resourceDictionary2;
			array16[3] = this;
			object obj15;
			xamlServiceProvider15.Add(typeFromHandle29, obj15 = new SimpleValueTargetProvider(array16, typeof(Setter).GetRuntimeProperty("Value"), nameScope29));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 45)));
			DynamicResource dynamicResource15 = markupExtension15.ProvideValue(xamlServiceProvider15);
			setter28.Value = dynamicResource15;
			style7.Setters.Add(setter28);
			setter29.Property = Label.LineBreakModeProperty;
			setter29.Value = "TailTruncation";
			setter29.Value = 4;
			style7.Setters.Add(setter29);
			setter30.Property = Label.FontAttributesProperty;
			setter30.Value = "Bold";
			setter30.Value = new FontAttributesConverter().ConvertFromInvariantString("Bold");
			style7.Setters.Add(setter30);
			resourceDictionary2.Add("NavigationBarLabel", style7);
			setter31.Property = View.VerticalOptionsProperty;
			setter31.Value = "Center";
			setter31.Value = LayoutOptions.Center;
			style8.Setters.Add(setter31);
			setter32.Property = Label.TextColorProperty;
			dynamicResourceExtension16.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = setter32;
			array17[1] = style8;
			array17[2] = resourceDictionary2;
			array17[3] = this;
			object obj16;
			xamlServiceProvider16.Add(typeFromHandle31, obj16 = new SimpleValueTargetProvider(array17, typeof(Setter).GetRuntimeProperty("Value"), nameScope33));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 46)));
			DynamicResource dynamicResource16 = markupExtension16.ProvideValue(xamlServiceProvider16);
			setter32.Value = dynamicResource16;
			style8.Setters.Add(setter32);
			setter33.Property = Label.FontSizeProperty;
			dynamicResourceExtension17.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = setter33;
			array18[1] = style8;
			array18[2] = resourceDictionary2;
			array18[3] = this;
			object obj17;
			xamlServiceProvider17.Add(typeFromHandle33, obj17 = new SimpleValueTargetProvider(array18, typeof(Setter).GetRuntimeProperty("Value"), nameScope34));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 45)));
			DynamicResource dynamicResource17 = markupExtension17.ProvideValue(xamlServiceProvider17);
			setter33.Value = dynamicResource17;
			style8.Setters.Add(setter33);
			setter34.Property = Label.LineBreakModeProperty;
			setter34.Value = "TailTruncation";
			setter34.Value = 4;
			style8.Setters.Add(setter34);
			setter35.Property = Label.FontAttributesProperty;
			setter35.Value = "Bold";
			setter35.Value = new FontAttributesConverter().ConvertFromInvariantString("Bold");
			style8.Setters.Add(setter35);
			resourceDictionary2.Add("NavigationBarNonScalableLabel", style8);
			setter36.Property = VisualElement.BackgroundColorProperty;
			setter36.Value = "Transparent";
			setter36.Value = Color.Transparent;
			style9.Setters.Add(setter36);
			setter37.Property = Switch.OnColorProperty;
			dynamicResourceExtension18.Key = "SwitchOnColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension18;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = setter37;
			array19[1] = style9;
			array19[2] = resourceDictionary2;
			array19[3] = this;
			object obj18;
			xamlServiceProvider18.Add(typeFromHandle35, obj18 = new SimpleValueTargetProvider(array19, typeof(Setter).GetRuntimeProperty("Value"), nameScope38));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 44)));
			DynamicResource dynamicResource18 = markupExtension18.ProvideValue(xamlServiceProvider18);
			setter37.Value = dynamicResource18;
			style9.Setters.Add(setter37);
			resourceDictionary2.Add(style9);
			setter38.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension19.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension19;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = setter38;
			array20[1] = style10;
			array20[2] = resourceDictionary2;
			array20[3] = this;
			object obj19;
			xamlServiceProvider19.Add(typeFromHandle37, obj19 = new SimpleValueTargetProvider(array20, typeof(Setter).GetRuntimeProperty("Value"), nameScope39));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(162, 52)));
			DynamicResource dynamicResource19 = markupExtension19.ProvideValue(xamlServiceProvider19);
			setter38.Value = dynamicResource19;
			style10.Setters.Add(setter38);
			setter39.Property = Picker.TextColorProperty;
			dynamicResourceExtension20.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension20 = dynamicResourceExtension20;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = setter39;
			array21[1] = style10;
			array21[2] = resourceDictionary2;
			array21[3] = this;
			object obj20;
			xamlServiceProvider20.Add(typeFromHandle39, obj20 = new SimpleValueTargetProvider(array21, typeof(Setter).GetRuntimeProperty("Value"), nameScope40));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(163, 46)));
			DynamicResource dynamicResource20 = markupExtension20.ProvideValue(xamlServiceProvider20);
			setter39.Value = dynamicResource20;
			style10.Setters.Add(setter39);
			setter40.Property = Picker.FontSizeProperty;
			dynamicResourceExtension21.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension21;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = setter40;
			array22[1] = style10;
			array22[2] = resourceDictionary2;
			array22[3] = this;
			object obj21;
			xamlServiceProvider21.Add(typeFromHandle41, obj21 = new SimpleValueTargetProvider(array22, typeof(Setter).GetRuntimeProperty("Value"), nameScope41));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 45)));
			DynamicResource dynamicResource21 = markupExtension21.ProvideValue(xamlServiceProvider21);
			setter40.Value = dynamicResource21;
			style10.Setters.Add(setter40);
			resourceDictionary2.Add(style10);
			setter41.Property = Entry.FontSizeProperty;
			dynamicResourceExtension22.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension22;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = setter41;
			array23[1] = style11;
			array23[2] = resourceDictionary2;
			array23[3] = this;
			object obj22;
			xamlServiceProvider22.Add(typeFromHandle43, obj22 = new SimpleValueTargetProvider(array23, typeof(Setter).GetRuntimeProperty("Value"), nameScope42));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 45)));
			DynamicResource dynamicResource22 = markupExtension22.ProvideValue(xamlServiceProvider22);
			setter41.Value = dynamicResource22;
			style11.Setters.Add(setter41);
			setter42.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension23.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension23;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = setter42;
			array24[1] = style11;
			array24[2] = resourceDictionary2;
			array24[3] = this;
			object obj23;
			xamlServiceProvider23.Add(typeFromHandle45, obj23 = new SimpleValueTargetProvider(array24, typeof(Setter).GetRuntimeProperty("Value"), nameScope43));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(169, 52)));
			DynamicResource dynamicResource23 = markupExtension23.ProvideValue(xamlServiceProvider23);
			setter42.Value = dynamicResource23;
			style11.Setters.Add(setter42);
			setter43.Property = Entry.PlaceholderColorProperty;
			dynamicResourceExtension24.Key = "GrayedTextColor";
			IMarkupExtension<DynamicResource> markupExtension24 = dynamicResourceExtension24;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = setter43;
			array25[1] = style11;
			array25[2] = resourceDictionary2;
			array25[3] = this;
			object obj24;
			xamlServiceProvider24.Add(typeFromHandle47, obj24 = new SimpleValueTargetProvider(array25, typeof(Setter).GetRuntimeProperty("Value"), nameScope44));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 53)));
			DynamicResource dynamicResource24 = markupExtension24.ProvideValue(xamlServiceProvider24);
			setter43.Value = dynamicResource24;
			style11.Setters.Add(setter43);
			setter44.Property = Entry.TextColorProperty;
			dynamicResourceExtension25.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension25;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = setter44;
			array26[1] = style11;
			array26[2] = resourceDictionary2;
			array26[3] = this;
			object obj25;
			xamlServiceProvider25.Add(typeFromHandle49, obj25 = new SimpleValueTargetProvider(array26, typeof(Setter).GetRuntimeProperty("Value"), nameScope45));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 46)));
			DynamicResource dynamicResource25 = markupExtension25.ProvideValue(xamlServiceProvider25);
			setter44.Value = dynamicResource25;
			style11.Setters.Add(setter44);
			resourceDictionary2.Add(style11);
			setter45.Property = Label.FontSizeProperty;
			dynamicResourceExtension26.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension26 = dynamicResourceExtension26;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 4];
			array27[0] = setter45;
			array27[1] = style12;
			array27[2] = resourceDictionary2;
			array27[3] = this;
			object obj26;
			xamlServiceProvider26.Add(typeFromHandle51, obj26 = new SimpleValueTargetProvider(array27, typeof(Setter).GetRuntimeProperty("Value"), nameScope46));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 45)));
			DynamicResource dynamicResource26 = markupExtension26.ProvideValue(xamlServiceProvider26);
			setter45.Value = dynamicResource26;
			style12.Setters.Add(setter45);
			setter46.Property = Label.TextColorProperty;
			dynamicResourceExtension27.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension27 = dynamicResourceExtension27;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = setter46;
			array28[1] = style12;
			array28[2] = resourceDictionary2;
			array28[3] = this;
			object obj27;
			xamlServiceProvider27.Add(typeFromHandle53, obj27 = new SimpleValueTargetProvider(array28, typeof(Setter).GetRuntimeProperty("Value"), nameScope47));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(176, 46)));
			DynamicResource dynamicResource27 = markupExtension27.ProvideValue(xamlServiceProvider27);
			setter46.Value = dynamicResource27;
			style12.Setters.Add(setter46);
			resourceDictionary2.Add(style12);
			setter47.Property = Slider.ThumbColorProperty;
			dynamicResourceExtension28.Key = "SwitchOnColor";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension28;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 4];
			array29[0] = setter47;
			array29[1] = style13;
			array29[2] = resourceDictionary2;
			array29[3] = this;
			object obj28;
			xamlServiceProvider28.Add(typeFromHandle55, obj28 = new SimpleValueTargetProvider(array29, typeof(Setter).GetRuntimeProperty("Value"), nameScope48));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(187, 47)));
			DynamicResource dynamicResource28 = markupExtension28.ProvideValue(xamlServiceProvider28);
			setter47.Value = dynamicResource28;
			style13.Setters.Add(setter47);
			setter48.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension29.Key = "SliderBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension29;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = setter48;
			array30[1] = style13;
			array30[2] = resourceDictionary2;
			array30[3] = this;
			object obj29;
			xamlServiceProvider29.Add(typeFromHandle57, obj29 = new SimpleValueTargetProvider(array30, typeof(Setter).GetRuntimeProperty("Value"), nameScope49));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 52)));
			DynamicResource dynamicResource29 = markupExtension29.ProvideValue(xamlServiceProvider29);
			setter48.Value = dynamicResource29;
			style13.Setters.Add(setter48);
			resourceDictionary2.Add(style13);
			setter49.Property = Slider.ThumbColorProperty;
			dynamicResourceExtension30.Key = "SwitchOnColor";
			IMarkupExtension<DynamicResource> markupExtension30 = dynamicResourceExtension30;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 4];
			array31[0] = setter49;
			array31[1] = style14;
			array31[2] = resourceDictionary2;
			array31[3] = this;
			object obj30;
			xamlServiceProvider30.Add(typeFromHandle59, obj30 = new SimpleValueTargetProvider(array31, typeof(Setter).GetRuntimeProperty("Value"), nameScope50));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 47)));
			DynamicResource dynamicResource30 = markupExtension30.ProvideValue(xamlServiceProvider30);
			setter49.Value = dynamicResource30;
			style14.Setters.Add(setter49);
			setter50.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension31.Key = "SliderBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension31 = dynamicResourceExtension31;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 4];
			array32[0] = setter50;
			array32[1] = style14;
			array32[2] = resourceDictionary2;
			array32[3] = this;
			object obj31;
			xamlServiceProvider31.Add(typeFromHandle61, obj31 = new SimpleValueTargetProvider(array32, typeof(Setter).GetRuntimeProperty("Value"), nameScope51));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(193, 52)));
			DynamicResource dynamicResource31 = markupExtension31.ProvideValue(xamlServiceProvider31);
			setter50.Value = dynamicResource31;
			style14.Setters.Add(setter50);
			resourceDictionary2.Add(style14);
			setter51.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension32.Key = "Gray";
			IMarkupExtension<DynamicResource> markupExtension32 = dynamicResourceExtension32;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 4];
			array33[0] = setter51;
			array33[1] = style15;
			array33[2] = resourceDictionary2;
			array33[3] = this;
			object obj32;
			xamlServiceProvider32.Add(typeFromHandle63, obj32 = new SimpleValueTargetProvider(array33, typeof(Setter).GetRuntimeProperty("Value"), nameScope52));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver32.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(198, 52)));
			DynamicResource dynamicResource32 = markupExtension32.ProvideValue(xamlServiceProvider32);
			setter51.Value = dynamicResource32;
			style15.Setters.Add(setter51);
			resourceDictionary2.Add(style15);
			setter52.Property = SearchBar.FontSizeProperty;
			dynamicResourceExtension33.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension33 = dynamicResourceExtension33;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = setter52;
			array34[1] = style16;
			array34[2] = resourceDictionary2;
			array34[3] = this;
			object obj33;
			xamlServiceProvider33.Add(typeFromHandle65, obj33 = new SimpleValueTargetProvider(array34, typeof(Setter).GetRuntimeProperty("Value"), nameScope53));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver33.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 45)));
			DynamicResource dynamicResource33 = markupExtension33.ProvideValue(xamlServiceProvider33);
			setter52.Value = dynamicResource33;
			style16.Setters.Add(setter52);
			setter53.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension34.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension34 = dynamicResourceExtension34;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 4];
			array35[0] = setter53;
			array35[1] = style16;
			array35[2] = resourceDictionary2;
			array35[3] = this;
			object obj34;
			xamlServiceProvider34.Add(typeFromHandle67, obj34 = new SimpleValueTargetProvider(array35, typeof(Setter).GetRuntimeProperty("Value"), nameScope54));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(204, 52)));
			DynamicResource dynamicResource34 = markupExtension34.ProvideValue(xamlServiceProvider34);
			setter53.Value = dynamicResource34;
			style16.Setters.Add(setter53);
			setter54.Property = SearchBar.PlaceholderColorProperty;
			dynamicResourceExtension35.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension35 = dynamicResourceExtension35;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = setter54;
			array36[1] = style16;
			array36[2] = resourceDictionary2;
			array36[3] = this;
			object obj35;
			xamlServiceProvider35.Add(typeFromHandle69, obj35 = new SimpleValueTargetProvider(array36, typeof(Setter).GetRuntimeProperty("Value"), nameScope55));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver35.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 53)));
			DynamicResource dynamicResource35 = markupExtension35.ProvideValue(xamlServiceProvider35);
			setter54.Value = dynamicResource35;
			style16.Setters.Add(setter54);
			setter55.Property = SearchBar.TextColorProperty;
			dynamicResourceExtension36.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension36 = dynamicResourceExtension36;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 4];
			array37[0] = setter55;
			array37[1] = style16;
			array37[2] = resourceDictionary2;
			array37[3] = this;
			object obj36;
			xamlServiceProvider36.Add(typeFromHandle71, obj36 = new SimpleValueTargetProvider(array37, typeof(Setter).GetRuntimeProperty("Value"), nameScope56));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver36.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(206, 46)));
			DynamicResource dynamicResource36 = markupExtension36.ProvideValue(xamlServiceProvider36);
			setter55.Value = dynamicResource36;
			style16.Setters.Add(setter55);
			resourceDictionary2.Add(style16);
			setter56.Property = VisualElement.BackgroundColorProperty;
			setter56.Value = "Transparent";
			setter56.Value = Color.Transparent;
			style17.Setters.Add(setter56);
			setter57.Property = ListView.SeparatorColorProperty;
			dynamicResourceExtension37.Key = "ListViewSeparatorColor";
			IMarkupExtension<DynamicResource> markupExtension37 = dynamicResourceExtension37;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = setter57;
			array38[1] = style17;
			array38[2] = resourceDictionary2;
			array38[3] = this;
			object obj37;
			xamlServiceProvider37.Add(typeFromHandle73, obj37 = new SimpleValueTargetProvider(array38, typeof(Setter).GetRuntimeProperty("Value"), nameScope58));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver37.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 51)));
			DynamicResource dynamicResource37 = markupExtension37.ProvideValue(xamlServiceProvider37);
			setter57.Value = dynamicResource37;
			style17.Setters.Add(setter57);
			resourceDictionary2.Add(style17);
			setter58.Property = ToggleButton.FontSizeProperty;
			dynamicResourceExtension38.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension38 = dynamicResourceExtension38;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 4];
			array39[0] = setter58;
			array39[1] = style18;
			array39[2] = resourceDictionary2;
			array39[3] = this;
			object obj38;
			xamlServiceProvider38.Add(typeFromHandle75, obj38 = new SimpleValueTargetProvider(array39, typeof(Setter).GetRuntimeProperty("Value"), nameScope59));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver38.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(217, 45)));
			DynamicResource dynamicResource38 = markupExtension38.ProvideValue(xamlServiceProvider38);
			setter58.Value = dynamicResource38;
			style18.Setters.Add(setter58);
			setter59.Property = ToggleButton.CheckedColorProperty;
			dynamicResourceExtension39.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension39 = dynamicResourceExtension39;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 4];
			array40[0] = setter59;
			array40[1] = style18;
			array40[2] = resourceDictionary2;
			array40[3] = this;
			object obj39;
			xamlServiceProvider39.Add(typeFromHandle77, obj39 = new SimpleValueTargetProvider(array40, typeof(Setter).GetRuntimeProperty("Value"), nameScope60));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver39.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(218, 49)));
			DynamicResource dynamicResource39 = markupExtension39.ProvideValue(xamlServiceProvider39);
			setter59.Value = dynamicResource39;
			style18.Setters.Add(setter59);
			setter60.Property = ToggleButton.UncheckedColorProperty;
			dynamicResourceExtension40.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension40 = dynamicResourceExtension40;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 4];
			array41[0] = setter60;
			array41[1] = style18;
			array41[2] = resourceDictionary2;
			array41[3] = this;
			object obj40;
			xamlServiceProvider40.Add(typeFromHandle79, obj40 = new SimpleValueTargetProvider(array41, typeof(Setter).GetRuntimeProperty("Value"), nameScope61));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver40.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(219, 51)));
			DynamicResource dynamicResource40 = markupExtension40.ProvideValue(xamlServiceProvider40);
			setter60.Value = dynamicResource40;
			style18.Setters.Add(setter60);
			setter61.Property = ToggleButton.TextColorProperty;
			dynamicResourceExtension41.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension41 = dynamicResourceExtension41;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 4];
			array42[0] = setter61;
			array42[1] = style18;
			array42[2] = resourceDictionary2;
			array42[3] = this;
			object obj41;
			xamlServiceProvider41.Add(typeFromHandle81, obj41 = new SimpleValueTargetProvider(array42, typeof(Setter).GetRuntimeProperty("Value"), nameScope62));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver41.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(220, 46)));
			DynamicResource dynamicResource41 = markupExtension41.ProvideValue(xamlServiceProvider41);
			setter61.Value = dynamicResource41;
			style18.Setters.Add(setter61);
			setter62.Property = ToggleButton.LineBreakModeProperty;
			setter62.Value = "WordWrap";
			setter62.Value = 1;
			style18.Setters.Add(setter62);
			resourceDictionary2.Add(style18);
			setter63.Property = RadioButton.FontSizeProperty;
			dynamicResourceExtension42.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension42 = dynamicResourceExtension42;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 4];
			array43[0] = setter63;
			array43[1] = style19;
			array43[2] = resourceDictionary2;
			array43[3] = this;
			object obj42;
			xamlServiceProvider42.Add(typeFromHandle83, obj42 = new SimpleValueTargetProvider(array43, typeof(Setter).GetRuntimeProperty("Value"), nameScope64));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver42.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(225, 45)));
			DynamicResource dynamicResource42 = markupExtension42.ProvideValue(xamlServiceProvider42);
			setter63.Value = dynamicResource42;
			style19.Setters.Add(setter63);
			setter64.Property = RadioButton.TextColorProperty;
			dynamicResourceExtension43.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension43 = dynamicResourceExtension43;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = setter64;
			array44[1] = style19;
			array44[2] = resourceDictionary2;
			array44[3] = this;
			object obj43;
			xamlServiceProvider43.Add(typeFromHandle85, obj43 = new SimpleValueTargetProvider(array44, typeof(Setter).GetRuntimeProperty("Value"), nameScope65));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver43.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(226, 46)));
			DynamicResource dynamicResource43 = markupExtension43.ProvideValue(xamlServiceProvider43);
			setter64.Value = dynamicResource43;
			style19.Setters.Add(setter64);
			setter65.Property = RadioButton.FontSizeProperty;
			dynamicResourceExtension44.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension44 = dynamicResourceExtension44;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 4];
			array45[0] = setter65;
			array45[1] = style19;
			array45[2] = resourceDictionary2;
			array45[3] = this;
			object obj44;
			xamlServiceProvider44.Add(typeFromHandle87, obj44 = new SimpleValueTargetProvider(array45, typeof(Setter).GetRuntimeProperty("Value"), nameScope66));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver44.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(227, 45)));
			DynamicResource dynamicResource44 = markupExtension44.ProvideValue(xamlServiceProvider44);
			setter65.Value = dynamicResource44;
			style19.Setters.Add(setter65);
			setter66.Property = RadioButton.FontAttributesProperty;
			setter66.Value = "None";
			setter66.Value = new FontAttributesConverter().ConvertFromInvariantString("None");
			style19.Setters.Add(setter66);
			resourceDictionary2.Add(style19);
			setter67.Property = ChartLineStyle.StrokeColorProperty;
			dynamicResourceExtension45.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension45 = dynamicResourceExtension45;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 4];
			array46[0] = setter67;
			array46[1] = style20;
			array46[2] = resourceDictionary2;
			array46[3] = this;
			object obj45;
			xamlServiceProvider45.Add(typeFromHandle89, obj45 = new SimpleValueTargetProvider(array46, typeof(Setter).GetRuntimeProperty("Value"), nameScope68));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver45.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(232, 48)));
			DynamicResource dynamicResource45 = markupExtension45.ProvideValue(xamlServiceProvider45);
			setter67.Value = dynamicResource45;
			style20.Setters.Add(setter67);
			resourceDictionary2.Add(style20);
			setter68.Property = ChartLabelStyle.TextColorProperty;
			dynamicResourceExtension46.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension46 = dynamicResourceExtension46;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 4];
			array47[0] = setter68;
			array47[1] = style21;
			array47[2] = resourceDictionary2;
			array47[3] = this;
			object obj46;
			xamlServiceProvider46.Add(typeFromHandle91, obj46 = new SimpleValueTargetProvider(array47, typeof(Setter).GetRuntimeProperty("Value"), nameScope69));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver46.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(235, 46)));
			DynamicResource dynamicResource46 = markupExtension46.ProvideValue(xamlServiceProvider46);
			setter68.Value = dynamicResource46;
			style21.Setters.Add(setter68);
			resourceDictionary2.Add(style21);
			setter69.Property = ChartLabelStyle.TextColorProperty;
			dynamicResourceExtension47.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension47 = dynamicResourceExtension47;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 4];
			array48[0] = setter69;
			array48[1] = style22;
			array48[2] = resourceDictionary2;
			array48[3] = this;
			object obj47;
			xamlServiceProvider47.Add(typeFromHandle93, obj47 = new SimpleValueTargetProvider(array48, typeof(Setter).GetRuntimeProperty("Value"), nameScope70));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver47.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(238, 46)));
			DynamicResource dynamicResource47 = markupExtension47.ProvideValue(xamlServiceProvider47);
			setter69.Value = dynamicResource47;
			style22.Setters.Add(setter69);
			resourceDictionary2.Add(style22);
			setter70.Property = TextCell.TextColorProperty;
			dynamicResourceExtension48.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension48 = dynamicResourceExtension48;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 4];
			array49[0] = setter70;
			array49[1] = style23;
			array49[2] = resourceDictionary2;
			array49[3] = this;
			object obj48;
			xamlServiceProvider48.Add(typeFromHandle95, obj48 = new SimpleValueTargetProvider(array49, typeof(Setter).GetRuntimeProperty("Value"), nameScope71));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver48.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(241, 46)));
			DynamicResource dynamicResource48 = markupExtension48.ProvideValue(xamlServiceProvider48);
			setter70.Value = dynamicResource48;
			style23.Setters.Add(setter70);
			setter71.Property = TextCell.DetailColorProperty;
			dynamicResourceExtension49.Key = "GrayedTextColor";
			IMarkupExtension<DynamicResource> markupExtension49 = dynamicResourceExtension49;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 4];
			array50[0] = setter71;
			array50[1] = style23;
			array50[2] = resourceDictionary2;
			array50[3] = this;
			object obj49;
			xamlServiceProvider49.Add(typeFromHandle97, obj49 = new SimpleValueTargetProvider(array50, typeof(Setter).GetRuntimeProperty("Value"), nameScope72));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver49.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(242, 48)));
			DynamicResource dynamicResource49 = markupExtension49.ProvideValue(xamlServiceProvider49);
			setter71.Value = dynamicResource49;
			style23.Setters.Add(setter71);
			resourceDictionary2.Add(style23);
			setter72.Property = SettingsView.SeparatorColorProperty;
			dynamicResourceExtension50.Key = "DisabledColor";
			IMarkupExtension<DynamicResource> markupExtension50 = dynamicResourceExtension50;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 4];
			array51[0] = setter72;
			array51[1] = style24;
			array51[2] = resourceDictionary2;
			array51[3] = this;
			object obj50;
			xamlServiceProvider50.Add(typeFromHandle99, obj50 = new SimpleValueTargetProvider(array51, typeof(Setter).GetRuntimeProperty("Value"), nameScope73));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver50.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver50.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(248, 51)));
			DynamicResource dynamicResource50 = markupExtension50.ProvideValue(xamlServiceProvider50);
			setter72.Value = dynamicResource50;
			style24.Setters.Add(setter72);
			setter73.Property = SettingsView.BackgroundColorProperty;
			dynamicResourceExtension51.Key = "SettingsBackground";
			IMarkupExtension<DynamicResource> markupExtension51 = dynamicResourceExtension51;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 4];
			array52[0] = setter73;
			array52[1] = style24;
			array52[2] = resourceDictionary2;
			array52[3] = this;
			object obj51;
			xamlServiceProvider51.Add(typeFromHandle101, obj51 = new SimpleValueTargetProvider(array52, typeof(Setter).GetRuntimeProperty("Value"), nameScope74));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver51.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver51.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(249, 52)));
			DynamicResource dynamicResource51 = markupExtension51.ProvideValue(xamlServiceProvider51);
			setter73.Value = dynamicResource51;
			style24.Setters.Add(setter73);
			setter74.Property = SettingsView.HeaderBackgroundColorProperty;
			dynamicResourceExtension52.Key = "SettingsBackground";
			IMarkupExtension<DynamicResource> markupExtension52 = dynamicResourceExtension52;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 4];
			array53[0] = setter74;
			array53[1] = style24;
			array53[2] = resourceDictionary2;
			array53[3] = this;
			object obj52;
			xamlServiceProvider52.Add(typeFromHandle103, obj52 = new SimpleValueTargetProvider(array53, typeof(Setter).GetRuntimeProperty("Value"), nameScope75));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver52.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver52.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver52.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider52.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver52, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(250, 58)));
			DynamicResource dynamicResource52 = markupExtension52.ProvideValue(xamlServiceProvider52);
			setter74.Value = dynamicResource52;
			style24.Setters.Add(setter74);
			setter75.Property = SettingsView.CellBackgroundColorProperty;
			dynamicResourceExtension53.Key = "SettingsCellBackground";
			IMarkupExtension<DynamicResource> markupExtension53 = dynamicResourceExtension53;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 4];
			array54[0] = setter75;
			array54[1] = style24;
			array54[2] = resourceDictionary2;
			array54[3] = this;
			object obj53;
			xamlServiceProvider53.Add(typeFromHandle105, obj53 = new SimpleValueTargetProvider(array54, typeof(Setter).GetRuntimeProperty("Value"), nameScope76));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver53.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver53.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver53.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider53.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver53, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(251, 56)));
			DynamicResource dynamicResource53 = markupExtension53.ProvideValue(xamlServiceProvider53);
			setter75.Value = dynamicResource53;
			style24.Setters.Add(setter75);
			setter76.Property = SettingsView.CellTitleColorProperty;
			dynamicResourceExtension54.Key = "SettingsCellTitleColor";
			IMarkupExtension<DynamicResource> markupExtension54 = dynamicResourceExtension54;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 4];
			array55[0] = setter76;
			array55[1] = style24;
			array55[2] = resourceDictionary2;
			array55[3] = this;
			object obj54;
			xamlServiceProvider54.Add(typeFromHandle107, obj54 = new SimpleValueTargetProvider(array55, typeof(Setter).GetRuntimeProperty("Value"), nameScope77));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver54.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver54.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver54.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider54.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver54, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(252, 51)));
			DynamicResource dynamicResource54 = markupExtension54.ProvideValue(xamlServiceProvider54);
			setter76.Value = dynamicResource54;
			style24.Setters.Add(setter76);
			setter77.Property = SettingsView.CellValueTextColorProperty;
			dynamicResourceExtension55.Key = "SettingsCellValueTextColor";
			IMarkupExtension<DynamicResource> markupExtension55 = dynamicResourceExtension55;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 4];
			array56[0] = setter77;
			array56[1] = style24;
			array56[2] = resourceDictionary2;
			array56[3] = this;
			object obj55;
			xamlServiceProvider55.Add(typeFromHandle109, obj55 = new SimpleValueTargetProvider(array56, typeof(Setter).GetRuntimeProperty("Value"), nameScope78));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver55.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver55.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver55.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider55.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver55, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(253, 55)));
			DynamicResource dynamicResource55 = markupExtension55.ProvideValue(xamlServiceProvider55);
			setter77.Value = dynamicResource55;
			style24.Setters.Add(setter77);
			setter78.Property = SettingsView.CellTitleFontSizeProperty;
			dynamicResourceExtension56.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension56 = dynamicResourceExtension56;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 4];
			array57[0] = setter78;
			array57[1] = style24;
			array57[2] = resourceDictionary2;
			array57[3] = this;
			object obj56;
			xamlServiceProvider56.Add(typeFromHandle111, obj56 = new SimpleValueTargetProvider(array57, typeof(Setter).GetRuntimeProperty("Value"), nameScope79));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver56.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver56.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver56.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider56.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver56, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(254, 54)));
			DynamicResource dynamicResource56 = markupExtension56.ProvideValue(xamlServiceProvider56);
			setter78.Value = dynamicResource56;
			style24.Setters.Add(setter78);
			setter79.Property = SettingsView.CellValueTextFontSizeProperty;
			dynamicResourceExtension57.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension57 = dynamicResourceExtension57;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle113 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 4];
			array58[0] = setter79;
			array58[1] = style24;
			array58[2] = resourceDictionary2;
			array58[3] = this;
			object obj57;
			xamlServiceProvider57.Add(typeFromHandle113, obj57 = new SimpleValueTargetProvider(array58, typeof(Setter).GetRuntimeProperty("Value"), nameScope80));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle114 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver57 = new XmlNamespaceResolver();
			xmlNamespaceResolver57.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver57.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver57.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver57.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver57.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver57.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver57.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider57.Add(typeFromHandle114, new XamlTypeResolver(xmlNamespaceResolver57, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(255, 58)));
			DynamicResource dynamicResource57 = markupExtension57.ProvideValue(xamlServiceProvider57);
			setter79.Value = dynamicResource57;
			style24.Setters.Add(setter79);
			setter80.Property = SettingsView.CellDescriptionColorProperty;
			dynamicResourceExtension58.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension58 = dynamicResourceExtension58;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle115 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 4];
			array59[0] = setter80;
			array59[1] = style24;
			array59[2] = resourceDictionary2;
			array59[3] = this;
			object obj58;
			xamlServiceProvider58.Add(typeFromHandle115, obj58 = new SimpleValueTargetProvider(array59, typeof(Setter).GetRuntimeProperty("Value"), nameScope81));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle116 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver58 = new XmlNamespaceResolver();
			xmlNamespaceResolver58.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver58.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver58.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver58.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver58.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver58.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider58.Add(typeFromHandle116, new XamlTypeResolver(xmlNamespaceResolver58, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(256, 57)));
			DynamicResource dynamicResource58 = markupExtension58.ProvideValue(xamlServiceProvider58);
			setter80.Value = dynamicResource58;
			style24.Setters.Add(setter80);
			setter81.Property = SettingsView.CellDescriptionFontSizeProperty;
			dynamicResourceExtension59.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension59 = dynamicResourceExtension59;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle117 = typeof(IProvideValueTarget);
			object[] array60 = new object[0 + 4];
			array60[0] = setter81;
			array60[1] = style24;
			array60[2] = resourceDictionary2;
			array60[3] = this;
			object obj59;
			xamlServiceProvider59.Add(typeFromHandle117, obj59 = new SimpleValueTargetProvider(array60, typeof(Setter).GetRuntimeProperty("Value"), nameScope82));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle118 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver59 = new XmlNamespaceResolver();
			xmlNamespaceResolver59.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver59.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver59.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver59.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver59.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver59.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver59.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver59.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver59.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider59.Add(typeFromHandle118, new XamlTypeResolver(xmlNamespaceResolver59, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(257, 60)));
			DynamicResource dynamicResource59 = markupExtension59.ProvideValue(xamlServiceProvider59);
			setter81.Value = dynamicResource59;
			style24.Setters.Add(setter81);
			setter82.Property = SettingsView.CellAccentColorProperty;
			dynamicResourceExtension60.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension60 = dynamicResourceExtension60;
			XamlServiceProvider xamlServiceProvider60 = new XamlServiceProvider();
			Type typeFromHandle119 = typeof(IProvideValueTarget);
			object[] array61 = new object[0 + 4];
			array61[0] = setter82;
			array61[1] = style24;
			array61[2] = resourceDictionary2;
			array61[3] = this;
			object obj60;
			xamlServiceProvider60.Add(typeFromHandle119, obj60 = new SimpleValueTargetProvider(array61, typeof(Setter).GetRuntimeProperty("Value"), nameScope83));
			xamlServiceProvider60.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle120 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver60 = new XmlNamespaceResolver();
			xmlNamespaceResolver60.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver60.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver60.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver60.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver60.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver60.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver60.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider60.Add(typeFromHandle120, new XamlTypeResolver(xmlNamespaceResolver60, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider60.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 52)));
			DynamicResource dynamicResource60 = markupExtension60.ProvideValue(xamlServiceProvider60);
			setter82.Value = dynamicResource60;
			style24.Setters.Add(setter82);
			setter83.Property = SettingsView.SelectedColorProperty;
			dynamicResourceExtension61.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension61 = dynamicResourceExtension61;
			XamlServiceProvider xamlServiceProvider61 = new XamlServiceProvider();
			Type typeFromHandle121 = typeof(IProvideValueTarget);
			object[] array62 = new object[0 + 4];
			array62[0] = setter83;
			array62[1] = style24;
			array62[2] = resourceDictionary2;
			array62[3] = this;
			object obj61;
			xamlServiceProvider61.Add(typeFromHandle121, obj61 = new SimpleValueTargetProvider(array62, typeof(Setter).GetRuntimeProperty("Value"), nameScope84));
			xamlServiceProvider61.Add(typeof(IReferenceProvider), obj61);
			Type typeFromHandle122 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver61 = new XmlNamespaceResolver();
			xmlNamespaceResolver61.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver61.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver61.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver61.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver61.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver61.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver61.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider61.Add(typeFromHandle122, new XamlTypeResolver(xmlNamespaceResolver61, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider61.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(259, 50)));
			DynamicResource dynamicResource61 = markupExtension61.ProvideValue(xamlServiceProvider61);
			setter83.Value = dynamicResource61;
			style24.Setters.Add(setter83);
			setter84.Property = SettingsView.HeaderTextColorProperty;
			dynamicResourceExtension62.Key = "SettingsHeaderTextColor";
			IMarkupExtension<DynamicResource> markupExtension62 = dynamicResourceExtension62;
			XamlServiceProvider xamlServiceProvider62 = new XamlServiceProvider();
			Type typeFromHandle123 = typeof(IProvideValueTarget);
			object[] array63 = new object[0 + 4];
			array63[0] = setter84;
			array63[1] = style24;
			array63[2] = resourceDictionary2;
			array63[3] = this;
			object obj62;
			xamlServiceProvider62.Add(typeFromHandle123, obj62 = new SimpleValueTargetProvider(array63, typeof(Setter).GetRuntimeProperty("Value"), nameScope85));
			xamlServiceProvider62.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle124 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver62 = new XmlNamespaceResolver();
			xmlNamespaceResolver62.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver62.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver62.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver62.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver62.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver62.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver62.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider62.Add(typeFromHandle124, new XamlTypeResolver(xmlNamespaceResolver62, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider62.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(260, 52)));
			DynamicResource dynamicResource62 = markupExtension62.ProvideValue(xamlServiceProvider62);
			setter84.Value = dynamicResource62;
			style24.Setters.Add(setter84);
			setter85.Property = SettingsView.HeaderFontSizeProperty;
			dynamicResourceExtension63.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension63 = dynamicResourceExtension63;
			XamlServiceProvider xamlServiceProvider63 = new XamlServiceProvider();
			Type typeFromHandle125 = typeof(IProvideValueTarget);
			object[] array64 = new object[0 + 4];
			array64[0] = setter85;
			array64[1] = style24;
			array64[2] = resourceDictionary2;
			array64[3] = this;
			object obj63;
			xamlServiceProvider63.Add(typeFromHandle125, obj63 = new SimpleValueTargetProvider(array64, typeof(Setter).GetRuntimeProperty("Value"), nameScope86));
			xamlServiceProvider63.Add(typeof(IReferenceProvider), obj63);
			Type typeFromHandle126 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver63 = new XmlNamespaceResolver();
			xmlNamespaceResolver63.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver63.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver63.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver63.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver63.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver63.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver63.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider63.Add(typeFromHandle126, new XamlTypeResolver(xmlNamespaceResolver63, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider63.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(261, 51)));
			DynamicResource dynamicResource63 = markupExtension63.ProvideValue(xamlServiceProvider63);
			setter85.Value = dynamicResource63;
			style24.Setters.Add(setter85);
			setter86.Property = SettingsView.FooterFontSizeProperty;
			dynamicResourceExtension64.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension64 = dynamicResourceExtension64;
			XamlServiceProvider xamlServiceProvider64 = new XamlServiceProvider();
			Type typeFromHandle127 = typeof(IProvideValueTarget);
			object[] array65 = new object[0 + 4];
			array65[0] = setter86;
			array65[1] = style24;
			array65[2] = resourceDictionary2;
			array65[3] = this;
			object obj64;
			xamlServiceProvider64.Add(typeFromHandle127, obj64 = new SimpleValueTargetProvider(array65, typeof(Setter).GetRuntimeProperty("Value"), nameScope87));
			xamlServiceProvider64.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle128 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver64 = new XmlNamespaceResolver();
			xmlNamespaceResolver64.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver64.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver64.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver64.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver64.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver64.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver64.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider64.Add(typeFromHandle128, new XamlTypeResolver(xmlNamespaceResolver64, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider64.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(262, 51)));
			DynamicResource dynamicResource64 = markupExtension64.ProvideValue(xamlServiceProvider64);
			setter86.Value = dynamicResource64;
			style24.Setters.Add(setter86);
			setter87.Property = SettingsView.FooterTextColorProperty;
			dynamicResourceExtension65.Key = "SettingsFooterTextColor";
			IMarkupExtension<DynamicResource> markupExtension65 = dynamicResourceExtension65;
			XamlServiceProvider xamlServiceProvider65 = new XamlServiceProvider();
			Type typeFromHandle129 = typeof(IProvideValueTarget);
			object[] array66 = new object[0 + 4];
			array66[0] = setter87;
			array66[1] = style24;
			array66[2] = resourceDictionary2;
			array66[3] = this;
			object obj65;
			xamlServiceProvider65.Add(typeFromHandle129, obj65 = new SimpleValueTargetProvider(array66, typeof(Setter).GetRuntimeProperty("Value"), nameScope88));
			xamlServiceProvider65.Add(typeof(IReferenceProvider), obj65);
			Type typeFromHandle130 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver65 = new XmlNamespaceResolver();
			xmlNamespaceResolver65.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver65.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver65.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver65.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver65.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver65.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver65.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider65.Add(typeFromHandle130, new XamlTypeResolver(xmlNamespaceResolver65, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider65.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(263, 52)));
			DynamicResource dynamicResource65 = markupExtension65.ProvideValue(xamlServiceProvider65);
			setter87.Value = dynamicResource65;
			style24.Setters.Add(setter87);
			setter88.Property = SettingsView.CellHintTextColorProperty;
			dynamicResourceExtension66.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension66 = dynamicResourceExtension66;
			XamlServiceProvider xamlServiceProvider66 = new XamlServiceProvider();
			Type typeFromHandle131 = typeof(IProvideValueTarget);
			object[] array67 = new object[0 + 4];
			array67[0] = setter88;
			array67[1] = style24;
			array67[2] = resourceDictionary2;
			array67[3] = this;
			object obj66;
			xamlServiceProvider66.Add(typeFromHandle131, obj66 = new SimpleValueTargetProvider(array67, typeof(Setter).GetRuntimeProperty("Value"), nameScope89));
			xamlServiceProvider66.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle132 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver66 = new XmlNamespaceResolver();
			xmlNamespaceResolver66.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver66.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver66.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver66.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver66.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver66.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver66.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider66.Add(typeFromHandle132, new XamlTypeResolver(xmlNamespaceResolver66, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider66.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(264, 54)));
			DynamicResource dynamicResource66 = markupExtension66.ProvideValue(xamlServiceProvider66);
			setter88.Value = dynamicResource66;
			style24.Setters.Add(setter88);
			resourceDictionary2.Add(style24);
			setter89.Property = CellBase.TitleColorProperty;
			dynamicResourceExtension67.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension67 = dynamicResourceExtension67;
			XamlServiceProvider xamlServiceProvider67 = new XamlServiceProvider();
			Type typeFromHandle133 = typeof(IProvideValueTarget);
			object[] array68 = new object[0 + 4];
			array68[0] = setter89;
			array68[1] = style25;
			array68[2] = resourceDictionary2;
			array68[3] = this;
			object obj67;
			xamlServiceProvider67.Add(typeFromHandle133, obj67 = new SimpleValueTargetProvider(array68, typeof(Setter).GetRuntimeProperty("Value"), nameScope90));
			xamlServiceProvider67.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle134 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver67 = new XmlNamespaceResolver();
			xmlNamespaceResolver67.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver67.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver67.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver67.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver67.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver67.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver67.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider67.Add(typeFromHandle134, new XamlTypeResolver(xmlNamespaceResolver67, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider67.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(268, 47)));
			DynamicResource dynamicResource67 = markupExtension67.ProvideValue(xamlServiceProvider67);
			setter89.Value = dynamicResource67;
			style25.Setters.Add(setter89);
			resourceDictionary2.Add(style25);
			setter90.Property = Label.FontSizeProperty;
			dynamicResourceExtension68.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension68 = dynamicResourceExtension68;
			XamlServiceProvider xamlServiceProvider68 = new XamlServiceProvider();
			Type typeFromHandle135 = typeof(IProvideValueTarget);
			object[] array69 = new object[0 + 4];
			array69[0] = setter90;
			array69[1] = style26;
			array69[2] = resourceDictionary2;
			array69[3] = this;
			object obj68;
			xamlServiceProvider68.Add(typeFromHandle135, obj68 = new SimpleValueTargetProvider(array69, typeof(Setter).GetRuntimeProperty("Value"), nameScope91));
			xamlServiceProvider68.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle136 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver68 = new XmlNamespaceResolver();
			xmlNamespaceResolver68.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver68.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver68.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver68.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver68.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver68.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver68.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider68.Add(typeFromHandle136, new XamlTypeResolver(xmlNamespaceResolver68, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider68.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(272, 45)));
			DynamicResource dynamicResource68 = markupExtension68.ProvideValue(xamlServiceProvider68);
			setter90.Value = dynamicResource68;
			style26.Setters.Add(setter90);
			setter91.Property = View.HorizontalOptionsProperty;
			setter91.Value = "Center";
			setter91.Value = LayoutOptions.Center;
			style26.Setters.Add(setter91);
			setter92.Property = Label.HorizontalTextAlignmentProperty;
			setter92.Value = "Center";
			setter92.Value = new TextAlignmentConverter().ConvertFromInvariantString("Center");
			style26.Setters.Add(setter92);
			setter93.Property = View.VerticalOptionsProperty;
			setter93.Value = "Center";
			setter93.Value = LayoutOptions.Center;
			style26.Setters.Add(setter93);
			setter94.Property = Label.VerticalTextAlignmentProperty;
			setter94.Value = "Center";
			setter94.Value = new TextAlignmentConverter().ConvertFromInvariantString("Center");
			style26.Setters.Add(setter94);
			setter95.Property = Label.TextColorProperty;
			dynamicResourceExtension69.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension69 = dynamicResourceExtension69;
			XamlServiceProvider xamlServiceProvider69 = new XamlServiceProvider();
			Type typeFromHandle137 = typeof(IProvideValueTarget);
			object[] array70 = new object[0 + 4];
			array70[0] = setter95;
			array70[1] = style26;
			array70[2] = resourceDictionary2;
			array70[3] = this;
			object obj69;
			xamlServiceProvider69.Add(typeFromHandle137, obj69 = new SimpleValueTargetProvider(array70, typeof(Setter).GetRuntimeProperty("Value"), nameScope96));
			xamlServiceProvider69.Add(typeof(IReferenceProvider), obj69);
			Type typeFromHandle138 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver69 = new XmlNamespaceResolver();
			xmlNamespaceResolver69.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver69.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver69.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver69.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver69.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver69.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver69.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider69.Add(typeFromHandle138, new XamlTypeResolver(xmlNamespaceResolver69, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider69.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(277, 46)));
			DynamicResource dynamicResource69 = markupExtension69.ProvideValue(xamlServiceProvider69);
			setter95.Value = dynamicResource69;
			style26.Setters.Add(setter95);
			resourceDictionary2.Add("SettingsValueAccentLabel", style26);
			setter96.Property = Entry.TextColorProperty;
			setter96.Value = "Red";
			setter96.Value = Color.Red;
			style27.Setters.Add(setter96);
			resourceDictionary2.Add("InvalidEntryStyle", style27);
			chartLineStyle.SetValue(ChartLineStyle.StrokeColorProperty, new Color(0.5647059082984924, 0.5647059082984924, 0.5647059082984924, 1.0));
			resourceDictionary2.Add("DefaultChartGirdLineStyle", chartLineStyle);
			chartAxisTickStyle.SetValue(ChartAxisTickStyle.StrokeColorProperty, new Color(0.5647059082984924, 0.5647059082984924, 0.5647059082984924, 1.0));
			resourceDictionary2.Add("DefaultChartGridTickStyle", chartAxisTickStyle);
			setter97.Property = CheckBox.ColorProperty;
			dynamicResourceExtension70.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension70 = dynamicResourceExtension70;
			XamlServiceProvider xamlServiceProvider70 = new XamlServiceProvider();
			Type typeFromHandle139 = typeof(IProvideValueTarget);
			object[] array71 = new object[0 + 4];
			array71[0] = setter97;
			array71[1] = style28;
			array71[2] = resourceDictionary2;
			array71[3] = this;
			object obj70;
			xamlServiceProvider70.Add(typeFromHandle139, obj70 = new SimpleValueTargetProvider(array71, typeof(Setter).GetRuntimeProperty("Value"), nameScope98));
			xamlServiceProvider70.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle140 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver70 = new XmlNamespaceResolver();
			xmlNamespaceResolver70.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver70.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver70.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver70.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver70.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver70.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver70.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider70.Add(typeFromHandle140, new XamlTypeResolver(xmlNamespaceResolver70, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider70.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(290, 42)));
			DynamicResource dynamicResource70 = markupExtension70.ProvideValue(xamlServiceProvider70);
			setter97.Value = dynamicResource70;
			style28.Setters.Add(setter97);
			resourceDictionary2.Add(style28);
			setter98.Property = CheckBoxWithLabel.CheckColorProperty;
			dynamicResourceExtension71.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension71 = dynamicResourceExtension71;
			XamlServiceProvider xamlServiceProvider71 = new XamlServiceProvider();
			Type typeFromHandle141 = typeof(IProvideValueTarget);
			object[] array72 = new object[0 + 4];
			array72[0] = setter98;
			array72[1] = style29;
			array72[2] = resourceDictionary2;
			array72[3] = this;
			object obj71;
			xamlServiceProvider71.Add(typeFromHandle141, obj71 = new SimpleValueTargetProvider(array72, typeof(Setter).GetRuntimeProperty("Value"), nameScope99));
			xamlServiceProvider71.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle142 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver71 = new XmlNamespaceResolver();
			xmlNamespaceResolver71.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver71.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver71.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver71.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver71.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver71.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver71.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider71.Add(typeFromHandle142, new XamlTypeResolver(xmlNamespaceResolver71, typeof(App).GetTypeInfo().Assembly));
			xamlServiceProvider71.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(294, 47)));
			DynamicResource dynamicResource71 = markupExtension71.ProvideValue(xamlServiceProvider71);
			setter98.Value = dynamicResource71;
			style29.Setters.Add(setter98);
			resourceDictionary2.Add(style29);
			solidColorBrush.SetValue(SolidColorBrush.ColorProperty, Color.Red);
			resourceDictionary2.Add("RadioButtonCheckMarkThemeColor", solidColorBrush);
			solidColorBrush2.SetValue(SolidColorBrush.ColorProperty, Color.Green);
			resourceDictionary2.Add("RadioButtonThemeColor", solidColorBrush2);
			this.SetValue(Application.EnableAccessibilityScalingForNamedFontSizesProperty, false);
			this.SetValue(Application.HandleControlUpdatesOnMainThreadProperty, true);
			this.Resources = resourceDictionary2;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00015914 File Offset: 0x00013B14
		// Note: this type is marked as 'beforefieldinit'.
		static App()
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00015932 File Offset: 0x00013B32
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<App>(this, typeof(App));
		}

		// Token: 0x04000175 RID: 373
		[CompilerGenerated]
		private RootNavigationPage <RootPage>k__BackingField;

		// Token: 0x04000176 RID: 374
		[CompilerGenerated]
		private static bool <UseLegacyUI>k__BackingField;

		// Token: 0x04000177 RID: 375
		public static OBDDataReader OBDReader = null;

		// Token: 0x04000178 RID: 376
		[CompilerGenerated]
		private static App <Instance>k__BackingField;

		// Token: 0x04000179 RID: 377
		internal static Dictionary<string, double> DefaultFontSizes = new Dictionary<string, double>();

		// Token: 0x0400017A RID: 378
		public static int OriginalNavBarColor = 0;

		// Token: 0x02000046 RID: 70
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060001C7 RID: 455 RVA: 0x00015945 File Offset: 0x00013B45
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060001C8 RID: 456 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060001C9 RID: 457 RVA: 0x00015951 File Offset: 0x00013B51
			internal void <ChangeLanguage>b__26_0()
			{
				if (App.OBDReader == null)
				{
					App.OBDReader = new OBDDataReader();
				}
				OBDReaderSimulator.Reload();
			}

			// Token: 0x060001CA RID: 458 RVA: 0x0001596C File Offset: 0x00013B6C
			internal void <PreventLinkerFromStrippingCommonLocalizationReferences>b__40_0()
			{
				try
				{
					new ChineseLunisolarCalendar();
					new GregorianCalendar();
					new HebrewCalendar();
					new HijriCalendar();
					new JapaneseCalendar();
					new JapaneseLunisolarCalendar();
					new JulianCalendar();
					new KoreanCalendar();
					new KoreanLunisolarCalendar();
					new PersianCalendar();
					new TaiwanCalendar();
					new TaiwanLunisolarCalendar();
					new ThaiBuddhistCalendar();
					new UmAlQuraCalendar();
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x0400017B RID: 379
			public static readonly App.<>c <>9 = new App.<>c();

			// Token: 0x0400017C RID: 380
			public static Action <>9__26_0;

			// Token: 0x0400017D RID: 381
			public static Action <>9__40_0;
		}

		// Token: 0x02000047 RID: 71
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_0
		{
			// Token: 0x060001CB RID: 459 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_0()
			{
			}

			// Token: 0x060001CC RID: 460 RVA: 0x000159E4 File Offset: 0x00013BE4
			internal void <CheckRP_IOS>b__0()
			{
				SharedSettings.Current.LastTimeLicenceChecked = this.dtTicks;
				PlatformHelper.IOSService.StoreReceiptParser_LoadReceipt();
			}

			// Token: 0x0400017E RID: 382
			public long dtTicks;
		}

		// Token: 0x02000048 RID: 72
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ChangeLanguage>d__26 : IAsyncStateMachine
		{
			// Token: 0x060001CD RID: 461 RVA: 0x00015A00 File Offset: 0x00013C00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						Language.SetLanguage();
						taskAwaiter = Task.Run(delegate
						{
							if (App.OBDReader == null)
							{
								App.OBDReader = new OBDDataReader();
							}
							OBDReaderSimulator.Reload();
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, App.<ChangeLanguage>d__26>(ref taskAwaiter, ref this);
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
					RootNavigationPage.ChangeRootPage(new SimpleMainPage());
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

			// Token: 0x060001CE RID: 462 RVA: 0x00015AD8 File Offset: 0x00013CD8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400017F RID: 383
			public int <>1__state;

			// Token: 0x04000180 RID: 384
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000181 RID: 385
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000049 RID: 73
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckRP>d__37 : IAsyncStateMachine
		{
			// Token: 0x060001CF RID: 463 RVA: 0x00015AE8 File Offset: 0x00013CE8
			void IAsyncStateMachine.MoveNext()
			{
				App app = this;
				try
				{
					if (PlatformHelper.IsAndroid)
					{
						app.CheckRP_DROID();
					}
					else if (PlatformHelper.IsiOS)
					{
						app.CheckRP_IOS();
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

			// Token: 0x060001D0 RID: 464 RVA: 0x00015B54 File Offset: 0x00013D54
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000182 RID: 386
			public int <>1__state;

			// Token: 0x04000183 RID: 387
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000184 RID: 388
			public App <>4__this;
		}

		// Token: 0x0200004A RID: 74
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckRP_DROID>d__38 : IAsyncStateMachine
		{
			// Token: 0x060001D1 RID: 465 RVA: 0x00015B64 File Offset: 0x00013D64
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.DeveloperMode)
						{
							goto IL_01D7;
						}
						taskAwaiter = Task.Delay(5000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, App.<CheckRP_DROID>d__38>(ref taskAwaiter, ref this);
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
					if (PlatformHelper.AppMarket != Markets.Sideload)
					{
						if (PlatformHelper.AppMarket == Markets.RUS)
						{
							if (SharedSettings.Current.AdsProductPurchased || (!string.IsNullOrEmpty(SharedSettings.Current.PurchaseOrderId) && SharedSettings.Current.PurchaseOrderId.Length == 27))
							{
								if (SharedSettings.Current.PurchaseToken != PlatformHelper.DroidService.DevicePermanentID_DeviceID)
								{
									SharedSettings.Current.AdsProductPurchased = false;
									SharedSettings.Current.PurchaseOrderId = "";
									SharedSettings.Current.PurchaseProductId = "";
									SharedSettings.Current.PurchaseToken = "";
								}
								else
								{
									long ticks = DateTimeNowHelper.NowSafe.Ticks;
									if (new TimeSpan(ticks - SharedSettings.Current.LastTimeLicenceChecked) >= SharedSettings.Current.LicenseCheckPeriod || SharedSettings.Current.LastTimeLicenceChecked > ticks)
									{
										PlatformHelper.DroidService.Droid_InApp_RuKeyActivator_CheckKeyAsync(SharedSettings.Current.PurchaseOrderId, false);
									}
								}
							}
						}
						else if (SharedSettings.Current.WhitelistDeviceActivated)
						{
							if (string.IsNullOrEmpty(SharedSettings.Current.WhitelistDeviceSN))
							{
								SharedSettings.Current.AdsProductPurchased = false;
								InAppManager.GetInstance().Restore();
							}
						}
						else
						{
							InAppManager.GetInstance().Restore();
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01D7:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x00015D78 File Offset: 0x00013F78
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000185 RID: 389
			public int <>1__state;

			// Token: 0x04000186 RID: 390
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000187 RID: 391
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200004B RID: 75
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnSleep>d__28 : IAsyncStateMachine
		{
			// Token: 0x060001D3 RID: 467 RVA: 0x00015D88 File Offset: 0x00013F88
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
					{
						PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnSleep:Start]\r\n");
					}
					SharedSettings.Current.LastException = "";
					DriveCycle.SaveAndReset();
					App.SaveCurrentData();
					if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.TraceLogs)
					{
						PCLDebugStream.CurrentInstance.WriteWithFlush("\r\n[" + DateTimeNowHelper.NowSafe.ToString() + " App.OnSleep:Finished]\r\n");
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

			// Token: 0x060001D4 RID: 468 RVA: 0x00015E68 File Offset: 0x00014068
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000188 RID: 392
			public int <>1__state;

			// Token: 0x04000189 RID: 393
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x0200004C RID: 76
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_0
		{
			// Token: 0x060001D5 RID: 469 RVA: 0x00015E78 File Offset: 0x00014078
			public <InitializeComponent>_anonXamlCDataTemplate_0()
			{
			}

			// Token: 0x060001D6 RID: 470 RVA: 0x00015E8C File Offset: 0x0001408C
			internal object LoadDataTemplate()
			{
				Setter setter;
				VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 42);
				VisualState visualState;
				VisualDiagnostics.RegisterSourceInfo(visualState = new VisualState(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 34);
				Setter setter2;
				VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 42);
				VisualState visualState2;
				VisualDiagnostics.RegisterSourceInfo(visualState2 = new VisualState(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 34);
				VisualStateGroup visualStateGroup;
				VisualDiagnostics.RegisterSourceInfo(visualStateGroup = new VisualStateGroup(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 30);
				VisualStateGroupList visualStateGroupList;
				VisualDiagnostics.RegisterSourceInfo(visualStateGroupList = new VisualStateGroupList(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 26);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 29);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 29);
				Ellipse ellipse;
				VisualDiagnostics.RegisterSourceInfo(ellipse = new Ellipse(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 26);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 29);
				Ellipse ellipse2;
				VisualDiagnostics.RegisterSourceInfo(ellipse2 = new Ellipse(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 22);
				ContentPresenter contentPresenter;
				VisualDiagnostics.RegisterSourceInfo(contentPresenter = new ContentPresenter(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 22);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("App.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid2, nameScope);
				NameScope nameScope2;
				(nameScope2 = new NameScope()).RegisterName("CheckedStates", visualStateGroup);
				nameScope2.RegisterName("Checked", visualState);
				nameScope2.RegisterName("Unchecked", visualState2);
				nameScope.RegisterName("check", ellipse2);
				if (ellipse2.StyleId == null)
				{
					ellipse2.StyleId = "check";
				}
				grid2.SetValue(View.MarginProperty, new Thickness(4.0));
				grid2.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("Auto,Auto"));
				visualStateGroup.Name = "CheckedStates";
				visualState.Name = "Checked";
				setter.TargetName = "check";
				setter.Property = VisualElement.OpacityProperty;
				setter.Value = "1";
				setter.Value = 1.0;
				visualState.Setters.Add(setter);
				visualStateGroup.States.Add(visualState);
				visualState2.Name = "Unchecked";
				setter2.TargetName = "check";
				setter2.Property = VisualElement.OpacityProperty;
				setter2.Value = "0";
				setter2.Value = 0.0;
				visualState2.Setters.Add(setter2);
				visualStateGroup.States.Add(visualState2);
				visualStateGroupList.Add(visualStateGroup);
				grid2.SetValue(VisualStateManager.VisualStateGroupsProperty, visualStateGroupList);
				grid.SetValue(Grid.ColumnProperty, 0);
				grid.SetValue(VisualElement.HeightRequestProperty, 21.0);
				grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
				grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
				grid.SetValue(VisualElement.WidthRequestProperty, 21.0);
				dynamicResourceExtension.Key = "BackgroundColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = ellipse;
				array2[1] = grid;
				array2[2] = grid2;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Shape.FillProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(App.<InitializeComponent>_anonXamlCDataTemplate_0).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				ellipse.SetDynamicResource(Shape.FillProperty, dynamicResource.Key);
				ellipse.SetValue(VisualElement.HeightRequestProperty, 21.0);
				ellipse.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				dynamicResourceExtension2.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = ellipse;
				array4[1] = grid;
				array4[2] = grid2;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Shape.StrokeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(App.<InitializeComponent>_anonXamlCDataTemplate_0).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 29)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				ellipse.SetDynamicResource(Shape.StrokeProperty, dynamicResource2.Key);
				ellipse.SetValue(Shape.StrokeThicknessProperty, 2.0);
				ellipse.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				ellipse.SetValue(VisualElement.WidthRequestProperty, 20.0);
				grid.Children.Add(ellipse);
				dynamicResourceExtension3.Key = "ButtonAccentColor";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array5, 3, num3);
				object[] array6 = array5;
				array6[0] = ellipse2;
				array6[1] = grid;
				array6[2] = grid2;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Shape.FillProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("android", "clr-namespace:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(App.<InitializeComponent>_anonXamlCDataTemplate_0).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 29)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				ellipse2.SetDynamicResource(Shape.FillProperty, dynamicResource3.Key);
				ellipse2.SetValue(VisualElement.HeightRequestProperty, 12.0);
				ellipse2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				ellipse2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				ellipse2.SetValue(VisualElement.WidthRequestProperty, 12.0);
				grid.Children.Add(ellipse2);
				grid2.Children.Add(grid);
				contentPresenter.SetValue(Grid.ColumnProperty, 1);
				grid2.Children.Add(contentPresenter);
				return grid2;
			}

			// Token: 0x0400018A RID: 394
			internal object[] parentValues;

			// Token: 0x0400018B RID: 395
			internal App root;
		}
	}
}
