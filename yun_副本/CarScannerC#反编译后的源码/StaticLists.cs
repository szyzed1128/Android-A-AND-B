using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.DTCv2;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.Settings.SettingsV3;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000056 RID: 86
	public static class StaticLists
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00016B48 File Offset: 0x00014D48
		public static List<string> Numbers0_300
		{
			get
			{
				if (StaticLists._Numbers0_300 == null)
				{
					List<string> list = new List<string>(301);
					for (int i = 0; i <= 300; i++)
					{
						list.Add(i.ToString());
					}
					StaticLists._Numbers0_300 = list;
				}
				return StaticLists._Numbers0_300;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00016B90 File Offset: 0x00014D90
		public static List<string> ConnectionPickerItems
		{
			get
			{
				if (PlatformHelper.IsAndroid)
				{
					return new List<string> { "Wi-Fi", "Bluetooth LE", "Bluetooth" };
				}
				if (PlatformHelper.IsiOS)
				{
					return new List<string> { "Wi-Fi", "Bluetooth LE (4.0)", "OBDLink MX+ MFI" };
				}
				throw new NotImplementedException("Platform Unknown=" + Device.RuntimePlatform);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00016C10 File Offset: 0x00014E10
		public static List<string> AndroidConnectionMethods
		{
			get
			{
				return new List<string>
				{
					Translate.GetString("droid_BT_Method_Auto"),
					"1",
					"2",
					"3",
					"4"
				};
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00016C5E File Offset: 0x00014E5E
		public static List<string> ChartViewSelector
		{
			get
			{
				return new List<string>(3)
				{
					Translate.GetString("Settings_Control_tbChartsMode_Ask"),
					Translate.GetString("ios_LiveDataMode_Combined"),
					Translate.GetString("ios_LiveDataMode_Separate")
				};
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00016C96 File Offset: 0x00014E96
		public static List<string> FuelConsumptionUnits
		{
			get
			{
				return new List<string>(3)
				{
					UnitsHelper.GetCaptionInvariant(UnitsHelper.Units.liters100km),
					UnitsHelper.GetCaptionInvariant(UnitsHelper.Units.km_liter),
					UnitsHelper.GetCaptionInvariant(UnitsHelper.Units.MPG)
				};
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00016CC5 File Offset: 0x00014EC5
		public static List<string> VolumeUnits
		{
			get
			{
				return new List<string>(3)
				{
					Translate.GetString("ios_Gallons"),
					Translate.GetString("ios_Liters")
				};
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00016CF0 File Offset: 0x00014EF0
		public static List<string> DoubleFormats
		{
			get
			{
				if (StaticLists._DoubleFormats == null)
				{
					StaticLists._DoubleFormats = new List<string>(9);
					StaticLists._DoubleFormats.Add("0");
					StaticLists._DoubleFormats.Add("0.#");
					StaticLists._DoubleFormats.Add("0.##");
					StaticLists._DoubleFormats.Add("0.###");
					StaticLists._DoubleFormats.Add("0.####");
					StaticLists._DoubleFormats.Add("0.0");
					StaticLists._DoubleFormats.Add("0.00");
					StaticLists._DoubleFormats.Add("0.000");
					StaticLists._DoubleFormats.Add("0.0000");
				}
				return StaticLists._DoubleFormats;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00016DA0 File Offset: 0x00014FA0
		public static List<string> Protocols
		{
			get
			{
				return new List<string>(48)
				{
					Translate.GetString("ios_Protocol_Automatic"),
					"1) SAE J1850 PWM (41.6 kbaud)",
					"2) SAE J1850 VPW (10.4 kbaud)",
					"3) ISO 9141-2 (5 baud init, 10.4 kbaud)",
					"4) ISO 14230-4 KWP (5 baud init, 10.4 kbaud)",
					"5) ISO 14230-4 KWP (fast baud init, 10.4 kbaud)",
					"6) ISO 15765-4 CAN (11 bit ID, 500 kbaud)",
					"7) ISO 15765-4 CAN (29 bit ID, 500 kbaud)",
					"8) ISO 15765-4 CAN (11 bit ID, 250 kbaud)",
					"9) ISO 15765-4 CAN (29 bit ID, 250 kbaud)",
					"10) SAE J1939 CAN (11 bit ID, 125 kbaud)",
					"11) Nissan Consult II (experimental support)",
					"12) ISO 14230-4 KWP (fast baud init, 9.6 kbaud)",
					"13) ISO 14230-4 KWP (fast baud init, 4.8 kbaud)",
					"14) ISO 14230-4 KWP (fast baud init 0x7A, 4.8 kbaud)",
					"15) ISO 14230-4 KWP (fast baud init 0x13, 4.8 kbaud)",
					"16) ISO 14230-4 KWP (fast baud init 0x33, 48 kbaud)",
					"17) ISO 14230-4 KWP (5 baud init, 9.6 kbaud)",
					"18) ISO 14230-4 KWP (5 baud init, 4.8 kbaud)",
					"19) ISO 14230-4 KWP (5 baud init 0x7A, 4.8 kbaud)",
					"20) ISO 14230-4 KWP (5 baud init 0x13, 4.8 kbaud)",
					"21) ISO 14230-4 KWP (5 baud init 0x33, 4.8 kbaud)",
					"22) ISO 9141-2 (5 baud init, 9.6 kbaud)",
					"23) ISO 9141-2 (5 baud init, 4.8 kbaud)",
					"24) ISO 9141-2 (5 baud init 0x7A, 4.8 kbaud)",
					"25) ISO 9141-2 (5 baud init 0x13, 4.8 kbaud)",
					"26) ISO 9141-2 (5 baud init 0x33, 4.8 kbaud)",
					"27) ISO 14230-4 KWP (fast baud init 0x13F1, 10.4 kbaud)",
					"28) ISO 14230-4 KWP (fast baud init 0x8013F0, 9.6 kbaud)",
					"29) ISO 14230-4 KWP (fast baud init 0x8213F0, 9.6 kbaud)",
					"30) ISO 14230-4 KWP (fast baud init 0x8013FC, 10.4 kbaud)",
					"31) ISO 14230-4 KWP (fast baud init 0x8013FC, 9.6 kbaud)",
					"32) ISO 14230-4 KWP (5 baud init 0x8013F1, 10.4 kbaud)",
					"33) ISO 14230-4 KWP (5 baud init 0x8013F0, 9.6 kbaud)",
					"34) ISO 14230-4 KWP (5 baud init 0x8213F0, 9.6 kbaud)",
					"35) ISO 14230-4 KWP (5 baud init 0x8013FC, 10.4 kbaud)",
					"36) ISO 14230-4 KWP (5 baud init 0x8013F1, 9.6 kbaud)",
					"37) ISO 14230-4 KWP (5 baud init 0x8113F1, 9.6 kbaud)",
					"38) ISO 14230-4 KWP (fast init 0x8110FC, 10.4 kbaud)",
					"39) ISO 14230-4 KWP (5 baud init 0x8013F1, 10.4 kbaud)",
					"40) ISO 14230-4 KWP (fast init 0x8113F1, 9.6 kbaud)",
					"41) ISO 14230-4 KWP (fast init 0x8213F1, 9.6 kbaud)",
					"42) ISO ISO 9141-2 (0x686AF1, 10.4 kbaud)",
					"43) Nissan Consult III (experimental)",
					"44) ISO 14230-4 KWP (fast baud init 0x8110F1, 10.4 kbaud)",
					"45) ISO 15765-4 CAN (11 bit ID, 500 kbaud, with flow control)"
				};
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00016FB3 File Offset: 0x000151B3
		public static List<string> AdaptiveTimingsList
		{
			get
			{
				return new List<string>(3) { "OFF", "AUTO1 (default)", "AUTO2" };
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00016FDC File Offset: 0x000151DC
		public static List<string> ATSTList
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(Translate.GetString("Settings_Control_ATST_OFF.Content"));
				list.Add(Translate.GetString("Settings_Control_ATST_0.Content"));
				list.Add(Translate.GetString("Settings_Control_ATST_8.Content"));
				list.Add(Translate.GetString("Settings_Control_ATST_16.Content"));
				list.Add(Translate.GetString("Settings_Control_ATST_32.Content"));
				list.Add(Translate.GetString("Settings_Control_ATST_48.Content"));
				list.Add(Translate.GetString("Settings_Control_ATST_64.Content"));
				list.Add(Translate.GetString("Settings_Control_ATST_96.Content"));
				string text = Translate.GetString("Settings_Control_ATST_96.Content");
				text = text.Replace("96", "FF");
				list.Add(text);
				return list;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00017094 File Offset: 0x00015294
		public static List<string> DTCModesList
		{
			get
			{
				return new List<string> { "Auto", "CAN", "ISO", "KWP2000", "Nissan Consult II", "UDS" };
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600020E RID: 526 RVA: 0x000170E8 File Offset: 0x000152E8
		public static List<string> DashboardItemTypesList
		{
			get
			{
				if (StaticLists._DashboardItemTypesList == null)
				{
					StaticLists._DashboardItemTypesList = new List<string>();
				}
				return StaticLists._DashboardItemTypesList;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00017100 File Offset: 0x00015300
		public static void RegisterDashboardItemType(string typename)
		{
			StaticLists.DashboardItemTypesList.Add(typename);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00017110 File Offset: 0x00015310
		public static List<DescriptionPage> DashboardPageTypes
		{
			get
			{
				if (StaticLists._DashboardPageTypes == null)
				{
					StaticLists._DashboardPageTypes = new List<DescriptionPage>();
					DashboardPage.RegisterDashboardPageTypes();
					StaticLists._DashboardPageTypes = StaticLists._DashboardPageTypes.OrderBy((DescriptionPage x) => x.PlacesCount).ToList<DescriptionPage>();
				}
				return StaticLists._DashboardPageTypes;
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0001716B File Offset: 0x0001536B
		public static void RegisterDashboardType(DashboardPage dash)
		{
			StaticLists.DashboardPageTypes.Add(new DescriptionPage(dash.Title, dash.PreviewFile, dash.DashboardType));
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0001718E File Offset: 0x0001538E
		public static List<string> Units
		{
			get
			{
				return UnitsHelper.UnitsCaptionCollection;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00017198 File Offset: 0x00015398
		public static List<string> LanguageList
		{
			get
			{
				return new List<string>(20)
				{
					"Auto", "English", "Русский", "Deutsche", "Türkçe", "Español", "Italiano", "Française", "Português", "Polski",
					"Český", "한국어", "简体中文", "اللغة العربية", "日本語", "Svenska (beta)", "Українська мова", "Magyar", "Български"
				};
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00017280 File Offset: 0x00015480
		public static List<string> SpeedTestTypesList
		{
			get
			{
				return new List<string>(4)
				{
					Translate.GetString("SpeedTest_AccelerationTimeItem.Content"),
					Translate.GetString("SpeedTest_BrakeTimeItem.Content"),
					Translate.GetString("SpeedTest_BrakeDistanceItem.Content"),
					"1/4 mile / 402m"
				};
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000172D0 File Offset: 0x000154D0
		public static List<string> FuelTypesList
		{
			get
			{
				return new List<string>(2)
				{
					Translate.GetString("Settings_Control_Gasoline.Content"),
					Translate.GetString("Settings_Control_Diesel.Content"),
					Translate.GetString("ios_Ethanol"),
					Translate.GetString("ios_Methanol"),
					Translate.GetString("ios_Propan"),
					Translate.GetString("ios_Methan"),
					Translate.GetString("ios_FlexFuelOBDII"),
					Translate.GetString("ios_Custom"),
					Translate.GetString("fuelType_EV")
				};
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00017374 File Offset: 0x00015574
		public static List<string> FuelFlowCalculationSchemesList
		{
			get
			{
				return new List<string>(6)
				{
					Translate.GetString("Settings_Control_FuelScheme_Auto.Content"),
					Translate.GetString("Settings_Control_FuelScheme_MAF.Content"),
					Translate.GetString("Settings_Control_FuelScheme_AbsLOAD.Content"),
					Translate.GetString("Settings_Control_FuelScheme_MAP.Content"),
					Translate.GetString("Settings_Control_FuelScheme_FuelFlowPID.Content"),
					Translate.GetString("Settings_Control_FuelScheme_Injector.Content"),
					Translate.GetString("PID_Cycle_Consumption")
				};
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000173F8 File Offset: 0x000155F8
		public static List<string> BoostCalculationSchemesList
		{
			get
			{
				return new List<string>(6)
				{
					Translate.GetString("Settings_Control_FuelScheme_Auto.Content"),
					Translate.GetString("Settings_Control_FuelScheme_MAP.Content"),
					Translate.GetString("Settings_Control_FuelScheme_MAF.Content"),
					Translate.GetString("Settings_Control_FuelScheme_AbsLOAD.Content")
				};
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0001744B File Offset: 0x0001564B
		public static Color TextColor
		{
			get
			{
				if (SharedSettings.Current.DashboardTheme == 0 || SharedSettings.Current.DashboardTheme == 2)
				{
					return Color.White;
				}
				return Color.Default;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00017471 File Offset: 0x00015671
		public static Color BackgroundColor
		{
			get
			{
				if (SharedSettings.Current.DashboardTheme == 0 || SharedSettings.Current.DashboardTheme == 2)
				{
					return Color.Black;
				}
				return Color.Default;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00017498 File Offset: 0x00015698
		public static List<SettingsPageElement> SettingsList
		{
			get
			{
				List<SettingsPageElement> list = new List<SettingsPageElement>(16);
				if (CodingListModel.IsCodingAvailable(true))
				{
					list.Add(new SettingsPageElement(Translate.GetString("coding_Coding"), "settings_coding.png", typeof(CodingModeSelection)));
				}
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive && DTCv2Model.IsDTCv2Available)
				{
					list.Add(new SettingsPageElement(Translate.GetString("ecuIdentsTitle"), "settings_info.png", typeof(EcuInfoPageV2)));
				}
				list.Add(new SettingsPageElement(Translate.GetString("ios_GarageTitle"), "settings_garage.png", typeof(SettingsGarage)));
				list.Add(new SettingsPageElement(Translate.GetString("Settings_Adapter"), "settings_connection.png", typeof(SettingsConnectionPageV3)));
				list.Add(new SettingsPageElement(Translate.GetString("SettingsPage_itemInterface.Content"), "settings_interface.png", typeof(SettingsInterfacePageV3)));
				list.Add(new SettingsPageElement(Translate.GetString("ios_DashboardSettings"), "settings_icon.png", typeof(SettingsDashboardV3)));
				list.Add(new SettingsPageElement(Translate.GetString("Settings_Control_VehicleOptions.Header"), "settings_car.png", typeof(SettingsVehicleOptions)));
				list.Add(new SettingsPageElement(Translate.GetString("Settings_Control_FuelFlowItem.Content"), "settings_fuel.png", typeof(SettingsFuelRatePage)));
				list.Add(new SettingsPageElement(Translate.GetString("ios_Sensors"), "settings_custompids.png", typeof(SettingsPIDOverrideListPage)));
				list.Add(new SettingsPageElement(Translate.GetString("ios_SettingsRate"), "settings_rate.png", null));
				if (!SharedSettings.Current.AdsProductPurchased)
				{
					Type type = InAppManager.GetInAppPage().GetType();
					list.Add(new SettingsPageElement(Translate.GetString("ios_Buy") + " Car Scanner Pro", "settings_buy.png", type));
					list.Add(new SettingsPageElement(Translate.GetString("ios_RestorePurchases"), "settings_restore.png", type));
				}
				list.Add(new SettingsPageElement(Translate.GetString("ios_BackupTitle"), "settings_backup.png", typeof(SettingsBackup)));
				list.Add(new SettingsPageElement(Translate.GetString("ios_MainPage_TileTerminal"), "settings_terminal.png", null));
				list.Add(new SettingsPageElement(Translate.GetString("ios_ContactDeveloper"), "settings_mail.png", typeof(ContactDeveloperPage)));
				list.Add(new SettingsPageElement(Translate.GetString("SettingsPage_itemHelpInfo.Content"), "settings_info.png", typeof(InfoPage)));
				if (SharedSettings.Current.ShowExperimental)
				{
					list.Add(new SettingsPageElement("Custom codings", "settings_coding.png", typeof(SettingsCustomCodingsListPage)));
				}
				return list;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00017740 File Offset: 0x00015940
		public static List<string> DashboardDefaultThemes
		{
			get
			{
				List<string> list = new List<string>(6);
				list.Add(Translate.GetString("ios_DashboardThemeDark"));
				list.Add(Translate.GetString("ios_DashboardThemeLight"));
				if (!PlatformHelper.IsAndroid)
				{
					list.Add(Translate.GetString("ios_DashboardThemeCarScanner"));
				}
				return list;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0001778C File Offset: 0x0001598C
		public static List<string> SoundsList
		{
			get
			{
				return PlatformHelper.CommonService.GetAvailableSounds().ToList<string>();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600021D RID: 541 RVA: 0x000177A0 File Offset: 0x000159A0
		public static List<string> EngineVolumeList
		{
			get
			{
				List<string> list = new List<string>(100);
				for (int i = 0; i < 100; i++)
				{
					list.Add(((double)i / 10.0).ToString("0.0"));
				}
				return list;
			}
		}

		// Token: 0x04000197 RID: 407
		private static List<string> _ConnectionPickerItems;

		// Token: 0x04000198 RID: 408
		private static List<string> _Numbers0_300;

		// Token: 0x04000199 RID: 409
		private static List<string> _DoubleFormats;

		// Token: 0x0400019A RID: 410
		private static List<string> _DashboardItemTypesList;

		// Token: 0x0400019B RID: 411
		private static List<DescriptionPage> _DashboardPageTypes;

		// Token: 0x02000057 RID: 87
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600021E RID: 542 RVA: 0x000177E2 File Offset: 0x000159E2
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600021F RID: 543 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06000220 RID: 544 RVA: 0x000177EE File Offset: 0x000159EE
			internal int <get_DashboardPageTypes>b__31_0(DescriptionPage x)
			{
				return x.PlacesCount;
			}

			// Token: 0x0400019C RID: 412
			public static readonly StaticLists.<>c <>9 = new StaticLists.<>c();

			// Token: 0x0400019D RID: 413
			public static Func<DescriptionPage, int> <>9__31_0;
		}
	}
}
