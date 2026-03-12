using System;
using CarScannerXamarinForms.UserControls;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200077D RID: 1917
	internal class DashAbsLayout8_2 : DashAbsLayout8_1
	{
		// Token: 0x06004204 RID: 16900 RVA: 0x0033B5D6 File Offset: 0x003397D6
		public DashAbsLayout8_2()
		{
			this.sqLayout = new SquareLayout();
			base.Content = this.sqLayout;
		}

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x06004205 RID: 16901 RVA: 0x0033B5F5 File Offset: 0x003397F5
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.DashAbsLayout8_2;
			}
		}

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x0033B5F9 File Offset: 0x003397F9
		public override string PreviewFile
		{
			get
			{
				return "dashAbsLayout8_2.png";
			}
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x06004207 RID: 16903 RVA: 0x0020019E File Offset: 0x001FE39E
		public override int ItemsCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x0033B600 File Offset: 0x00339800
		protected override void RotateVertical()
		{
			if (this.sqLayout != null)
			{
				base.SetAbsLayoutParams(0, 0.0, 0.0, 1.0, 0.15, false, -1);
				base.SetAbsLayoutParams(1, 0.5, 0.55, 1.0, 0.45, true, -1);
				base.SetAbsLayoutParams(2, 0.0, 1.0, 1.0, 0.15, false, -1);
				base.SetAbsLayoutParams(3, 0.0, 0.17, 0.25, 0.25, true, -1);
				base.SetAbsLayoutParams(4, 0.5, 0.17, 0.25, 0.25, true, -1);
				base.SetAbsLayoutParams(5, 1.0, 0.17, 0.25, 0.25, true, -1);
				base.SetAbsLayoutParams(6, 0.0, 0.85, 0.3, 0.3, true, -1);
				base.SetAbsLayoutParams(7, 1.0, 0.85, 0.3, 0.3, true, -1);
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x0033B780 File Offset: 0x00339980
		protected override void RotateHorizontal()
		{
			base.RotateHorizontal();
		}
	}
}
