using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000786 RID: 1926
	internal class Dash_3x5table : DashboardPage
	{
		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x0033C145 File Offset: 0x0033A345
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3x5table;
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x0600423B RID: 16955 RVA: 0x0033C149 File Offset: 0x0033A349
		public override string PreviewFile
		{
			get
			{
				return "dash_3x5table.png";
			}
		}

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x002005C2 File Offset: 0x001FE7C2
		public override int ItemsCount
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x0033C150 File Offset: 0x0033A350
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 3);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 0, 1, 1, 1);
				base.SetItemPosition(2, 0, 2, 1, 1);
				base.SetItemPosition(3, 1, 0, 1, 1);
				base.SetItemPosition(4, 1, 1, 1, 1);
				base.SetItemPosition(5, 1, 2, 1, 1);
				base.SetItemPosition(6, 2, 0, 1, 1);
				base.SetItemPosition(7, 2, 1, 1, 1);
				base.SetItemPosition(8, 2, 2, 1, 1);
				base.SetItemPosition(9, 3, 0, 1, 1);
				base.SetItemPosition(10, 3, 1, 1, 1);
				base.SetItemPosition(11, 3, 2, 1, 1);
				base.SetItemPosition(12, 4, 0, 1, 1);
				base.SetItemPosition(13, 4, 1, 1, 1);
				base.SetItemPosition(14, 4, 2, 1, 1);
			}
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x0033C21C File Offset: 0x0033A41C
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 5);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 1, 0, 1, 1);
				base.SetItemPosition(2, 2, 0, 1, 1);
				base.SetItemPosition(3, 0, 1, 1, 1);
				base.SetItemPosition(4, 1, 1, 1, 1);
				base.SetItemPosition(5, 2, 1, 1, 1);
				base.SetItemPosition(6, 0, 2, 1, 1);
				base.SetItemPosition(7, 1, 2, 1, 1);
				base.SetItemPosition(8, 2, 2, 1, 1);
				base.SetItemPosition(9, 0, 3, 1, 1);
				base.SetItemPosition(10, 1, 3, 1, 1);
				base.SetItemPosition(11, 2, 3, 1, 1);
				base.SetItemPosition(12, 0, 4, 1, 1);
				base.SetItemPosition(13, 1, 4, 1, 1);
				base.SetItemPosition(14, 2, 4, 1, 1);
			}
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3x5table()
		{
		}
	}
}
