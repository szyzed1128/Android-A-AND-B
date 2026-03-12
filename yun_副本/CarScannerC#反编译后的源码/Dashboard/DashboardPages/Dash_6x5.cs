using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000799 RID: 1945
	internal class Dash_6x5 : DashboardPage
	{
		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x060042AC RID: 17068 RVA: 0x0033D47F File Offset: 0x0033B67F
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_6x5;
			}
		}

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x0033D483 File Offset: 0x0033B683
		public override string PreviewFile
		{
			get
			{
				return "dash_6x5.png";
			}
		}

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x060042AE RID: 17070 RVA: 0x0033D200 File Offset: 0x0033B400
		public override int ItemsCount
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x0033D48C File Offset: 0x0033B68C
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 5);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 0, 3);
				base.SetItemPosition(4, 0, 4);
				base.SetItemPosition(5, 1, 0);
				base.SetItemPosition(6, 1, 1);
				base.SetItemPosition(7, 1, 2);
				base.SetItemPosition(8, 1, 3);
				base.SetItemPosition(9, 1, 4);
				base.SetItemPosition(10, 2, 0);
				base.SetItemPosition(11, 2, 1);
				base.SetItemPosition(12, 2, 2);
				base.SetItemPosition(13, 2, 3);
				base.SetItemPosition(14, 2, 4);
				base.SetItemPosition(15, 3, 0);
				base.SetItemPosition(16, 3, 1);
				base.SetItemPosition(17, 3, 2);
				base.SetItemPosition(18, 3, 3);
				base.SetItemPosition(19, 3, 4);
				base.SetItemPosition(20, 4, 0);
				base.SetItemPosition(21, 4, 1);
				base.SetItemPosition(22, 4, 2);
				base.SetItemPosition(23, 4, 3);
				base.SetItemPosition(24, 4, 4);
				base.SetItemPosition(25, 5, 0);
				base.SetItemPosition(26, 5, 1);
				base.SetItemPosition(27, 5, 2);
				base.SetItemPosition(28, 5, 3);
				base.SetItemPosition(29, 5, 4);
			}
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x0033D5D0 File Offset: 0x0033B7D0
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 6);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 3, 0);
				base.SetItemPosition(4, 4, 0);
				base.SetItemPosition(5, 0, 1);
				base.SetItemPosition(6, 1, 1);
				base.SetItemPosition(7, 2, 1);
				base.SetItemPosition(8, 3, 1);
				base.SetItemPosition(9, 4, 1);
				base.SetItemPosition(10, 0, 2);
				base.SetItemPosition(11, 1, 2);
				base.SetItemPosition(12, 2, 2);
				base.SetItemPosition(13, 3, 2);
				base.SetItemPosition(14, 4, 2);
				base.SetItemPosition(15, 0, 3);
				base.SetItemPosition(16, 1, 3);
				base.SetItemPosition(17, 2, 3);
				base.SetItemPosition(18, 3, 3);
				base.SetItemPosition(19, 4, 3);
				base.SetItemPosition(20, 0, 4);
				base.SetItemPosition(21, 1, 4);
				base.SetItemPosition(22, 2, 4);
				base.SetItemPosition(23, 3, 4);
				base.SetItemPosition(24, 4, 4);
				base.SetItemPosition(25, 0, 5);
				base.SetItemPosition(26, 1, 5);
				base.SetItemPosition(27, 2, 5);
				base.SetItemPosition(28, 3, 5);
				base.SetItemPosition(29, 4, 5);
			}
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_6x5()
		{
		}
	}
}
