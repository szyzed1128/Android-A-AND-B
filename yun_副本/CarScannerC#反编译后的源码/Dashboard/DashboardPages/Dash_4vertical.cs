using System;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000790 RID: 1936
	public class Dash_4vertical : DashboardPage
	{
		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x06004276 RID: 17014 RVA: 0x001ECD4C File Offset: 0x001EAF4C
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.FourVertical;
			}
		}

		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x06004277 RID: 17015 RVA: 0x0033CB31 File Offset: 0x0033AD31
		public override string PreviewFile
		{
			get
			{
				return "_4medium.png";
			}
		}

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06004278 RID: 17016 RVA: 0x001ECD4C File Offset: 0x001EAF4C
		public override int ItemsCount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x0033CB38 File Offset: 0x0033AD38
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(4, 1);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 1, 0);
				base.SetItemPosition(2, 2, 0);
				base.SetItemPosition(3, 3, 0);
			}
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x0033CB6E File Offset: 0x0033AD6E
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				base.SetRowsAndColumns(1, 4);
				base.SetItemPosition(0, 0, 0);
				base.SetItemPosition(1, 0, 1);
				base.SetItemPosition(2, 0, 2);
				base.SetItemPosition(3, 0, 3);
			}
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_4vertical()
		{
		}
	}
}
