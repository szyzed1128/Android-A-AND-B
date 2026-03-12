using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000780 RID: 1920
	internal class Dash_1_6 : DashboardPage
	{
		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x0033BB94 File Offset: 0x00339D94
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_1_6;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x0033BB98 File Offset: 0x00339D98
		public override string PreviewFile
		{
			get
			{
				return "dash_1_6.png";
			}
		}

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x06004218 RID: 16920 RVA: 0x0033AB8C File Offset: 0x00338D8C
		public override int ItemsCount
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x0033BBA0 File Offset: 0x00339DA0
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 2);
				base.SetItemPosition(0, 0, 0, 3, 2);
				base.SetItemPosition(1, 3, 0, 1, 1);
				base.SetItemPosition(2, 3, 1, 1, 1);
				base.SetItemPosition(3, 4, 0, 1, 1);
				base.SetItemPosition(4, 4, 1, 1, 1);
				base.SetItemPosition(5, 5, 0, 1, 1);
				base.SetItemPosition(6, 5, 1, 1, 1);
			}
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x0033BC0C File Offset: 0x00339E0C
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 4);
				base.SetItemPosition(0, 0, 0, 3, 2);
				base.SetItemPosition(1, 0, 2, 1, 1);
				base.SetItemPosition(2, 0, 3, 1, 1);
				base.SetItemPosition(3, 1, 2, 1, 1);
				base.SetItemPosition(4, 1, 3, 1, 1);
				base.SetItemPosition(5, 2, 2, 1, 1);
				base.SetItemPosition(6, 2, 3, 1, 1);
			}
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_1_6()
		{
		}
	}
}
