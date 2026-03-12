using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000787 RID: 1927
	public class Dash_3_1_1_3table : DashboardPage
	{
		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x06004240 RID: 16960 RVA: 0x002003A2 File Offset: 0x001FE5A2
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_3_1_1_3;
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x06004241 RID: 16961 RVA: 0x0033C2E7 File Offset: 0x0033A4E7
		public override string PreviewFile
		{
			get
			{
				return "dash_3_1_1_3.png";
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06004242 RID: 16962 RVA: 0x0020019E File Offset: 0x001FE39E
		public override int ItemsCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x0033C2F0 File Offset: 0x0033A4F0
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 3);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 1, 0, 1, 3);
				base.SetItemPosition(4, 2, 0, 1, 3);
				base.SetItemPosition(5, 3, 0);
				base.SetItemPosition(6, 3, 1);
				base.SetItemPosition(7, 3, 2);
			}
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x0033C35C File Offset: 0x0033A55C
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 0, 1, 3, 1);
				base.SetItemPosition(4, 0, 2, 3, 1);
				base.SetItemPosition(5, 0, 3);
				base.SetItemPosition(6, 1, 3);
				base.SetItemPosition(7, 2, 3);
			}
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_3_1_1_3table()
		{
		}
	}
}
