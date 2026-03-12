using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200007B RID: 123
	public class ItemTypeToIndexConverter : IValueConverter
	{
		// Token: 0x06000291 RID: 657 RVA: 0x00018664 File Offset: 0x00016864
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DashboardItemTypes)
			{
				return (int)((DashboardItemTypes)value);
			}
			return 0;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00018680 File Offset: 0x00016880
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int)
			{
				return (DashboardItemTypes)((int)value);
			}
			return DashboardItemTypes.Text;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00002050 File Offset: 0x00000250
		public ItemTypeToIndexConverter()
		{
		}
	}
}
