using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x02000783 RID: 1923
	public class Dash_3vertical : DashboardPage
	{
		// Token: 0x06004228 RID: 16936 RVA: 0x0033BDFE File Offset: 0x00339FFE
		public Dash_3vertical()
		{
			this.Title = "3 in a row";
		}

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x06004229 RID: 16937 RVA: 0x00002076 File Offset: 0x00000276
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.ThreeInARow;
			}
		}

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x0600422A RID: 16938 RVA: 0x0033BE11 File Offset: 0x0033A011
		public override string PreviewFile
		{
			get
			{
				return "_3vertical.png";
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x0600422B RID: 16939 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		public override int ItemsCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x0033BE18 File Offset: 0x0033A018
		protected override void RotateVertical()
		{
			if (this.grid != null)
			{
				this.grid.ColumnDefinitions.Clear();
				this.grid.RowDefinitions.Clear();
				for (int i = 0; i < 3; i++)
				{
					RowDefinition rowDefinition = new RowDefinition();
					rowDefinition.Height = GridLength.Star;
					this.grid.RowDefinitions.Add(rowDefinition);
					Grid.SetRow(base.Items[i], i);
					Grid.SetColumn(base.Items[i], 0);
				}
			}
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x0033BEA0 File Offset: 0x0033A0A0
		protected override void RotateHorizontal()
		{
			if (this.grid != null)
			{
				this.grid.ColumnDefinitions.Clear();
				this.grid.RowDefinitions.Clear();
				for (int i = 0; i < 3; i++)
				{
					ColumnDefinition columnDefinition = new ColumnDefinition();
					columnDefinition.Width = GridLength.Star;
					this.grid.ColumnDefinitions.Add(columnDefinition);
					Grid.SetRow(base.Items[i], 0);
					Grid.SetColumn(base.Items[i], i);
				}
			}
		}
	}
}
