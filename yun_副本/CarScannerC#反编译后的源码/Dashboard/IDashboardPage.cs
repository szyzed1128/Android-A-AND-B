using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Dashboard
{
	// Token: 0x02000776 RID: 1910
	public interface IDashboardPage
	{
		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x0600412C RID: 16684
		ObservableCollection<DashboardItem> Items { get; }

		// Token: 0x0600412D RID: 16685
		Task Start();

		// Token: 0x0600412E RID: 16686
		void Stop();

		// Token: 0x0600412F RID: 16687
		void Clear();

		// Token: 0x06004130 RID: 16688
		void RebuildItems();

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06004131 RID: 16689
		string Title { get; }

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06004132 RID: 16690
		DashboardTypes DashboardType { get; }

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x06004133 RID: 16691
		// (set) Token: 0x06004134 RID: 16692
		int ItemsCount { get; set; }
	}
}
