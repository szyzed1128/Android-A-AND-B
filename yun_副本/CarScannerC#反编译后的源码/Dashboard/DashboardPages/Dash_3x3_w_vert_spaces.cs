using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000785 RID: 1925
	internal class Dash_3x3_w_vert_spaces : DashboardPage
	{
		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x0033C034 File Offset: 0x0033A234
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3x3_w_vert_spaces;
			}
		}

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06004235 RID: 16949 RVA: 0x0033C038 File Offset: 0x0033A238
		public override string PreviewFile
		{
			get
			{
				return "dash_3x3_w_vert_spaces.png";
			}
		}

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x0033BF2E File Offset: 0x0033A12E
		public override int ItemsCount
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x0033C040 File Offset: 0x0033A240
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 9);
				base.SetItemPosition(0, 0, 0, 1, 3);
				base.SetItemPosition(1, 0, 3, 1, 3);
				base.SetItemPosition(2, 0, 6, 1, 3);
				base.SetItemPosition(3, 1, 0, 1, 2);
				base.SetItemPosition(4, 1, 2, 1, 5);
				base.SetItemPosition(5, 1, 7, 1, 2);
				base.SetItemPosition(6, 2, 0, 1, 3);
				base.SetItemPosition(7, 2, 3, 1, 3);
				base.SetItemPosition(8, 2, 6, 1, 3);
			}
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x0033C0C4 File Offset: 0x0033A2C4
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 9);
				base.SetItemPosition(0, 0, 0, 1, 2);
				base.SetItemPosition(1, 1, 0, 1, 2);
				base.SetItemPosition(2, 2, 0, 1, 2);
				base.SetItemPosition(3, 0, 2, 3, 1);
				base.SetItemPosition(4, 0, 3, 3, 3);
				base.SetItemPosition(5, 0, 6, 3, 1);
				base.SetItemPosition(6, 0, 7, 1, 2);
				base.SetItemPosition(7, 1, 7, 1, 2);
				base.SetItemPosition(8, 2, 7, 1, 2);
			}
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3x3_w_vert_spaces()
		{
		}
	}
}
