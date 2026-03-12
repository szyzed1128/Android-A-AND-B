using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200005E RID: 94
	public class BoolToPickerItemConverter : IValueConverter
	{
		// Token: 0x06000237 RID: 567 RVA: 0x000179A7 File Offset: 0x00015BA7
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is bool))
			{
				return false;
			}
			if ((bool)value)
			{
				return BoolToPickerItemConverter.Items[0];
			}
			return BoolToPickerItemConverter.Items[1];
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000179DA File Offset: 0x00015BDA
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is string))
			{
				return false;
			}
			if ((string)value == BoolToPickerItemConverter.Items[0])
			{
				return true;
			}
			return false;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00017A14 File Offset: 0x00015C14
		public static List<string> Items
		{
			get
			{
				if (BoolToPickerItemConverter._Items == null)
				{
					string text = "";
					string text2 = "";
					BoolToPickerItemConverter._Items = new List<string> { text, text2 };
				}
				return BoolToPickerItemConverter._Items;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00017A51 File Offset: 0x00015C51
		public List<string> ItemsS
		{
			get
			{
				return BoolToPickerItemConverter.Items;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00002050 File Offset: 0x00000250
		public BoolToPickerItemConverter()
		{
		}

		// Token: 0x040001A8 RID: 424
		private static List<string> _Items;
	}
}
