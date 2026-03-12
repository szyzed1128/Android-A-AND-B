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
	// Token: 0x02000079 RID: 121
	public class InjectorPidIdToPIDConverter : IValueConverter
	{
		// Token: 0x0600028C RID: 652 RVA: 0x000185D0 File Offset: 0x000167D0
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<PID> injectorPIDCollection = SharedSettings.Current.InjectorPIDCollection;
			if (value is int)
			{
				int id = (int)value;
				PID pid = injectorPIDCollection.FirstOrDefault((PID x) => x.Id == id);
				if (pid == null)
				{
					pid = injectorPIDCollection.FirstOrDefault<PID>();
				}
				return pid;
			}
			return injectorPIDCollection.FirstOrDefault<PID>();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00018627 File Offset: 0x00016827
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return 122;
			}
			if (value is PID)
			{
				return (value as PID).Id;
			}
			return 122;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00002050 File Offset: 0x00000250
		public InjectorPidIdToPIDConverter()
		{
		}

		// Token: 0x0200007A RID: 122
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x0600028F RID: 655 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06000290 RID: 656 RVA: 0x00018654 File Offset: 0x00016854
			internal bool <Convert>b__0(PID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x040001AA RID: 426
			public int id;
		}
	}
}
