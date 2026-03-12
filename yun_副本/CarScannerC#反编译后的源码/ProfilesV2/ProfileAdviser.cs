using System;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.ProfilesV2
{
	// Token: 0x020002C3 RID: 707
	internal static class ProfileAdviser
	{
		// Token: 0x06002278 RID: 8824 RVA: 0x001A9F2C File Offset: 0x001A812C
		public static string CheckForBetterProfileAvailable()
		{
			string text = null;
			string selectedBrand = SharedSettings.Current.SelectedBrand;
			if (selectedBrand == "Toyota" || selectedBrand == "Lexus")
			{
				text = ProfileAdviser.GetBetterProfileForToyota();
			}
			return text;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x001A9F68 File Offset: 0x001A8168
		private static string GetBetterProfileForToyota()
		{
			if (SharedSettings.Current.SelectedProfileV2Name == "OBD-II / EOBD")
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					if (CarInfoViewModel.Instance.IsVINAvailable)
					{
						int yearFromVin = ProfileAdviser.GetYearFromVin(CarInfoViewModel.Instance.VIN);
						if (yearFromVin >= 2006)
						{
							if (yearFromVin >= 2016)
							{
								return "96260a504ebb4c6781eedee86dba6f09";
							}
							if (yearFromVin < 2009)
							{
								return "7bc899de79a34bf98596ea8e6c6f27b4";
							}
							return "726eab54fe6f4247a5b0c88d7f9e8174";
						}
					}
				}
				else if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					return "cd56b7a6cb8f42d4b52b76c65cf8dddb";
				}
			}
			return null;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x001A9FF4 File Offset: 0x001A81F4
		public static int GetYearFromVin(string vin)
		{
			if (vin == null)
			{
				return 0;
			}
			if (vin.Length < 17)
			{
				return 0;
			}
			int year = DateTimeNowHelper.NowSafe.Year;
			switch (vin[9])
			{
			case '8':
				return 2008;
			case '9':
				return 2009;
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
			}
			return 0;
		}
	}
}
