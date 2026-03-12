using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarScannerXamarinForms
{
	// Token: 0x02000090 RID: 144
	public interface ICustomInAppManager
	{
		// Token: 0x060002DB RID: 731
		Task<bool> CanMakePayments();

		// Token: 0x060002DC RID: 732
		void RequestProductData(List<string> productIds);

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002DD RID: 733
		// (remove) Token: 0x060002DE RID: 734
		event ProductsReceivedEvent OnProductsReceived;

		// Token: 0x060002DF RID: 735
		void Purchase(string productID);

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060002E0 RID: 736
		// (remove) Token: 0x060002E1 RID: 737
		event EventHandler<string> Error;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060002E2 RID: 738
		// (remove) Token: 0x060002E3 RID: 739
		event EventHandler ActionFinished;

		// Token: 0x060002E4 RID: 740
		void Restore();

		// Token: 0x060002E5 RID: 741
		void Initialize();

		// Token: 0x060002E6 RID: 742
		void FreeResources();
	}
}
