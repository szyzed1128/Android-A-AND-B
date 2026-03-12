using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200078F RID: 1935
	public class Dash_4table : DashboardPage
	{
		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x0033BAF7 File Offset: 0x00339CF7
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.FourTable;
			}
		}

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x06004271 RID: 17009 RVA: 0x0033CAF4 File Offset: 0x0033ACF4
		public override string PreviewFile
		{
			get
			{
				return "_4table.png";
			}
		}

		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x06004272 RID: 17010 RVA: 0x001ECD4C File Offset: 0x001EAF4C
		public override int ItemsCount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x0033CAFB File Offset: 0x0033ACFB
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 2);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 1, 0);
				base.SetItemPosition(3, 1, 1);
			}
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x0033CAFB File Offset: 0x0033ACFB
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 2);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 1, 0);
				base.SetItemPosition(3, 1, 1);
			}
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_4table()
		{
		}
	}
}
