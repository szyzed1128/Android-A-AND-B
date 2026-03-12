using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200077A RID: 1914
	internal class DashAbsLayout7_1 : DashAbsLayout8_1
	{
		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x060041EE RID: 16878 RVA: 0x0033AB81 File Offset: 0x00338D81
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.DashAbsLayout7_1;
			}
		}

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x060041EF RID: 16879 RVA: 0x0033AB85 File Offset: 0x00338D85
		public override string PreviewFile
		{
			get
			{
				return "dashAbsLayout7_1.png";
			}
		}

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x060041F0 RID: 16880 RVA: 0x0033AB8C File Offset: 0x00338D8C
		public override int ItemsCount
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x0033AB90 File Offset: 0x00338D90
		protected override void RotateVertical()
		{
			if (this.sqLayout != null)
			{
				base.SetAbsLayoutParams(0, 0.0, 0.0, 1.0, 0.15, false, -1);
				base.SetAbsLayoutParams(1, 0.5, 0.65, 1.0, 0.55, true, -1);
				base.SetAbsLayoutParams(2, 0.0, 0.17, 0.25, 0.25, true, -1);
				base.SetAbsLayoutParams(3, 0.5, 0.17, 0.25, 0.25, true, -1);
				base.SetAbsLayoutParams(4, 1.0, 0.17, 0.25, 0.25, true, -1);
				base.SetAbsLayoutParams(5, 0.0, 1.0, 0.3, 0.3, true, -1);
				base.SetAbsLayoutParams(6, 1.0, 1.0, 0.3, 0.3, true, -1);
			}
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x0033ACE4 File Offset: 0x00338EE4
		protected override void RotateHorizontal()
		{
			if (this.sqLayout != null)
			{
				base.SetAbsLayoutParams(0, 0.0, 0.0, 1.0, 0.15, false, -1);
				base.SetAbsLayoutParams(1, 0.425, 0.95, 1.0, 0.85, true, -1);
				base.SetAbsLayoutParams(2, 0.0, 0.21, 0.28, 0.28, true, -1);
				base.SetAbsLayoutParams(3, 0.0, 0.6, 0.28, 0.28, true, -1);
				base.SetAbsLayoutParams(4, 0.0, 1.0, 0.28, 0.28, true, -1);
				base.SetAbsLayoutParams(5, 1.0, 0.25, 0.4, 0.4, true, -1);
				base.SetAbsLayoutParams(6, 1.0, 1.0, 0.4, 0.4, true, -1);
				this.sqLayout.LowerChild(base.Items[1]);
			}
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x0033AE4E File Offset: 0x0033904E
		public DashAbsLayout7_1()
		{
		}
	}
}
