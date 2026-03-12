using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000789 RID: 1929
	internal class Dash_3_1_2_3 : DashboardPage
	{
		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x0600424C RID: 16972 RVA: 0x001FFD0E File Offset: 0x001FDF0E
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3_1_2_3;
			}
		}

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x0033C4B9 File Offset: 0x0033A6B9
		public override string PreviewFile
		{
			get
			{
				return "dash_3_1_2_3.png";
			}
		}

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x0600424E RID: 16974 RVA: 0x0033BF2E File Offset: 0x0033A12E
		public override int ItemsCount
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x0033C4C0 File Offset: 0x0033A6C0
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 6);
				base.SetItemPosition(0, 0, 0, 1, 2);
				base.SetItemPosition(1, 0, 2, 1, 2);
				base.SetItemPosition(2, 0, 4, 1, 2);
				base.SetItemPosition(3, 1, 0, 2, 6);
				base.SetItemPosition(4, 3, 0, 1, 3);
				base.SetItemPosition(5, 3, 3, 1, 3);
				base.SetItemPosition(6, 4, 0, 1, 2);
				base.SetItemPosition(7, 4, 2, 1, 2);
				base.SetItemPosition(8, 4, 4, 1, 2);
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0033C540 File Offset: 0x0033A740
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 5);
				base.SetItemPosition(0, 0, 0, 2, 1);
				base.SetItemPosition(1, 2, 0, 2, 1);
				base.SetItemPosition(2, 4, 0, 2, 1);
				base.SetItemPosition(3, 0, 1, 6, 2);
				base.SetItemPosition(4, 0, 3, 3, 1);
				base.SetItemPosition(5, 3, 3, 3, 1);
				base.SetItemPosition(6, 0, 4, 2, 1);
				base.SetItemPosition(7, 2, 4, 2, 1);
				base.SetItemPosition(8, 4, 4, 2, 1);
			}
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3_1_2_3()
		{
		}
	}
}
