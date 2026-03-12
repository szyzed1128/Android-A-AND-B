using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000795 RID: 1941
	internal class Dash_5x2 : DashboardPage
	{
		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x06004294 RID: 17044 RVA: 0x0020001C File Offset: 0x001FE21C
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_5x2;
			}
		}

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06004295 RID: 17045 RVA: 0x0033D0E0 File Offset: 0x0033B2E0
		public override string PreviewFile
		{
			get
			{
				return "dash_5x2.png";
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06004296 RID: 17046 RVA: 0x0033C691 File Offset: 0x0033A891
		public override int ItemsCount
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x0033D0E8 File Offset: 0x0033B2E8
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 2);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 0, 1, 1, 1);
				base.SetItemPosition(2, 1, 0, 1, 1);
				base.SetItemPosition(3, 1, 1, 1, 1);
				base.SetItemPosition(4, 2, 0, 1, 1);
				base.SetItemPosition(5, 2, 1, 1, 1);
				base.SetItemPosition(6, 3, 0, 1, 1);
				base.SetItemPosition(7, 3, 1, 1, 1);
				base.SetItemPosition(8, 4, 0, 1, 1);
				base.SetItemPosition(9, 4, 1, 1, 1);
			}
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x0033D174 File Offset: 0x0033B374
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 5);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 1, 0, 1, 1);
				base.SetItemPosition(2, 0, 1, 1, 1);
				base.SetItemPosition(3, 1, 1, 1, 1);
				base.SetItemPosition(4, 0, 2, 1, 1);
				base.SetItemPosition(5, 1, 2, 1, 1);
				base.SetItemPosition(6, 0, 3, 1, 1);
				base.SetItemPosition(7, 1, 3, 1, 1);
				base.SetItemPosition(8, 0, 4, 1, 1);
				base.SetItemPosition(9, 1, 4, 1, 1);
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_5x2()
		{
		}
	}
}
