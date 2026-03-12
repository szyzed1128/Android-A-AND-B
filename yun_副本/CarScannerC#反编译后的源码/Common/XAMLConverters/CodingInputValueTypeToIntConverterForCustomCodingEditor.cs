using System;
using System.Globalization;
using CarScannerXamarinForms.Coding;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000816 RID: 2070
	internal class CodingInputValueTypeToIntConverterForCustomCodingEditor : IValueConverter
	{
		// Token: 0x060047DB RID: 18395 RVA: 0x0036FA94 File Offset: 0x0036DC94
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			AdaptationValueTypes adaptationValueTypes = (AdaptationValueTypes)value;
			switch (adaptationValueTypes)
			{
			case AdaptationValueTypes.OptionType:
				return 0;
			case AdaptationValueTypes.InputValueType:
				return 1;
			case AdaptationValueTypes.InputHexDataType:
				return 3;
			case AdaptationValueTypes.MQBLightConfiguration:
			case AdaptationValueTypes.MQBColorList:
			case AdaptationValueTypes.MQBParametrizeDump:
				break;
			case AdaptationValueTypes.InputTextType:
				return 4;
			default:
				if (adaptationValueTypes == AdaptationValueTypes.InputFloatIEEE754)
				{
					return 2;
				}
				break;
			}
			return 0;
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0036FAFC File Offset: 0x0036DCFC
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				switch ((int)value)
				{
				case 0:
					obj = AdaptationValueTypes.OptionType;
					break;
				case 1:
					obj = AdaptationValueTypes.InputValueType;
					break;
				case 2:
					obj = AdaptationValueTypes.InputFloatIEEE754;
					break;
				case 3:
					obj = AdaptationValueTypes.InputHexDataType;
					break;
				case 4:
					obj = AdaptationValueTypes.InputTextType;
					break;
				default:
					obj = AdaptationValueTypes.OptionType;
					break;
				}
			}
			catch
			{
				obj = AdaptationValueTypes.OptionType;
			}
			return obj;
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x00002050 File Offset: 0x00000250
		public CodingInputValueTypeToIntConverterForCustomCodingEditor()
		{
		}
	}
}
