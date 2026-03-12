using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.OBD2.RequestProducers
{
	// Token: 0x020003D4 RID: 980
	public static class RequestProducerStatic
	{
		// Token: 0x060027C7 RID: 10183 RVA: 0x001E8C84 File Offset: 0x001E6E84
		public static void UpdateOBDReaderRequests()
		{
			List<OBDRequest> list = new List<OBDRequest>();
			for (int i = 0; i < RequestProducerStatic.producers.Length; i++)
			{
				try
				{
					RequestProducerStatic.producers[i].AddRequests(list);
				}
				catch (Exception ex)
				{
					App.OBDReader.DebugWrite(string.Format("\r\n[requestProducer[{0}]error->{1}]\r\n", i, ex.ToString()));
				}
			}
			IEnumerable<OBDRequest> enumerable = OBDRequestQueueOptimizer.Optimize(list);
			App.OBDReader.ReplaceQueue(enumerable);
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x001E8D00 File Offset: 0x001E6F00
		// Note: this type is marked as 'beforefieldinit'.
		static RequestProducerStatic()
		{
		}

		// Token: 0x04001645 RID: 5701
		private static IRequestProducer[] producers = new IRequestProducer[]
		{
			new MainAppRequestProducer(),
			new CarPlayRequestProducer(),
			new FuelConsumptionRequestProducer(),
			new CarWebsGuruRequestProducer(),
			new PIDSelectorRequestProducer()
		};

		// Token: 0x04001646 RID: 5702
		public static WorkingModes CurrentWorkingMode = WorkingModes.Normal;
	}
}
