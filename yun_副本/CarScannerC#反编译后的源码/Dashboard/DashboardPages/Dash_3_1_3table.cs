using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200078A RID: 1930
	public class Dash_3_1_3table : DashboardPage
	{
		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06004252 RID: 16978 RVA: 0x0001941D File Offset: 0x0001761D
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3_1_3;
			}
		}

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x06004253 RID: 16979 RVA: 0x0033C5C0 File Offset: 0x0033A7C0
		public override string PreviewFile
		{
			get
			{
				return "dash_3_1_3.png";
			}
		}

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06004254 RID: 16980 RVA: 0x0033AB8C File Offset: 0x00338D8C
		public override int ItemsCount
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x0033C5C8 File Offset: 0x0033A7C8
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 3);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 1, 0, 1, 3);
				base.SetItemPosition(4, 2, 0);
				base.SetItemPosition(5, 2, 1);
				base.SetItemPosition(6, 2, 2);
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x0033C628 File Offset: 0x0033A828
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 3);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 0, 1, 3, 1);
				base.SetItemPosition(4, 0, 2);
				base.SetItemPosition(5, 1, 2);
				base.SetItemPosition(6, 2, 2);
			}
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3_1_3table()
		{
		}
	}
}
