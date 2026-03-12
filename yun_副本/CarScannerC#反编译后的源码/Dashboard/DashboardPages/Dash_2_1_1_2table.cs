using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000781 RID: 1921
	public class Dash_2_1_1_2table : DashboardPage
	{
		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x0033BC76 File Offset: 0x00339E76
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_2_1_1_2table;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x0033BC7A File Offset: 0x00339E7A
		public override string PreviewFile
		{
			get
			{
				return "dash_2_1_1_2.png";
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x0600421E RID: 16926 RVA: 0x0033BC81 File Offset: 0x00339E81
		public override int ItemsCount
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x0033BC84 File Offset: 0x00339E84
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 2);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 1, 0, 1, 2);
				base.SetItemPosition(3, 2, 0, 1, 2);
				base.SetItemPosition(4, 3, 0);
				base.SetItemPosition(5, 3, 1);
			}
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x0033BCDC File Offset: 0x00339EDC
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 0, 1, 2, 1);
				base.SetItemPosition(3, 0, 2, 2, 1);
				base.SetItemPosition(4, 0, 3);
				base.SetItemPosition(5, 1, 3);
			}
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_2_1_1_2table()
		{
		}
	}
}
