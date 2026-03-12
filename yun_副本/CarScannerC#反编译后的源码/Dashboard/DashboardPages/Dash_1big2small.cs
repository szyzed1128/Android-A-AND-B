using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200077E RID: 1918
	public class Dash_1big2small : DashboardPage
	{
		// Token: 0x0600420A RID: 16906 RVA: 0x0033B788 File Offset: 0x00339988
		public Dash_1big2small()
		{
			this.Title = "1 big and 2 small";
		}

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.OneAndTwo;
			}
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0033B79C File Offset: 0x0033999C
		protected override void RotateVertical()
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
			Grid.SetRowSpan(base.Items[0], 2);
			Grid.SetColumnSpan(base.Items[0], 2);
			Grid.SetRow(base.Items[1], 2);
			Grid.SetColumn(base.Items[1], 0);
			Grid.SetRowSpan(base.Items[1], 1);
			Grid.SetColumnSpan(base.Items[1], 1);
			Grid.SetRow(base.Items[2], 2);
			Grid.SetColumn(base.Items[2], 1);
			Grid.SetRowSpan(base.Items[2], 1);
			Grid.SetColumnSpan(base.Items[2], 1);
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x0033B944 File Offset: 0x00339B44
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
			Grid.SetRowSpan(base.Items[0], 2);
			Grid.SetColumnSpan(base.Items[0], 2);
			Grid.SetRow(base.Items[1], 0);
			Grid.SetColumn(base.Items[1], 2);
			Grid.SetRowSpan(base.Items[1], 1);
			Grid.SetColumnSpan(base.Items[1], 1);
			Grid.SetRow(base.Items[2], 1);
			Grid.SetColumn(base.Items[2], 2);
			Grid.SetRowSpan(base.Items[2], 1);
			Grid.SetColumnSpan(base.Items[2], 1);
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x0033BAE9 File Offset: 0x00339CE9
		public override string PreviewFile
		{
			get
			{
				return "_1big2small.png";
			}
		}

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		public override int ItemsCount
		{
			get
			{
				return 3;
			}
		}
	}
}
