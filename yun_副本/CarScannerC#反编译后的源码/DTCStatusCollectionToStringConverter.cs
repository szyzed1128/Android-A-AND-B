using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CarScannerXamarinForms.DTC;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200006F RID: 111
	public class DTCStatusCollectionToStringConverter : IValueConverter
	{
		// Token: 0x0600026C RID: 620 RVA: 0x0001811C File Offset: 0x0001631C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is IEnumerable<DTCStatusHelper.DTCStatus>)
			{
				new Label();
				DTCStatusHelper.DTCStatus[] array = (value as IEnumerable<DTCStatusHelper.DTCStatus>).ToArray<DTCStatusHelper.DTCStatus>();
				FormattedString formattedString = new FormattedString();
				formattedString.Spans.Add(new Span
				{
					Text = Translate.GetString("ios_Status") + " ",
					FontAttributes = 0,
					FontSize = (double)Application.Current.Resources["BaseFontSize"]
				});
				for (int i = 0; i < array.Length; i++)
				{
					if (i < array.Length - 1)
					{
						formattedString.Spans.Add(new Span
						{
							Text = DTCStatusHelper.GetTitle(array[i]) + ", ",
							FontAttributes = 2,
							FontSize = (double)Application.Current.Resources["BaseFontSize"]
						});
					}
					else
					{
						formattedString.Spans.Add(new Span
						{
							Text = DTCStatusHelper.GetTitle(array[i]),
							FontAttributes = 2,
							FontSize = (double)Application.Current.Resources["BaseFontSize"]
						});
					}
				}
				return formattedString;
			}
			return new FormattedString();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00002050 File Offset: 0x00000250
		public DTCStatusCollectionToStringConverter()
		{
		}
	}
}
