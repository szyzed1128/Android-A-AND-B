using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200078C RID: 1932
	internal class Dash_3_2_3 : DashboardPage
	{
		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x0033C78E File Offset: 0x0033A98E
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3_2_3;
			}
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x0600425F RID: 16991 RVA: 0x0033C792 File Offset: 0x0033A992
		public override string PreviewFile
		{
			get
			{
				return "dash_3_2_3.png";
			}
		}

		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x06004260 RID: 16992 RVA: 0x0020019E File Offset: 0x001FE39E
		public override int ItemsCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x0033C79C File Offset: 0x0033A99C
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 6);
				base.SetItemPosition(0, 0, 0, 1, 2);
				base.SetItemPosition(1, 0, 2, 1, 2);
				base.SetItemPosition(2, 0, 4, 1, 2);
				base.SetItemPosition(3, 1, 0, 1, 3);
				base.SetItemPosition(4, 1, 3, 1, 3);
				base.SetItemPosition(5, 2, 0, 1, 2);
				base.SetItemPosition(6, 2, 2, 1, 2);
				base.SetItemPosition(7, 2, 4, 1, 2);
			}
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x0033C814 File Offset: 0x0033AA14
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 3);
				base.SetItemPosition(0, 0, 0, 2, 1);
				base.SetItemPosition(1, 2, 0, 2, 1);
				base.SetItemPosition(2, 4, 0, 2, 1);
				base.SetItemPosition(3, 0, 1, 3, 1);
				base.SetItemPosition(4, 3, 1, 3, 1);
				base.SetItemPosition(5, 0, 2, 2, 1);
				base.SetItemPosition(6, 2, 2, 2, 1);
				base.SetItemPosition(7, 4, 2, 2, 1);
			}
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3_2_3()
		{
		}
	}
}
