using System;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.Dashboard
{
	// Token: 0x02000775 RID: 1909
	public interface IDashboardItem
	{
		// Token: 0x06004117 RID: 16663
		void Start();

		// Token: 0x06004118 RID: 16664
		void Stop();

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x06004119 RID: 16665
		// (set) Token: 0x0600411A RID: 16666
		LiveDataPIDModel Model { get; set; }

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x0600411B RID: 16667
		// (set) Token: 0x0600411C RID: 16668
		int PID_Id { get; set; }

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x0600411D RID: 16669
		// (set) Token: 0x0600411E RID: 16670
		double Minimum { get; set; }

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x0600411F RID: 16671
		// (set) Token: 0x06004120 RID: 16672
		double Maximum { get; set; }

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x06004121 RID: 16673
		// (set) Token: 0x06004122 RID: 16674
		DashboardItemTypes ItemType { get; set; }

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x06004123 RID: 16675
		double Interval { get; }

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x06004124 RID: 16676
		// (set) Token: 0x06004125 RID: 16677
		double PositionX { get; set; }

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06004126 RID: 16678
		// (set) Token: 0x06004127 RID: 16679
		double PositionY { get; set; }

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x06004128 RID: 16680
		// (set) Token: 0x06004129 RID: 16681
		double DesiredWidth { get; set; }

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x0600412A RID: 16682
		// (set) Token: 0x0600412B RID: 16683
		double DesiredHeight { get; set; }
	}
}
