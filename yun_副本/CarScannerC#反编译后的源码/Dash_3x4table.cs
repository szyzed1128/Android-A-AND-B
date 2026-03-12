using System;
using CarScannerXamarinForms.Dashboard;

namespace CarScannerXamarinForms
{
	// Token: 0x0200008C RID: 140
	public class Dash_3x4table : DashboardPage
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00019412 File Offset: 0x00017612
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3x4;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00019416 File Offset: 0x00017616
		public override string PreviewFile
		{
			get
			{
				return "dash_3x4.png";
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0001941D File Offset: 0x0001761D
		public override int ItemsCount
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00019424 File Offset: 0x00017624
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 3);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 1, 0);
				base.SetItemPosition(4, 1, 1);
				base.SetItemPosition(5, 1, 2);
				base.SetItemPosition(6, 2, 0);
				base.SetItemPosition(7, 2, 1);
				base.SetItemPosition(8, 2, 2);
				base.SetItemPosition(9, 3, 0);
				base.SetItemPosition(10, 3, 1);
				base.SetItemPosition(11, 3, 2);
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000194B0 File Offset: 0x000176B0
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 0, 3);
				base.SetItemPosition(4, 1, 0);
				base.SetItemPosition(5, 1, 1);
				base.SetItemPosition(6, 1, 2);
				base.SetItemPosition(7, 1, 3);
				base.SetItemPosition(8, 2, 0);
				base.SetItemPosition(9, 2, 1);
				base.SetItemPosition(10, 2, 2);
				base.SetItemPosition(11, 2, 3);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3x4table()
		{
		}
	}
}
