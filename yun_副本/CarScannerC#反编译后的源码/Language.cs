using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms
{
	// Token: 0x020000A0 RID: 160
	public static class Language
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0001D84C File Offset: 0x0001BA4C
		public static void SetLanguage()
		{
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				Language.origCulture = new CultureInfo("ru", true);
				Language.origUICulture = Language.origCulture;
			}
			CultureInfo cultureInfo;
			switch (SharedSettings.Current.Language)
			{
			case 1:
				cultureInfo = new CultureInfo("en", true);
				goto IL_01E4;
			case 2:
				cultureInfo = new CultureInfo("ru", true);
				goto IL_01E4;
			case 3:
				cultureInfo = new CultureInfo("de", true);
				goto IL_01E4;
			case 4:
				cultureInfo = new CultureInfo("tr", true);
				goto IL_01E4;
			case 5:
				cultureInfo = new CultureInfo("es", true);
				goto IL_01E4;
			case 6:
				cultureInfo = new CultureInfo("it", true);
				goto IL_01E4;
			case 7:
				cultureInfo = new CultureInfo("fr", true);
				goto IL_01E4;
			case 8:
				cultureInfo = new CultureInfo("pt", true);
				goto IL_01E4;
			case 9:
				cultureInfo = new CultureInfo("pl", true);
				goto IL_01E4;
			case 10:
				cultureInfo = new CultureInfo("cs-CZ", true);
				goto IL_01E4;
			case 11:
				cultureInfo = new CultureInfo("ko", true);
				goto IL_01E4;
			case 12:
				cultureInfo = new CultureInfo("zh-Hans", true);
				goto IL_01E4;
			case 13:
				cultureInfo = new CultureInfo("fa", true);
				goto IL_01E4;
			case 14:
				cultureInfo = new CultureInfo("ja", true);
				goto IL_01E4;
			case 15:
				cultureInfo = new CultureInfo("sv-SE", true);
				goto IL_01E4;
			case 16:
				cultureInfo = new CultureInfo("uk", true);
				goto IL_01E4;
			case 17:
				cultureInfo = new CultureInfo("hu", true);
				goto IL_01E4;
			case 18:
				cultureInfo = new CultureInfo("bg", true);
				goto IL_01E4;
			}
			Thread.CurrentThread.CurrentCulture = Language.origCulture;
			Thread.CurrentThread.CurrentUICulture = Language.origUICulture;
			Language.CurrentLanguageCode = Language.origUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
			Translate.SetCulture(Language.origUICulture);
			return;
			IL_01E4:
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				cultureInfo = new CultureInfo("ru", true);
			}
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			Language.CurrentLanguageCode = cultureInfo.TwoLetterISOLanguageName.ToLowerInvariant();
			Translate.SetCulture(cultureInfo);
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0001DA7D File Offset: 0x0001BC7D
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0001DA84 File Offset: 0x0001BC84
		public static CultureInfo origCulture
		{
			[CompilerGenerated]
			get
			{
				return Language.<origCulture>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				Language.<origCulture>k__BackingField = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0001DA8C File Offset: 0x0001BC8C
		// (set) Token: 0x0600032F RID: 815 RVA: 0x0001DA93 File Offset: 0x0001BC93
		public static CultureInfo origUICulture
		{
			[CompilerGenerated]
			get
			{
				return Language.<origUICulture>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				Language.<origUICulture>k__BackingField = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0001DA9B File Offset: 0x0001BC9B
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0001DAA2 File Offset: 0x0001BCA2
		public static string CurrentLanguageCode
		{
			[CompilerGenerated]
			get
			{
				return Language.<CurrentLanguageCode>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				Language.<CurrentLanguageCode>k__BackingField = value;
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0001DAAA File Offset: 0x0001BCAA
		public static void SetOrigCulture()
		{
			if (Language.origCulture == null)
			{
				Language.origCulture = Thread.CurrentThread.CurrentCulture;
			}
			if (Language.origUICulture == null)
			{
				Language.origUICulture = Thread.CurrentThread.CurrentUICulture;
			}
		}

		// Token: 0x0400020C RID: 524
		[CompilerGenerated]
		private static CultureInfo <origCulture>k__BackingField;

		// Token: 0x0400020D RID: 525
		[CompilerGenerated]
		private static CultureInfo <origUICulture>k__BackingField;

		// Token: 0x0400020E RID: 526
		[CompilerGenerated]
		private static string <CurrentLanguageCode>k__BackingField;
	}
}
