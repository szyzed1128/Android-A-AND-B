using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000060 RID: 96
	internal class BTLENameToVisibleBoolConverter : IValueConverter
	{
		// Token: 0x0600023F RID: 575 RVA: 0x00017A78 File Offset: 0x00015C78
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
				"CarScanX", "IOS-MAOZUA", "JUTA OBD II IOS", "L4-Automati", "MicroTech", "PP2145", "SPD SCAN", "SwiSys", "WSIIROON OBDII", "NexzScan"
			})
			{
				if (text.IndexOf(text2, 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00002050 File Offset: 0x00000250
		public BTLENameToVisibleBoolConverter()
		{
		}
	}
}
