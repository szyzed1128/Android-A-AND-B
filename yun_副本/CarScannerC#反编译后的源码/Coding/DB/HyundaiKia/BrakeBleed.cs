using System;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B86 RID: 2950
	internal static class BrakeBleed
	{
		// Token: 0x06005A66 RID: 23142 RVA: 0x004320B0 File Offset: 0x004302B0
		public static HyundaiKiaCyclingProcedurePrimitive PicantoTA()
		{
			HyundaiKiaCyclingProcedurePrimitive hyundaiKiaCyclingProcedurePrimitive = new HyundaiKiaCyclingProcedurePrimitive(new string[] { "3019110102", "3019110202" }, "7D1", "7D9", "1081;1090", 5000, 60, true);
			string text = "Kia Picanto TA, Kia Ceed ED, Kia Ceed JD, Kia Soul PS, Kia Venga, Kia Carnival YP, Kia Cerato YD (var.1)";
			hyundaiKiaCyclingProcedurePrimitive.Name = "Brake system bleed: " + text;
			hyundaiKiaCyclingProcedurePrimitive.InnerDescription = "WARNING! OPERATIONS WITH BRAKE SYSTEM ARE EXTREMELY DANGEROUS AND COULD LEAD TO ACCIDENTS!" + SupportedItemsDetectorBase.GetAccessKeyWarning;
			hyundaiKiaCyclingProcedurePrimitive.Group = CodingGroup.Brakes;
			hyundaiKiaCyclingProcedurePrimitive.Translations.Add(new TranslationItem("ru", "Прокачка тормозной системы: " + text, "", "ВНИМАНИЕ! ЛЮБЫЕ ОПЕРАЦИИ С ТОРМОЗНОЙ СИСТЕМОЙ ЧРЕЗВЫЧАЙНО ОПАСНЫ! НЕКВАЛИФИЦИРОВАННОЕ ПРОВЕДЕНИЕ РАБОТ С ТОРМОЗНОЙ СИСТЕМОЙ МОЖЕТ ПРИВЕСТИ К ДТП!" + SupportedItemsDetectorBase.GetAccessKeyWarningRu));
			return hyundaiKiaCyclingProcedurePrimitive;
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x00432158 File Offset: 0x00430358
		public static HyundaiKiaCyclingProcedurePrimitive PicantoJA()
		{
			HyundaiKiaCyclingProcedurePrimitive hyundaiKiaCyclingProcedurePrimitive = new HyundaiKiaCyclingProcedurePrimitive(new string[] { "2FF01E03AA4C", "2FF01E00AA4C" }, "7D1", "7D9", "1003;1003", 5000, 60, true);
			string text = "Kia Picanto JA, Kia RIO SC, Kia RIO YB";
			hyundaiKiaCyclingProcedurePrimitive.Name = "Brake system bleed: " + text;
			hyundaiKiaCyclingProcedurePrimitive.InnerDescription = "WARNING! OPERATIONS WITH BRAKE SYSTEM ARE EXTREMELY DANGEROUS AND COULD LEAD TO ACCIDENTS!";
			hyundaiKiaCyclingProcedurePrimitive.Group = CodingGroup.Brakes;
			hyundaiKiaCyclingProcedurePrimitive.Translations.Add(new TranslationItem("ru", "Прокачка тормозной системы: " + text, "", "ВНИМАНИЕ! ЛЮБЫЕ ОПЕРАЦИИ С ТОРМОЗНОЙ СИСТЕМОЙ ЧРЕЗВЫЧАЙНО ОПАСНЫ! НЕКВАЛИФИЦИРОВАННОЕ ПРОВЕДЕНИЕ РАБОТ С ТОРМОЗНОЙ СИСТЕМОЙ МОЖЕТ ПРИВЕСТИ К ДТП!"));
			return hyundaiKiaCyclingProcedurePrimitive;
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x004321EC File Offset: 0x004303EC
		public static HyundaiKiaCyclingProcedurePrimitive RioFB()
		{
			HyundaiKiaCyclingProcedurePrimitive hyundaiKiaCyclingProcedurePrimitive = new HyundaiKiaCyclingProcedurePrimitive(new string[] { "2FF06F03FFF019", "2FF06F00FFF019" }, "7D1", "7D9", "1003;1003", 5000, 60, true);
			string text = "Kia RIO FB, Kia SPORTAGE QL, Kia STINGER CK, Kia Cadenza YG, Kia OPTIMA TF";
			hyundaiKiaCyclingProcedurePrimitive.Name = "Brake system bleed: " + text;
			hyundaiKiaCyclingProcedurePrimitive.InnerDescription = "WARNING! OPERATIONS WITH BRAKE SYSTEM ARE EXTREMELY DANGEROUS AND COULD LEAD TO ACCIDENTS!";
			hyundaiKiaCyclingProcedurePrimitive.Group = CodingGroup.Brakes;
			hyundaiKiaCyclingProcedurePrimitive.Translations.Add(new TranslationItem("ru", "Прокачка тормозной системы: " + text, "", "ВНИМАНИЕ! ЛЮБЫЕ ОПЕРАЦИИ С ТОРМОЗНОЙ СИСТЕМОЙ ЧРЕЗВЫЧАЙНО ОПАСНЫ! НЕКВАЛИФИЦИРОВАННОЕ ПРОВЕДЕНИЕ РАБОТ С ТОРМОЗНОЙ СИСТЕМОЙ МОЖЕТ ПРИВЕСТИ К ДТП!"));
			return hyundaiKiaCyclingProcedurePrimitive;
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x00432280 File Offset: 0x00430480
		public static HyundaiKiaCyclingProcedurePrimitive RioQBR()
		{
			HyundaiKiaCyclingProcedurePrimitive hyundaiKiaCyclingProcedurePrimitive = new HyundaiKiaCyclingProcedurePrimitive(new string[] { "30F000F019FF", "30F011F019FF" }, "7D1", "7D9", "1081;1090", 5000, 60, false);
			string text = "Kia Rio QBR, Kia Rio UB, Kia CERATO TD, Kia SORENTO XM, Kia SOUL AM, Kia SPORTAGE SL, Kia QUORIS KH, Kia Cadenza VG, Kia CARENS RP";
			hyundaiKiaCyclingProcedurePrimitive.Name = "Brake system bleed: " + text;
			hyundaiKiaCyclingProcedurePrimitive.InnerDescription = "WARNING! OPERATIONS WITH BRAKE SYSTEM ARE EXTREMELY DANGEROUS AND COULD LEAD TO ACCIDENTS!";
			hyundaiKiaCyclingProcedurePrimitive.Group = CodingGroup.Brakes;
			hyundaiKiaCyclingProcedurePrimitive.Translations.Add(new TranslationItem("ru", "Прокачка тормозной системы: " + text, "", "ВНИМАНИЕ! ЛЮБЫЕ ОПЕРАЦИИ С ТОРМОЗНОЙ СИСТЕМОЙ ЧРЕЗВЫЧАЙНО ОПАСНЫ! НЕКВАЛИФИЦИРОВАННОЕ ПРОВЕДЕНИЕ РАБОТ С ТОРМОЗНОЙ СИСТЕМОЙ МОЖЕТ ПРИВЕСТИ К ДТП!"));
			return hyundaiKiaCyclingProcedurePrimitive;
		}

		// Token: 0x06005A6A RID: 23146 RVA: 0x00432314 File Offset: 0x00430514
		public static HyundaiKiaCyclingProcedurePrimitive CeratoYD()
		{
			HyundaiKiaCyclingProcedurePrimitive hyundaiKiaCyclingProcedurePrimitive = new HyundaiKiaCyclingProcedurePrimitive(new string[] { "2FF022030102", "2FF022030202" }, "7D1", "7D9", "1001;1003", 5000, 60, false);
			string text = "KIA CERATO YD (var.2), Kia SORENTO UM, Kia OPTIMA JF";
			hyundaiKiaCyclingProcedurePrimitive.Name = "Brake system bleed: " + text;
			hyundaiKiaCyclingProcedurePrimitive.InnerDescription = "WARNING! OPERATIONS WITH BRAKE SYSTEM ARE EXTREMELY DANGEROUS AND COULD LEAD TO ACCIDENTS!";
			hyundaiKiaCyclingProcedurePrimitive.Group = CodingGroup.Brakes;
			hyundaiKiaCyclingProcedurePrimitive.Translations.Add(new TranslationItem("ru", "Прокачка тормозной системы: " + text, "", "ВНИМАНИЕ! ЛЮБЫЕ ОПЕРАЦИИ С ТОРМОЗНОЙ СИСТЕМОЙ ЧРЕЗВЫЧАЙНО ОПАСНЫ! НЕКВАЛИФИЦИРОВАННОЕ ПРОВЕДЕНИЕ РАБОТ С ТОРМОЗНОЙ СИСТЕМОЙ МОЖЕТ ПРИВЕСТИ К ДТП!"));
			return hyundaiKiaCyclingProcedurePrimitive;
		}
	}
}
