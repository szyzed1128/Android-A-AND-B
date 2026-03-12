using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200079C RID: 1948
	internal class Dash_8vertical : DashboardPage
	{
		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x060042BE RID: 17086 RVA: 0x001FFE5F File Offset: 0x001FE05F
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_8vertical;
			}
		}

		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x060042BF RID: 17087 RVA: 0x0033D8A1 File Offset: 0x0033BAA1
		public override string PreviewFile
		{
			get
			{
				return "dash_8vertical.png";
			}
		}

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x0020019E File Offset: 0x001FE39E
		public override int ItemsCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x0033D8A8 File Offset: 0x0033BAA8
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(8, 1);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 1, 0, 1, 1);
				base.SetItemPosition(2, 2, 0, 1, 1);
				base.SetItemPosition(3, 3, 0, 1, 1);
				base.SetItemPosition(4, 4, 0, 1, 1);
				base.SetItemPosition(5, 5, 0, 1, 1);
				base.SetItemPosition(6, 6, 0, 1, 1);
				base.SetItemPosition(7, 7, 0, 1, 1);
			}
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x0033D920 File Offset: 0x0033BB20
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 2);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 1, 0, 1, 1);
				base.SetItemPosition(2, 2, 0, 1, 1);
				base.SetItemPosition(3, 3, 0, 1, 1);
				base.SetItemPosition(4, 0, 1, 1, 1);
				base.SetItemPosition(5, 1, 1, 1, 1);
				base.SetItemPosition(6, 2, 1, 1, 1);
				base.SetItemPosition(7, 3, 1, 1, 1);
			}
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_8vertical()
		{
		}
	}
}
