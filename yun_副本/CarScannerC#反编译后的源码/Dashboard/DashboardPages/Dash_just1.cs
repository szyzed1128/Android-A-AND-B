using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x020007A6 RID: 1958
	public class Dash_just1 : DashboardPage
	{
		// Token: 0x060042ED RID: 17133 RVA: 0x0001953C File Offset: 0x0001773C
		public Dash_just1()
		{
		}

		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x060042EE RID: 17134 RVA: 0x0033BF2E File Offset: 0x0033A12E
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Just1;
			}
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x0033EE24 File Offset: 0x0033D024
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
			Grid.SetRow(base.Items[0], 0);
			Grid.SetColumn(base.Items[0], 0);
			Grid.SetRowSpan(base.Items[0], 1);
			Grid.SetColumnSpan(base.Items[0], 1);
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x0033EEDC File Offset: 0x0033D0DC
		protected override void RotateHorizontal()
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
			Grid.SetRow(base.Items[0], 0);
			Grid.SetColumn(base.Items[0], 0);
			Grid.SetRowSpan(base.Items[0], 1);
			Grid.SetColumnSpan(base.Items[0], 1);
		}

		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x060042F1 RID: 17137 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public override int ItemsCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x060042F2 RID: 17138 RVA: 0x0033EF91 File Offset: 0x0033D191
		public override string PreviewFile
		{
			get
			{
				return "just1.png";
			}
		}
	}
}
