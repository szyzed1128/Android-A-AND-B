using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000782 RID: 1922
	public class Dash_2_1_2_2 : DashboardPage
	{
		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x0033BD33 File Offset: 0x00339F33
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_2_1_2_2;
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x06004223 RID: 16931 RVA: 0x0033BD37 File Offset: 0x00339F37
		public override string PreviewFile
		{
			get
			{
				return "dash_2_1_2_2.png";
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x0033AB8C File Offset: 0x00338D8C
		public override int ItemsCount
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x0033BD40 File Offset: 0x00339F40
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 2);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 1, 0, 1, 2);
				base.SetItemPosition(3, 2, 0);
				base.SetItemPosition(4, 2, 1);
				base.SetItemPosition(5, 3, 0);
				base.SetItemPosition(6, 3, 1);
			}
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x0033BDA0 File Offset: 0x00339FA0
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 0, 1, 2, 1);
				base.SetItemPosition(3, 0, 2);
				base.SetItemPosition(4, 1, 2);
				base.SetItemPosition(5, 0, 3);
				base.SetItemPosition(6, 1, 3);
			}
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_2_1_2_2()
		{
		}
	}
}
