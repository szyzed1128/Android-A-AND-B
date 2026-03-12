using System;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000848 RID: 2120
	internal class VagCodingPlatformToTrueConverter : IValueConverter
	{
		// Token: 0x06004872 RID: 18546 RVA: 0x00370B5C File Offset: 0x0036ED5C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
			{
				string text = (string)value;
				if (text != null)
				{
					int length = text.Length;
					switch (length)
					{
					case 3:
						if (!(text == "MQB"))
						{
							goto IL_00EA;
						}
						break;
					case 4:
					{
						char c = text[0];
						if (c != 'P')
						{
							if (c != 'Y')
							{
								goto IL_00EA;
							}
							if (!(text == "YETI"))
							{
								goto IL_00EA;
							}
						}
						else if (!(text == "PQ26"))
						{
							goto IL_00EA;
						}
						break;
					}
					case 5:
						if (!(text == "TNFFL"))
						{
							goto IL_00EA;
						}
						break;
					case 6:
					case 8:
					case 10:
					case 11:
						goto IL_00EA;
					case 7:
						if (!(text == "FABIANJ"))
						{
							goto IL_00EA;
						}
						break;
					case 9:
						if (!(text == "TIGUAN1FL"))
						{
							goto IL_00EA;
						}
						break;
					case 12:
						if (!(text == "MLB-EVO-A4B9"))
						{
							goto IL_00EA;
						}
						break;
					default:
						if (length != 16)
						{
							goto IL_00EA;
						}
						if (!(text == "PQ26NEWRAPID2020"))
						{
							goto IL_00EA;
						}
						break;
					}
					return true;
				}
			}
			IL_00EA:
			return false;
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x00002050 File Offset: 0x00000250
		public VagCodingPlatformToTrueConverter()
		{
		}
	}
}
