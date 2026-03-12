using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000796 RID: 1942
	internal class Dash_5x5 : DashboardPage
	{
		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x0600429A RID: 17050 RVA: 0x0033D200 File Offset: 0x0033B400
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_5x5;
			}
		}

		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x0600429B RID: 17051 RVA: 0x0033D204 File Offset: 0x0033B404
		public override string PreviewFile
		{
			get
			{
				return "dash_5x5.png";
			}
		}

		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x0600429C RID: 17052 RVA: 0x0033BB94 File Offset: 0x00339D94
		public override int ItemsCount
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x0033D20C File Offset: 0x0033B40C
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 5);
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
			}
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x0033CC6B File Offset: 0x0033AE6B
		protected override void RotateHorizontal()
		{
			this.RotateVertical();
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_5x5()
		{
		}
	}
}
