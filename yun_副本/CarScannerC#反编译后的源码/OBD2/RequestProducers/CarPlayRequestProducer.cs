using System;
using System.Collections.Generic;
using CarScannerXamarinForms.CarPlay;

namespace CarScannerXamarinForms.OBD2.RequestProducers
{
	// Token: 0x020003CA RID: 970
	internal class CarPlayRequestProducer : IRequestProducer
	{
		// Token: 0x060027B2 RID: 10162 RVA: 0x001E8BBA File Offset: 0x001E6DBA
		public void AddRequests(List<OBDRequest> requests)
		{
			if (RequestProducerStatic.CurrentWorkingMode == WorkingModes.Normal)
			{
				CarPlayManager.AddRequests(requests);
			}
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x00002050 File Offset: 0x00000250
		public CarPlayRequestProducer()
		{
		}
	}
}
