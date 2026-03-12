using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200077F RID: 1919
	public class Dash_1big4small_top : DashboardPage
	{
		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.FiveBigTop;
			}
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x0033BAF0 File Offset: 0x00339CF0
		public override string PreviewFile
		{
			get
			{
				return "_1big4small_top.png";
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x0033BAF7 File Offset: 0x00339CF7
		public override int ItemsCount
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x0033BAFC File Offset: 0x00339CFC
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 2);
				base.SetItemPosition(0, 0, 0, 2, 2);
				base.SetItemPosition(1, 2, 0);
				base.SetItemPosition(2, 2, 1);
				base.SetItemPosition(3, 3, 0);
				base.SetItemPosition(4, 3, 1);
			}
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0033BB48 File Offset: 0x00339D48
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 4);
				base.SetItemPosition(0, 0, 0, 2, 2);
				base.SetItemPosition(1, 1, 2);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 1, 3);
				base.SetItemPosition(4, 0, 3);
			}
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_1big4small_top()
		{
		}
	}
}
