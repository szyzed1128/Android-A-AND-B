using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Forms.Xaml;

namespace CarScannerXamarinForms
{
	// Token: 0x0200005B RID: 91
	public class Translate : IMarkupExtension
	{
		// Token: 0x06000228 RID: 552 RVA: 0x00017835 File Offset: 0x00015A35
		public static void SetCulture(CultureInfo culture)
		{
			Translate._culture = culture;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0001783D File Offset: 0x00015A3D
		public static ResourceManager ResMgr
		{
			get
			{
				if (Translate._resmgr == null)
				{
					Translate._resmgr = new ResourceManager("CarScannerXamarinForms.Resources", typeof(SimpleMainPage).GetTypeInfo().Assembly);
				}
				return Translate._resmgr;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00017870 File Offset: 0x00015A70
		public static string GetString(string key)
		{
			string text2;
			try
			{
				string text = ((Translate._culture == null) ? Translate.ResMgr.GetString(key) : Translate.ResMgr.GetString(key, Translate._culture));
				if (PlatformHelper.AppMarket == Markets.RUS && text != null && text.Length >= 11 && text.IndexOf("Car Scanner") >= 0 && text.IndexOf("Car Scanner Rus") <= 0)
				{
					text = text.Replace("Car Scanner", "Car Scanner Rus");
				}
				text2 = text;
			}
			catch (Exception)
			{
				text2 = key;
			}
			return text2;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00017900 File Offset: 0x00015B00
		public static bool HasString(string key, out string target)
		{
			string @string = Translate.GetString(key);
			target = @string;
			return @string != null;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0001791D File Offset: 0x00015B1D
		public static bool HasString(string key)
		{
			return Translate.GetString(key) != null;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00002050 File Offset: 0x00000250
		public Translate()
		{
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0001792A File Offset: 0x00015B2A
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00017932 File Offset: 0x00015B32
		public string Text
		{
			[CompilerGenerated]
			get
			{
				return this.<Text>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Text>k__BackingField = value;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0001793C File Offset: 0x00015B3C
		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this.Text == null)
			{
				return "";
			}
			string text = Translate.GetString(this.Text);
			if (text == null)
			{
				text = this.Text;
			}
			return text;
		}

		// Token: 0x040001A4 RID: 420
		private static CultureInfo _culture;

		// Token: 0x040001A5 RID: 421
		private static ResourceManager _resmgr;

		// Token: 0x040001A6 RID: 422
		private const string ResourceId = "CarScannerXamarinForms.Resources";

		// Token: 0x040001A7 RID: 423
		[CompilerGenerated]
		private string <Text>k__BackingField;
	}
}
