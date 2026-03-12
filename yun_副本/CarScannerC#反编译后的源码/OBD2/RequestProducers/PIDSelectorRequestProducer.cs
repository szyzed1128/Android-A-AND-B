using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.OBD2.RequestProducers
{
	// Token: 0x020003D2 RID: 978
	internal class PIDSelectorRequestProducer : IRequestProducer
	{
		// Token: 0x060027C5 RID: 10181 RVA: 0x001E8C68 File Offset: 0x001E6E68
		public void AddRequests(List<OBDRequest> requests)
		{
			if (RequestProducerStatic.CurrentWorkingMode == WorkingModes.Normal)
			{
				AddRequestsDelegate @delegate = PIDSelectorRequestProducer.Delegate;
				if (@delegate == null)
				{
					return;
				}
				@delegate(requests);
			}
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x00002050 File Offset: 0x00000250
		public PIDSelectorRequestProducer()
		{
		}

		// Token: 0x04001640 RID: 5696
		public static AddRequestsDelegate Delegate;
	}
}
