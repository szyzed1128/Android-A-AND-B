using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200007E RID: 126
	public class PID_IDToPIDConverter : IValueConverter
	{
		// Token: 0x0600029A RID: 666 RVA: 0x000187EC File Offset: 0x000169EC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is int))
			{
				return PID.Empty;
			}
			int id = (int)value;
			PID pid;
			if (id < 1001)
			{
				pid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == id);
			}
			else
			{
				pid = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x.Id == id);
				if (pid == null)
				{
					pid = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Id == id);
				}
			}
			if (pid == null)
			{
				return PID.Empty;
			}
			return pid;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00018897 File Offset: 0x00016A97
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is PID)
			{
				return (value as PID).Id;
			}
			return PID.Empty.Id;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00002050 File Offset: 0x00000250
		public PID_IDToPIDConverter()
		{
		}

		// Token: 0x0200007F RID: 127
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x0600029D RID: 669 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x0600029E RID: 670 RVA: 0x000188C4 File Offset: 0x00016AC4
			internal bool <Convert>b__0(PID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x0600029F RID: 671 RVA: 0x000188C4 File Offset: 0x00016AC4
			internal bool <Convert>b__1(CustomPID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x060002A0 RID: 672 RVA: 0x000188C4 File Offset: 0x00016AC4
			internal bool <Convert>b__2(CustomPID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x040001AC RID: 428
			public int id;
		}
	}
}
