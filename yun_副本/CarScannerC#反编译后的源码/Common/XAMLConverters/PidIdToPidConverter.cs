using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x0200083C RID: 2108
	internal class PidIdToPidConverter : IValueConverter
	{
		// Token: 0x0600484D RID: 18509 RVA: 0x00370818 File Offset: 0x0036EA18
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

		// Token: 0x0600484E RID: 18510 RVA: 0x00018897 File Offset: 0x00016A97
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is PID)
			{
				return (value as PID).Id;
			}
			return PID.Empty.Id;
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x00002050 File Offset: 0x00000250
		public PidIdToPidConverter()
		{
		}

		// Token: 0x0200083D RID: 2109
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06004850 RID: 18512 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06004851 RID: 18513 RVA: 0x003708C3 File Offset: 0x0036EAC3
			internal bool <Convert>b__0(PID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x06004852 RID: 18514 RVA: 0x003708C3 File Offset: 0x0036EAC3
			internal bool <Convert>b__1(CustomPID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x06004853 RID: 18515 RVA: 0x003708C3 File Offset: 0x0036EAC3
			internal bool <Convert>b__2(CustomPID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x040029DE RID: 10718
			public int id;
		}
	}
}
