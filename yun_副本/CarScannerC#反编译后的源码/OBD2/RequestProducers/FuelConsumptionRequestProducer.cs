using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.RequestProducers
{
	// Token: 0x020003CD RID: 973
	internal class FuelConsumptionRequestProducer : IRequestProducer
	{
		// Token: 0x060027B9 RID: 10169 RVA: 0x001E8BF0 File Offset: 0x001E6DF0
		public void AddRequests(List<OBDRequest> requests)
		{
			if (SharedSettings.Current.AlwaysRecordFuelConsumption && RequestProducerStatic.CurrentWorkingMode == WorkingModes.Normal)
			{
				PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption);
				if (pid != null)
				{
					LiveDataPIDModel.GetRequests(pid, requests, null, "");
				}
			}
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x00002050 File Offset: 0x00000250
		public FuelConsumptionRequestProducer()
		{
		}

		// Token: 0x020003CE RID: 974
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060027BB RID: 10171 RVA: 0x001E8C4A File Offset: 0x001E6E4A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060027BC RID: 10172 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060027BD RID: 10173 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <AddRequests>b__0_0(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x0400163D RID: 5693
			public static readonly FuelConsumptionRequestProducer.<>c <>9 = new FuelConsumptionRequestProducer.<>c();

			// Token: 0x0400163E RID: 5694
			public static Func<PID, bool> <>9__0_0;
		}
	}
}
