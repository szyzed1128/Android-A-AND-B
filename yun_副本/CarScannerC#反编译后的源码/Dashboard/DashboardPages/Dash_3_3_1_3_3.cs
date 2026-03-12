using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200078D RID: 1933
	internal class Dash_3_3_1_3_3 : DashboardPage
	{
		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x06004264 RID: 16996 RVA: 0x0033C889 File Offset: 0x0033AA89
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3_3_1_3_3;
			}
		}

		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x06004265 RID: 16997 RVA: 0x0033C88D File Offset: 0x0033AA8D
		public override string PreviewFile
		{
			get
			{
				return "dash_3_3_1_3_3.png";
			}
		}

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x00019412 File Offset: 0x00017612
		public override int ItemsCount
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x0033C894 File Offset: 0x0033AA94
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 3);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 1, 0);
				base.SetItemPosition(4, 1, 1);
				base.SetItemPosition(5, 1, 2);
				base.SetItemPosition(6, 2, 0, 2, 3);
				base.SetItemPosition(7, 4, 0);
				base.SetItemPosition(8, 4, 1);
				base.SetItemPosition(9, 4, 2);
				base.SetItemPosition(10, 5, 0);
				base.SetItemPosition(11, 5, 1);
				base.SetItemPosition(12, 5, 2);
			}
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x0033C930 File Offset: 0x0033AB30
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 6);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 0, 1);
				base.SetItemPosition(4, 1, 1);
				base.SetItemPosition(5, 2, 1);
				base.SetItemPosition(6, 0, 2, 3, 2);
				base.SetItemPosition(7, 0, 4);
				base.SetItemPosition(8, 1, 4);
				base.SetItemPosition(9, 2, 4);
				base.SetItemPosition(10, 0, 5);
				base.SetItemPosition(11, 1, 5);
				base.SetItemPosition(12, 2, 5);
			}
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3_3_1_3_3()
		{
		}
	}
}
