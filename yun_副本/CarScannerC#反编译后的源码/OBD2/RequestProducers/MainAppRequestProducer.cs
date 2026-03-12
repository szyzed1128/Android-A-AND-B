using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.OBD2.RequestProducers
{
	// Token: 0x020003D1 RID: 977
	internal class MainAppRequestProducer : IRequestProducer
	{
		// Token: 0x060027C3 RID: 10179 RVA: 0x001E8C56 File Offset: 0x001E6E56
		public void AddRequests(List<OBDRequest> requests)
		{
			AddRequestsDelegate @delegate = MainAppRequestProducer.Delegate;
			if (@delegate == null)
			{
				return;
			}
			@delegate(requests);
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x00002050 File Offset: 0x00000250
		public MainAppRequestProducer()
		{
		}

		// Token: 0x0400163F RID: 5695
		public static AddRequestsDelegate Delegate;
	}
}
