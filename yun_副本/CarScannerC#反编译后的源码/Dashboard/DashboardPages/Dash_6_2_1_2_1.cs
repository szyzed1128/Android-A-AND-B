using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200079A RID: 1946
	public class Dash_6_2_1_2_1 : DashboardPage
	{
		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x060042B2 RID: 17074 RVA: 0x0033CBAF File Offset: 0x0033ADAF
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_6_2_1_2_1;
			}
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x060042B3 RID: 17075 RVA: 0x0033D713 File Offset: 0x0033B913
		public override string PreviewFile
		{
			get
			{
				return "dash_6_2_1_2_1.png";
			}
		}

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x060042B4 RID: 17076 RVA: 0x0033BC81 File Offset: 0x00339E81
		public override int ItemsCount
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x0033D71C File Offset: 0x0033B91C
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
				base.SetItemPosition(5, 3, 0, 1, 2);
			}
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x0033D774 File Offset: 0x0033B974
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
				base.SetItemPosition(5, 0, 3, 2, 1);
			}
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_6_2_1_2_1()
		{
		}
	}
}
