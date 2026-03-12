using System;
using System.Globalization;
using CarScannerXamarinForms.Dashboard;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000069 RID: 105
	public class DashboardItemTypeToIndexConverter : IValueConverter
	{
		// Token: 0x0600025A RID: 602 RVA: 0x00017E58 File Offset: 0x00016058
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is DashboardItemTypes)
			{
				return (int)((DashboardItemTypes)value);
			}
			return 0;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00017E78 File Offset: 0x00016078
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is int))
			{
				return DashboardItemTypes.Text;
			}
			int num = (int)value;
			if (num >= 0 && num < StaticLists.DashboardItemTypesList.Count)
			{
				return (DashboardItemTypes)num;
			}
			return DashboardItemTypes.Text;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00002050 File Offset: 0x00000250
		public DashboardItemTypeToIndexConverter()
		{
		}
	}
}
