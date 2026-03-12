using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000791 RID: 1937
	internal class Dash_4x4 : DashboardPage
	{
		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x0600427C RID: 17020 RVA: 0x0033CBA4 File Offset: 0x0033ADA4
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Dash_4x4;
			}
		}

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x0600427D RID: 17021 RVA: 0x0033CBA8 File Offset: 0x0033ADA8
		public override string PreviewFile
		{
			get
			{
				return "dash_4x4.png";
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x0600427E RID: 17022 RVA: 0x0033CBAF File Offset: 0x0033ADAF
		public override int ItemsCount
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x0033CBB4 File Offset: 0x0033ADB4
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 4);
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
				base.SetItemPosition(12, 3, 0);
				base.SetItemPosition(13, 3, 1);
				base.SetItemPosition(14, 3, 2);
				base.SetItemPosition(15, 3, 3);
			}
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x0033CC6B File Offset: 0x0033AE6B
		protected override void RotateHorizontal()
		{
			this.RotateVertical();
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_4x4()
		{
		}
	}
}
