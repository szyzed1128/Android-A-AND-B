using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2.PIDS;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200083E RID: 2110
	internal class PidToUnitsTitleConverter : IValueConverter
	{
		// Token: 0x06004854 RID: 18516 RVA: 0x003708D4 File Offset: 0x0036EAD4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IPID ipid = (IPID)value;
			if (!(ipid is IPIDFloatValue))
			{
				return "";
			}
			IPIDFloatValue ipidfloatValue = (IPIDFloatValue)ipid;
			UnitsHelper.Units customUnit = ipid.CustomUnit;
			if (customUnit == UnitsHelper.Units.None)
			{
				return UnitsHelper.GetCaption(ipidfloatValue.Units);
			}
			if (customUnit == UnitsHelper.Units.None)
			{
				return Translate.GetString("pid_Units_None");
			}
			return UnitsHelper.GetCaptionInvariant(customUnit);
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x00002050 File Offset: 0x00000250
		public PidToUnitsTitleConverter()
		{
		}
	}
}
