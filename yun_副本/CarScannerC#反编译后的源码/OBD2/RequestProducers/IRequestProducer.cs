using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.OBD2.RequestProducers
{
	// Token: 0x020003CF RID: 975
	internal interface IRequestProducer
	{
		// Token: 0x060027BE RID: 10174
		void AddRequests(List<OBDRequest> requests);
	}
}
