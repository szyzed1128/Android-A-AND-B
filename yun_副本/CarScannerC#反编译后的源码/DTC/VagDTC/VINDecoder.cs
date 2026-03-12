using System;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.DTC.VagDTC
{
	// Token: 0x0200058D RID: 1421
	internal static class VINDecoder
	{
		// Token: 0x060033E7 RID: 13287 RVA: 0x002440D8 File Offset: 0x002422D8
		private static int GetYearFromChar2010_2039(char c10)
		{
			switch (c10)
			{
			case '1':
				return 2031;
			case '2':
				return 2032;
			case '3':
				return 2033;
			case '4':
				return 2034;
			case '5':
				return 2035;
			case '6':
				return 2036;
			case '7':
				return 2037;
			case '8':
				return 2038;
			case '9':
				return 2039;
			case 'A':
				return 2010;
			case 'B':
				return 2011;
			case 'C':
				return 2012;
			case 'D':
				return 2013;
			case 'E':
				return 2014;
			case 'F':
				return 2015;
			case 'G':
				return 2016;
			case 'H':
				return 2017;
			case 'J':
				return 2018;
			case 'K':
				return 2019;
			case 'L':
				return 2020;
			case 'M':
				return 2021;
			case 'N':
				return 2022;
			case 'P':
				return 2023;
			case 'R':
				return 2024;
			case 'S':
				return 2025;
			case 'T':
				return 2026;
			case 'V':
				return 2027;
			case 'W':
				return 2028;
			case 'X':
				return 2029;
			case 'Y':
				return 2030;
			}
			return 0;
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0024424C File Offset: 0x0024244C
		private static int GetYearFromChar1980_2009(char c10)
		{
			switch (c10)
			{
			case '1':
				return 2001;
			case '2':
				return 2002;
			case '3':
				return 2003;
			case '4':
				return 2004;
			case '5':
				return 2005;
			case '6':
				return 2006;
			case '7':
				return 2007;
			case '8':
				return 2008;
			case '9':
				return 2009;
			case 'A':
				return 1980;
			case 'B':
				return 1981;
			case 'C':
				return 1982;
			case 'D':
				return 1983;
			case 'E':
				return 1984;
			case 'F':
				return 1985;
			case 'G':
				return 1986;
			case 'H':
				return 1987;
			case 'J':
				return 1988;
			case 'K':
				return 1989;
			case 'L':
				return 1990;
			case 'M':
				return 1991;
			case 'N':
				return 1992;
			case 'P':
				return 1993;
			case 'R':
				return 1994;
			case 'S':
				return 1995;
			case 'T':
				return 1996;
			case 'V':
				return 1997;
			case 'W':
				return 1998;
			case 'X':
				return 1999;
			case 'Y':
				return 2000;
			}
			return 0;
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x002443C0 File Offset: 0x002425C0
		public static int GetYearFromVin(string vin, bool? usesCAN = null)
		{
			if (vin == null)
			{
				return 0;
			}
			if (vin.Length < 10)
			{
				return 0;
			}
			char c = vin[6];
			char c2 = vin[9];
			int year = DateTimeNowHelper.NowSafe.Year;
			if (char.IsDigit(c))
			{
				int yearFromChar1980_ = VINDecoder.GetYearFromChar1980_2009(c2);
				if (usesCAN != null && usesCAN.Value && yearFromChar1980_ < 1991)
				{
					return VINDecoder.GetYearFromChar2010_2039(c2);
				}
				if (yearFromChar1980_ < 1996)
				{
					return VINDecoder.GetYearFromChar2010_2039(c2);
				}
				return yearFromChar1980_;
			}
			else
			{
				if (!char.IsLetter(c))
				{
					return 0;
				}
				int yearFromChar2010_ = VINDecoder.GetYearFromChar2010_2039(c2);
				if (yearFromChar2010_ > year)
				{
					return VINDecoder.GetYearFromChar1980_2009(c2);
				}
				return yearFromChar2010_;
			}
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x00244462 File Offset: 0x00242662
		public static string GetVagModelCodeFromVIN(string vin)
		{
			if (string.IsNullOrEmpty(vin))
			{
				return "";
			}
			if (vin.Length < 8)
			{
				return "";
			}
			return vin.Substring(6, 2);
		}
	}
}
