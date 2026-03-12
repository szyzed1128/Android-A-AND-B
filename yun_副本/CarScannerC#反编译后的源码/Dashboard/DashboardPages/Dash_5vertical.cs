using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000794 RID: 1940
	public class Dash_5vertical : DashboardPage
	{
		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x0600428E RID: 17038 RVA: 0x002005C2 File Offset: 0x001FE7C2
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_5Vertical;
			}
		}

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x0033D05B File Offset: 0x0033B25B
		public override string PreviewFile
		{
			get
			{
				return "dash_5vertical.png";
			}
		}

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06004290 RID: 17040 RVA: 0x0033BAF7 File Offset: 0x00339CF7
		public override int ItemsCount
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x0033D062 File Offset: 0x0033B262
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 1);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 3, 0);
				base.SetItemPosition(4, 4, 0);
			}
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x0033D0A1 File Offset: 0x0033B2A1
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(1, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 0, 3);
				base.SetItemPosition(4, 0, 4);
			}
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_5vertical()
		{
		}
	}
}
