using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000784 RID: 1924
	internal class Dash_3x3 : DashboardPage
	{
		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x0600422E RID: 16942 RVA: 0x00200840 File Offset: 0x001FEA40
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3x3;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x0600422F RID: 16943 RVA: 0x0033BF27 File Offset: 0x0033A127
		public override string PreviewFile
		{
			get
			{
				return "dash_3x3.png";
			}
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x0033BF2E File Offset: 0x0033A12E
		public override int ItemsCount
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x0033BF34 File Offset: 0x0033A134
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 3);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 0, 1, 1, 1);
				base.SetItemPosition(2, 0, 2, 1, 1);
				base.SetItemPosition(3, 1, 0, 1, 1);
				base.SetItemPosition(4, 1, 1, 1, 1);
				base.SetItemPosition(5, 1, 2, 1, 1);
				base.SetItemPosition(6, 2, 0, 1, 1);
				base.SetItemPosition(7, 2, 1, 1, 1);
				base.SetItemPosition(8, 2, 2, 1, 1);
			}
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x0033BFB4 File Offset: 0x0033A1B4
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 3);
				base.SetItemPosition(0, 0, 0, 1, 1);
				base.SetItemPosition(1, 1, 0, 1, 1);
				base.SetItemPosition(2, 2, 0, 1, 1);
				base.SetItemPosition(3, 0, 1, 1, 1);
				base.SetItemPosition(4, 1, 1, 1, 1);
				base.SetItemPosition(5, 2, 1, 1, 1);
				base.SetItemPosition(6, 0, 2, 1, 1);
				base.SetItemPosition(7, 1, 2, 1, 1);
				base.SetItemPosition(8, 2, 2, 1, 1);
			}
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3x3()
		{
		}
	}
}
