using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000797 RID: 1943
	public class Dash_5_big_in_center : DashboardPage
	{
		// Token: 0x060042A0 RID: 17056 RVA: 0x0033D31D File Offset: 0x0033B51D
		public Dash_5_big_in_center()
		{
			this.Title = "5 items, big in cetner";
		}

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x060042A1 RID: 17057 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.FiveBigCenter;
			}
		}

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x060042A2 RID: 17058 RVA: 0x0033D330 File Offset: 0x0033B530
		public override string PreviewFile
		{
			get
			{
				return "_1big4small_center.png";
			}
		}

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x060042A3 RID: 17059 RVA: 0x0033BAF7 File Offset: 0x00339CF7
		public override int ItemsCount
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x0033D338 File Offset: 0x0033B538
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 2);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 1, 0, 2, 2);
				base.SetItemPosition(3, 3, 0);
				base.SetItemPosition(4, 3, 1);
			}
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x0033D384 File Offset: 0x0033B584
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 0, 1, 2, 2);
				base.SetItemPosition(3, 0, 3);
				base.SetItemPosition(4, 1, 3);
			}
		}
	}
}
