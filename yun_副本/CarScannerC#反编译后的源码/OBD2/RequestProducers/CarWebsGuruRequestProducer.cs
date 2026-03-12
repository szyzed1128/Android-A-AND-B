using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.OBD2.RequestProducers
{
	// Token: 0x020003CB RID: 971
	internal class CarWebsGuruRequestProducer : IRequestProducer
	{
		// Token: 0x060027B4 RID: 10164 RVA: 0x001E8BCC File Offset: 0x001E6DCC
		public void AddRequests(List<OBDRequest> requests)
		{
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x00002050 File Offset: 0x00000250
		public CarWebsGuruRequestProducer()
		{
		}

		// Token: 0x020003CC RID: 972
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060027B6 RID: 10166 RVA: 0x001E8BD9 File Offset: 0x001E6DD9
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060027B7 RID: 10167 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060027B8 RID: 10168 RVA: 0x001E8BE5 File Offset: 0x001E6DE5
			internal bool <AddRequests>b__0_0(PID x)
			{
				return x.Role == Roles.Coolant;
			}

			// Token: 0x0400163B RID: 5691
			public static readonly CarWebsGuruRequestProducer.<>c <>9 = new CarWebsGuruRequestProducer.<>c();

			// Token: 0x0400163C RID: 5692
			public static Func<PID, bool> <>9__0_0;
		}
	}
}
