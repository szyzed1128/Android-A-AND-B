using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000070 RID: 112
	public class ECUInitializationPickerConverter : IValueConverter
	{
		// Token: 0x0600026F RID: 623 RVA: 0x00018253 File Offset: 0x00016453
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is bool))
			{
				return false;
			}
			if ((bool)value)
			{
				return ECUInitializationPickerConverter.Items[0];
			}
			return ECUInitializationPickerConverter.Items[1];
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00018286 File Offset: 0x00016486
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is string))
			{
				return false;
			}
			if ((string)value == ECUInitializationPickerConverter.Items[0])
			{
				return true;
			}
			return false;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000271 RID: 625 RVA: 0x000182C0 File Offset: 0x000164C0
		public static List<string> Items
		{
			get
			{
				if (ECUInitializationPickerConverter._Items == null)
				{
					string @string = Translate.GetString("Settings_Control_InitSequenceDefault.Content");
					string string2 = Translate.GetString("Settings_Control_InitSequenceCustom.Content");
					ECUInitializationPickerConverter._Items = new List<string> { @string, string2 };
				}
				return ECUInitializationPickerConverter._Items;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00018307 File Offset: 0x00016507
		public List<string> ItemsS
		{
			get
			{
				return ECUInitializationPickerConverter.Items;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00002050 File Offset: 0x00000250
		public ECUInitializationPickerConverter()
		{
		}

		// Token: 0x040001A9 RID: 425
		private static List<string> _Items;
	}
}
