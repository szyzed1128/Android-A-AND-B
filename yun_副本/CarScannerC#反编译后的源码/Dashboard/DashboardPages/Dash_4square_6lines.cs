using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200078E RID: 1934
	internal class Dash_4square_6lines : DashboardPage
	{
		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x0033C9CB File Offset: 0x0033ABCB
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_4square_6lines;
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x0600426B RID: 17003 RVA: 0x0033C9CF File Offset: 0x0033ABCF
		public override string PreviewFile
		{
			get
			{
				return "dash_4square_6lines.png";
			}
		}

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x0600426C RID: 17004 RVA: 0x0033C691 File Offset: 0x0033A891
		public override int ItemsCount
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x0033C9D8 File Offset: 0x0033ABD8
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(10, 2);
				base.SetItemPosition(0, 0, 0, 2, 1);
				base.SetItemPosition(1, 0, 1, 2, 1);
				base.SetItemPosition(2, 2, 0, 2, 1);
				base.SetItemPosition(3, 2, 1, 2, 1);
				base.SetItemPosition(4, 4, 0, 1, 2);
				base.SetItemPosition(5, 5, 0, 1, 2);
				base.SetItemPosition(6, 6, 0, 1, 2);
				base.SetItemPosition(7, 7, 0, 1, 2);
				base.SetItemPosition(8, 8, 0, 1, 2);
				base.SetItemPosition(9, 9, 0, 1, 2);
			}
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x0033CA68 File Offset: 0x0033AC68
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 4);
				base.SetItemPosition(0, 0, 0, 3, 1);
				base.SetItemPosition(1, 0, 1, 3, 1);
				base.SetItemPosition(2, 3, 0, 3, 1);
				base.SetItemPosition(3, 3, 1, 3, 1);
				base.SetItemPosition(4, 0, 2, 1, 2);
				base.SetItemPosition(5, 1, 2, 1, 2);
				base.SetItemPosition(6, 2, 2, 1, 2);
				base.SetItemPosition(7, 3, 2, 1, 2);
				base.SetItemPosition(8, 4, 2, 1, 2);
				base.SetItemPosition(9, 5, 2, 1, 2);
			}
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_4square_6lines()
		{
		}
	}
}
