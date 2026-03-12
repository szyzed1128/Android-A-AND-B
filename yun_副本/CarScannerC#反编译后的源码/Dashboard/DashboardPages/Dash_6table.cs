using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000798 RID: 1944
	public class Dash_6table : DashboardPage
	{
		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x060042A6 RID: 17062 RVA: 0x0033BC81 File Offset: 0x00339E81
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.SixTable;
			}
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x060042A7 RID: 17063 RVA: 0x0033D3D0 File Offset: 0x0033B5D0
		public override string PreviewFile
		{
			get
			{
				return "_6medium.png";
			}
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x060042A8 RID: 17064 RVA: 0x0033BC81 File Offset: 0x00339E81
		public override int ItemsCount
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x0033D3D8 File Offset: 0x0033B5D8
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(3, 2);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 1, 0);
				base.SetItemPosition(3, 1, 1);
				base.SetItemPosition(4, 2, 0);
				base.SetItemPosition(5, 2, 1);
			}
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x0033D42C File Offset: 0x0033B62C
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(2, 3);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 0, 1);
				base.SetItemPosition(3, 1, 1);
				base.SetItemPosition(4, 0, 2);
				base.SetItemPosition(5, 1, 2);
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_6table()
		{
		}
	}
}
