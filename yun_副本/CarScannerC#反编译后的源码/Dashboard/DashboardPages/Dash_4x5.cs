using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000792 RID: 1938
	internal class Dash_4x5 : DashboardPage
	{
		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x0033CC73 File Offset: 0x0033AE73
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_4x5;
			}
		}

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x06004283 RID: 17027 RVA: 0x0033CC77 File Offset: 0x0033AE77
		public override string PreviewFile
		{
			get
			{
				return "dash_4x5.png";
			}
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x06004284 RID: 17028 RVA: 0x001FFD0E File Offset: 0x001FDF0E
		public override int ItemsCount
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x0033CC80 File Offset: 0x0033AE80
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 4);
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
			}
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x0033CD60 File Offset: 0x0033AF60
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 5);
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
			}
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_4x5()
		{
		}
	}
}
