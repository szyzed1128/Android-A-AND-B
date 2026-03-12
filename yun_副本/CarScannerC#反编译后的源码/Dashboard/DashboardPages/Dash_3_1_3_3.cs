using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200078B RID: 1931
	public class Dash_3_1_3_3 : DashboardPage
	{
		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06004258 RID: 16984 RVA: 0x0033C686 File Offset: 0x0033A886
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3_1_3_3;
			}
		}

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06004259 RID: 16985 RVA: 0x0033C68A File Offset: 0x0033A88A
		public override string PreviewFile
		{
			get
			{
				return "dash_3_1_3_3.png";
			}
		}

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x0600425A RID: 16986 RVA: 0x0033C691 File Offset: 0x0033A891
		public override int ItemsCount
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x0033C698 File Offset: 0x0033A898
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 3);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 1, 0, 2, 3);
				base.SetItemPosition(4, 3, 0);
				base.SetItemPosition(5, 3, 1);
				base.SetItemPosition(6, 3, 2);
				base.SetItemPosition(7, 4, 0);
				base.SetItemPosition(8, 4, 1);
				base.SetItemPosition(9, 4, 2);
			}
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x0033C714 File Offset: 0x0033A914
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 5);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 0, 1, 3, 2);
				base.SetItemPosition(4, 0, 3);
				base.SetItemPosition(5, 1, 3);
				base.SetItemPosition(6, 2, 3);
				base.SetItemPosition(7, 0, 4);
				base.SetItemPosition(8, 1, 4);
				base.SetItemPosition(9, 2, 4);
			}
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3_1_3_3()
		{
		}
	}
}
