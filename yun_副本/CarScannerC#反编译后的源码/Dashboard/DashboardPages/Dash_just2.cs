using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x020007A7 RID: 1959
	public class Dash_just2 : DashboardPage
	{
		// Token: 0x060042F3 RID: 17139 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_just2()
		{
		}

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x060042F4 RID: 17140 RVA: 0x0033EF98 File Offset: 0x0033D198
		public override string PreviewFile
		{
			get
			{
				return "just2.png";
			}
		}

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x060042F5 RID: 17141 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public override int ItemsCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x060042F6 RID: 17142 RVA: 0x0033C691 File Offset: 0x0033A891
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Just2;
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x0033EFA0 File Offset: 0x0033D1A0
		protected override void RotateVertical()
		{
			this.grid.RowDefinitions.Clear();
			this.grid.ColumnDefinitions.Clear();
			this.grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Star
			});
			this.grid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Star
			});
			this.grid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Star
			});
			Grid.SetRow(base.Items[0], 0);
			Grid.SetColumn(base.Items[0], 0);
			Grid.SetRowSpan(base.Items[0], 1);
			Grid.SetColumnSpan(base.Items[0], 1);
			Grid.SetRow(base.Items[1], 1);
			Grid.SetColumn(base.Items[1], 0);
			Grid.SetRowSpan(base.Items[1], 1);
			Grid.SetColumnSpan(base.Items[1], 1);
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x0033F0C0 File Offset: 0x0033D2C0
		protected override void RotateHorizontal()
		{
			this.grid.RowDefinitions.Clear();
			this.grid.ColumnDefinitions.Clear();
			this.grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Star
			});
			this.grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Star
			});
			this.grid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Star
			});
			Grid.SetRow(base.Items[0], 0);
			Grid.SetColumn(base.Items[0], 0);
			Grid.SetRowSpan(base.Items[0], 1);
			Grid.SetColumnSpan(base.Items[0], 1);
			Grid.SetRow(base.Items[1], 0);
			Grid.SetColumn(base.Items[1], 1);
			Grid.SetRowSpan(base.Items[1], 1);
			Grid.SetColumnSpan(base.Items[1], 1);
		}
	}
}
