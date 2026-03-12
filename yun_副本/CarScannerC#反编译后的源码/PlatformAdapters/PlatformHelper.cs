using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.PlatformAdapters
{
	// Token: 0x020002E0 RID: 736
	public static class PlatformHelper
	{
		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x001AE01A File Offset: 0x001AC21A
		// (set) Token: 0x06002322 RID: 8994 RVA: 0x001AE021 File Offset: 0x001AC221
		public static bool IsAndroid
		{
			[CompilerGenerated]
			get
			{
				return PlatformHelper.<IsAndroid>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PlatformHelper.<IsAndroid>k__BackingField = value;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x001AE029 File Offset: 0x001AC229
		// (set) Token: 0x06002324 RID: 8996 RVA: 0x001AE030 File Offset: 0x001AC230
		public static bool IsiOS
		{
			[CompilerGenerated]
			get
			{
				return PlatformHelper.<IsiOS>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PlatformHelper.<IsiOS>k__BackingField = value;
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x001AE038 File Offset: 0x001AC238
		// (set) Token: 0x06002326 RID: 8998 RVA: 0x001AE03F File Offset: 0x001AC23F
		public static Markets AppMarket
		{
			[CompilerGenerated]
			get
			{
				return PlatformHelper.<AppMarket>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				PlatformHelper.<AppMarket>k__BackingField = value;
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x001AE048 File Offset: 0x001AC248
		public static string AppMarketTitle
		{
			get
			{
				switch (PlatformHelper.AppMarket)
				{
				case Markets.AppStore:
					return "App Store";
				case Markets.GooglePlay:
					return "Google Play";
				case Markets.HMS:
					return "Huawei AppGallery";
				case Markets.Rustore:
					return "Rustore";
				case Markets.RUS:
					return "Car Scanner Rus";
				case Markets.Sideload:
					return "Google Play";
				default:
					return "";
				}
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06002328 RID: 9000 RVA: 0x001AE0A4 File Offset: 0x001AC2A4
		public static string AppMarketTag
		{
			get
			{
				switch (PlatformHelper.AppMarket)
				{
				case Markets.AppStore:
					return "iOS";
				case Markets.GooglePlay:
					return "GP";
				case Markets.HMS:
					return "HMS";
				case Markets.Rustore:
					return "RUSTORE";
				case Markets.RUS:
					return "RUS";
				case Markets.Sideload:
					return "SL";
				default:
					return "";
				}
			}
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x001AE100 File Offset: 0x001AC300
		public static bool IsPlatformVersionNewerOrEqual(int major, int minor)
		{
			return DependencyService.Get<IPlatformCommonService>(0).IsVersionEqualsOrHigher(major, minor);
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x0600232A RID: 9002 RVA: 0x001AE10F File Offset: 0x001AC30F
		public static string AppVersion
		{
			get
			{
				return DependencyService.Get<IPlatformCommonService>(0).AppVersion;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x001AE11C File Offset: 0x001AC31C
		public static string AppBuild
		{
			get
			{
				return DependencyService.Get<IPlatformCommonService>(0).AppBuild;
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x0600232C RID: 9004 RVA: 0x001AE129 File Offset: 0x001AC329
		public static IPlatformCommonService CommonService
		{
			get
			{
				if (PlatformHelper._CommonService == null)
				{
					PlatformHelper._CommonService = DependencyService.Get<IPlatformCommonService>(0);
				}
				return PlatformHelper._CommonService;
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x001AE142 File Offset: 0x001AC342
		public static IPlatformSpecificServiceDroid DroidService
		{
			get
			{
				if (PlatformHelper._DroidService == null)
				{
					PlatformHelper._DroidService = DependencyService.Get<IPlatformSpecificServiceDroid>(0);
				}
				return PlatformHelper._DroidService;
			}
		}

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x0600232E RID: 9006 RVA: 0x001AE15B File Offset: 0x001AC35B
		public static IPlatformSpecificServiceIOS IOSService
		{
			get
			{
				if (PlatformHelper._IOSService == null)
				{
					PlatformHelper._IOSService = DependencyService.Get<IPlatformSpecificServiceIOS>(0);
				}
				return PlatformHelper._IOSService;
			}
		}

		// Token: 0x040010EC RID: 4332
		[CompilerGenerated]
		private static bool <IsAndroid>k__BackingField;

		// Token: 0x040010ED RID: 4333
		[CompilerGenerated]
		private static bool <IsiOS>k__BackingField;

		// Token: 0x040010EE RID: 4334
		[CompilerGenerated]
		private static Markets <AppMarket>k__BackingField;

		// Token: 0x040010EF RID: 4335
		private static IPlatformCommonService _CommonService;

		// Token: 0x040010F0 RID: 4336
		private static IPlatformSpecificServiceDroid _DroidService;

		// Token: 0x040010F1 RID: 4337
		private static IPlatformSpecificServiceIOS _IOSService;
	}
}
