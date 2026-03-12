using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000788 RID: 1928
	public class Dash_3_1_2_2 : DashboardPage
	{
		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x06004246 RID: 16966 RVA: 0x0020066C File Offset: 0x001FE86C
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3_1_2_2;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x06004247 RID: 16967 RVA: 0x0033C3C5 File Offset: 0x0033A5C5
		public override string PreviewFile
		{
			get
			{
				return "dash_3_1_2_2.png";
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x06004248 RID: 16968 RVA: 0x0020019E File Offset: 0x001FE39E
		public override int ItemsCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x0033C3CC File Offset: 0x0033A5CC
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(5, 6);
				base.SetItemPosition(0, 0, 0, 1, 2);
				base.SetItemPosition(1, 0, 2, 1, 2);
				base.SetItemPosition(2, 0, 4, 1, 2);
				base.SetItemPosition(3, 1, 0, 2, 6);
				base.SetItemPosition(4, 3, 0, 1, 3);
				base.SetItemPosition(5, 3, 3, 1, 3);
				base.SetItemPosition(6, 4, 0, 1, 3);
				base.SetItemPosition(7, 4, 3, 1, 3);
			}
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x0033C444 File Offset: 0x0033A644
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(6, 4);
				base.SetItemPosition(0, 0, 0, 2, 1);
				base.SetItemPosition(1, 2, 0, 2, 1);
				base.SetItemPosition(2, 4, 0, 2, 1);
				base.SetItemPosition(3, 0, 1, 6, 2);
				base.SetItemPosition(4, 0, 3, 3, 1);
				base.SetItemPosition(5, 3, 3, 3, 1);
				base.SetItemPosition(6, 0, 4, 3, 1);
				base.SetItemPosition(7, 3, 4, 3, 1);
			}
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3_1_2_2()
		{
		}
	}
}
