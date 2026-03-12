using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200077B RID: 1915
	internal class DashAbsLayout7_2 : DashAbsLayout8_1
	{
		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x060041F4 RID: 16884 RVA: 0x0033AE56 File Offset: 0x00339056
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.DashAbsLayout7_2;
			}
		}

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x0033AE5A File Offset: 0x0033905A
		public override string PreviewFile
		{
			get
			{
				return "dashAbsLayout7_2.png";
			}
		}

		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x0033AB8C File Offset: 0x00338D8C
		public override int ItemsCount
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x0033AE64 File Offset: 0x00339064
		protected override void RotateVertical()
		{
			if (this.sqLayout != null)
			{
				base.SetAbsLayoutParams(0, 0.95, 0.5, 0.63, 0.63, true, -1);
				base.SetAbsLayoutParams(1, 0.95, 0.05, 0.33, 0.33, true, -1);
				base.SetAbsLayoutParams(2, 0.35, 0.1, 0.31, 0.31, true, -1);
				base.SetAbsLayoutParams(3, 0.0, 0.375, 0.29, 0.29, true, -1);
				base.SetAbsLayoutParams(4, 0.0, 0.625, 0.29, 0.29, true, -1);
				base.SetAbsLayoutParams(5, 0.35, 0.9, 0.31, 0.31, true, -1);
				base.SetAbsLayoutParams(6, 0.95, 0.95, 0.33, 0.33, true, -1);
			}
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x0033AFB8 File Offset: 0x003391B8
		protected override void RotateHorizontal()
		{
			if (this.sqLayout != null)
			{
				base.SetAbsLayoutParams(0, 0.5, 1.0, 0.7, 0.7, true, -1);
				base.SetAbsLayoutParams(1, 0.0, 1.0, 0.33, 0.33, true, -1);
				base.SetAbsLayoutParams(2, 0.1, 0.45, 0.31, 0.31, true, -1);
				base.SetAbsLayoutParams(3, 0.38, 0.005, 0.29, 0.29, true, -1);
				base.SetAbsLayoutParams(4, 0.62, 0.005, 0.29, 0.29, true, -1);
				base.SetAbsLayoutParams(5, 0.9, 0.45, 0.31, 0.31, true, -1);
				base.SetAbsLayoutParams(6, 1.0, 1.0, 0.33, 0.33, true, -1);
			}
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x0033AE4E File Offset: 0x0033904E
		public DashAbsLayout7_2()
		{
		}
	}
}
