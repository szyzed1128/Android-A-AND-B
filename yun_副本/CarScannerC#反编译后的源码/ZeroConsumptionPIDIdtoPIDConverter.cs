using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000088 RID: 136
	public class ZeroConsumptionPIDIdtoPIDConverter : IValueConverter
	{
		// Token: 0x060002BC RID: 700 RVA: 0x00018E88 File Offset: 0x00017088
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<PID> zeroConsumptionPIDCollection = SharedSettings.Current.ZeroConsumptionPIDCollection;
			if (value is int)
			{
				int id = (int)value;
				PID pid = zeroConsumptionPIDCollection.FirstOrDefault((PID x) => x.Id == id);
				if (pid == null)
				{
					pid = zeroConsumptionPIDCollection.FirstOrDefault<PID>();
				}
				return pid;
			}
			return zeroConsumptionPIDCollection.FirstOrDefault<PID>();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00018EDF File Offset: 0x000170DF
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return BindableProperty.UnsetValue;
			}
			if (value is PID)
			{
				return (value as PID).Id;
			}
			return -1;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00002050 File Offset: 0x00000250
		public ZeroConsumptionPIDIdtoPIDConverter()
		{
		}

		// Token: 0x02000089 RID: 137
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060002BF RID: 703 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060002C0 RID: 704 RVA: 0x00018F09 File Offset: 0x00017109
			internal bool <Convert>b__0(PID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x040001BA RID: 442
			public int id;
		}
	}
}
