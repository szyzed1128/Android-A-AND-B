using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000815 RID: 2069
	internal class BTLENameToImageSourceConverter : IValueConverter
	{
		// Token: 0x060047D8 RID: 18392 RVA: 0x0036F930 File Offset: 0x0036DB30
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			foreach (string text2 in new string[]
			{
				"obd", "OBDII", "Micro Mechanic", "IOS-Vlink", "VEEPEAK", "Viecar", "Carista", "OBDBLE", "SCANEX", "OBDII iOS",
				"TECAR", "UniCarScan iOS", "FIXD", "JDY-08", "Autophix 3210", "CACAGOO OBD-II", "BAST", "JDY", "Kiwi", "Ycle Ast",
				"CarScanX", "IOS-MAOZUA", "Ia``", "JUTA OBD II IOS", "L4-Automati", "MicroTech", "PP2145", "SPD SCAN", "SwiSys", "WSIIROON OBDII",
				"Rokodil"
			})
			{
				if (text.IndexOf(text2, 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
				{
					return "elm327icon.png";
				}
			}
			return "";
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x00002050 File Offset: 0x00000250
		public BTLENameToImageSourceConverter()
		{
		}
	}
}
