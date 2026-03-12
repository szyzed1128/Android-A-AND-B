using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CarScannerXamarinForms.DTC;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200006D RID: 109
	public class DTCDescriptionCollectionToStringConverter : IValueConverter
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00018044 File Offset: 0x00016244
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is IEnumerable<BrandAndDescription>)
			{
				BrandAndDescription[] array = (value as IEnumerable<BrandAndDescription>).ToArray<BrandAndDescription>();
				FormattedString formattedString = new FormattedString();
				for (int i = 0; i < array.Length; i++)
				{
					formattedString.Spans.Add(new Span
					{
						Text = array[i].Description,
						FontSize = (double)Application.Current.Resources["BaseFontSize-"]
					});
					if (i < array.Length - 1)
					{
						formattedString.Spans.Add(new Span
						{
							Text = "\n"
						});
					}
				}
				return formattedString;
			}
			return new FormattedString();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00002050 File Offset: 0x00000250
		public DTCDescriptionCollectionToStringConverter()
		{
		}
	}
}
