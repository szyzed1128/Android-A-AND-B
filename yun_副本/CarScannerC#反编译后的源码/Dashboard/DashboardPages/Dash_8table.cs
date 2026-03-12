using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200079B RID: 1947
	public class Dash_8table : DashboardPage
	{
		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x060042B8 RID: 17080 RVA: 0x0033AB8C File Offset: 0x00338D8C
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.EightTable;
			}
		}

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x060042B9 RID: 17081 RVA: 0x0033D7CB File Offset: 0x0033B9CB
		public override string PreviewFile
		{
			get
			{
				return "_8small.png";
			}
		}

		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x060042BA RID: 17082 RVA: 0x0020019E File Offset: 0x001FE39E
		public override int ItemsCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x0033D7D4 File Offset: 0x0033B9D4
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 2);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 1, 0);
				base.SetItemPosition(3, 1, 1);
				base.SetItemPosition(4, 2, 0);
				base.SetItemPosition(5, 2, 1);
				base.SetItemPosition(6, 3, 0);
				base.SetItemPosition(7, 3, 1);
			}
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x0033D83C File Offset: 0x0033BA3C
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 0, 1);
				base.SetItemPosition(3, 1, 1);
				base.SetItemPosition(4, 0, 2);
				base.SetItemPosition(5, 1, 2);
				base.SetItemPosition(6, 0, 3);
				base.SetItemPosition(7, 1, 3);
			}
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_8table()
		{
		}
	}
}
