using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000793 RID: 1939
	internal class Dash_4x6 : DashboardPage
	{
		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x06004288 RID: 17032 RVA: 0x0033CE3F File Offset: 0x0033B03F
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_4x6;
			}
		}

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x06004289 RID: 17033 RVA: 0x0033CE43 File Offset: 0x0033B043
		public override string PreviewFile
		{
			get
			{
				return "dash_4x6.png";
			}
		}

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x0600428A RID: 17034 RVA: 0x00200840 File Offset: 0x001FEA40
		public override int ItemsCount
		{
			get
			{
				return 24;
			}
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x0033CE4C File Offset: 0x0033B04C
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 0, 3);
				base.SetItemPosition(4, 1, 0);
				base.SetItemPosition(5, 1, 1);
				base.SetItemPosition(6, 1, 2);
				base.SetItemPosition(7, 1, 3);
				base.SetItemPosition(8, 2, 0);
				base.SetItemPosition(9, 2, 1);
				base.SetItemPosition(10, 2, 2);
				base.SetItemPosition(11, 2, 3);
				base.SetItemPosition(12, 3, 0);
				base.SetItemPosition(13, 3, 1);
				base.SetItemPosition(14, 3, 2);
				base.SetItemPosition(15, 3, 3);
				base.SetItemPosition(16, 4, 0);
				base.SetItemPosition(17, 4, 1);
				base.SetItemPosition(18, 4, 2);
				base.SetItemPosition(19, 4, 3);
				base.SetItemPosition(20, 5, 0);
				base.SetItemPosition(21, 5, 1);
				base.SetItemPosition(22, 5, 2);
				base.SetItemPosition(23, 5, 3);
			}
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x0033CF54 File Offset: 0x0033B154
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 6);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 3, 0);
				base.SetItemPosition(4, 0, 1);
				base.SetItemPosition(5, 1, 1);
				base.SetItemPosition(6, 2, 1);
				base.SetItemPosition(7, 3, 1);
				base.SetItemPosition(8, 0, 2);
				base.SetItemPosition(9, 1, 2);
				base.SetItemPosition(10, 2, 2);
				base.SetItemPosition(11, 3, 2);
				base.SetItemPosition(12, 0, 3);
				base.SetItemPosition(13, 1, 3);
				base.SetItemPosition(14, 2, 3);
				base.SetItemPosition(15, 3, 3);
				base.SetItemPosition(16, 0, 4);
				base.SetItemPosition(17, 1, 4);
				base.SetItemPosition(18, 2, 4);
				base.SetItemPosition(19, 3, 4);
				base.SetItemPosition(20, 0, 5);
				base.SetItemPosition(21, 1, 5);
				base.SetItemPosition(22, 2, 5);
				base.SetItemPosition(23, 3, 5);
			}
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_4x6()
		{
		}
	}
}
