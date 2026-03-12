using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2.PIDS;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200081B RID: 2075
	internal class CustomPIDTypeFormulaToTrue : IValueConverter
	{
		// Token: 0x060047EA RID: 18410 RVA: 0x0036FD0A File Offset: 0x0036DF0A
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((CustomPIDType)value == CustomPIDType.Formula)
			{
				return true;
			}
			return false;
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x00002050 File Offset: 0x00000250
		public CustomPIDTypeFormulaToTrue()
		{
		}
	}
}
