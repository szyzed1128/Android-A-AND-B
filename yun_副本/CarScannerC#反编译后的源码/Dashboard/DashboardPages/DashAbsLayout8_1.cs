using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200077C RID: 1916
	internal class DashAbsLayout8_1 : DashboardPage
	{
		// Token: 0x060041FA RID: 16890 RVA: 0x0033B10C File Offset: 0x0033930C
		public DashAbsLayout8_1()
		{
			this.sqLayout = new SquareLayout();
			base.Content = this.sqLayout;
			TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.TapGesture_Tapped;
			this.sqLayout.GestureRecognizers.Add(tapGestureRecognizer);
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x0033B160 File Offset: 0x00339360
		private void TapGesture_Tapped(object sender, EventArgs e)
		{
			DashboardXamlPage dashboardXamlPage = App.GetCurrentPage() as DashboardXamlPage;
			if (dashboardXamlPage != null && dashboardXamlPage != null)
			{
				dashboardXamlPage.ShowTopGrid();
			}
		}

		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x060041FC RID: 16892 RVA: 0x0033B185 File Offset: 0x00339385
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.DashAbsLayout8_1;
			}
		}

		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x0033B189 File Offset: 0x00339389
		public override string PreviewFile
		{
			get
			{
				return "dashAbsLayout8_1.png";
			}
		}

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x0020019E File Offset: 0x001FE39E
		public override int ItemsCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x0033B190 File Offset: 0x00339390
		public override void CreateGrid()
		{
			this.models = new ObservableCollection<LiveDataPIDModel>();
			base.Items.Clear();
			for (int i = 0; i < this.ItemsCount; i++)
			{
				DashboardItem dashboardItem = new DashboardItem();
				dashboardItem.Model = new LiveDataPIDModel
				{
					DoubleFormat = dashboardItem.ValueFormat
				};
				base.Items.Add(dashboardItem);
				this.sqLayout.Children.Add(dashboardItem);
			}
			foreach (View view in this.sqLayout.Children.Where((View x) => !base.Items.Contains(x)).ToArray<View>())
			{
				this.sqLayout.Children.Remove(view);
			}
			if (base.Height >= base.Width)
			{
				this.RotateVertical();
				return;
			}
			this.RotateHorizontal();
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x0033B268 File Offset: 0x00339468
		protected override void RotateVertical()
		{
			if (this.sqLayout != null)
			{
				this.SetAbsLayoutParams(0, 0.0, 0.0, 1.0, 0.15, false, -1);
				this.SetAbsLayoutParams(1, 0.5, 0.55, 1.0, 0.45, true, -1);
				this.SetAbsLayoutParams(2, 0.0, 1.0, 1.0, 0.15, false, -1);
				this.SetAbsLayoutParams(3, 0.0, 0.2, 0.25, 0.25, true, -1);
				this.SetAbsLayoutParams(4, 0.5, 0.15, 0.25, 0.25, true, -1);
				this.SetAbsLayoutParams(5, 1.0, 0.2, 0.25, 0.25, true, -1);
				this.SetAbsLayoutParams(6, 0.0, 0.85, 0.3, 0.3, true, -1);
				this.SetAbsLayoutParams(7, 1.0, 0.85, 0.3, 0.3, true, -1);
			}
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x0033B3E8 File Offset: 0x003395E8
		protected override void RotateHorizontal()
		{
			if (this.sqLayout != null)
			{
				this.SetAbsLayoutParams(0, 0.0, 0.0, 1.0, 0.15, false, -1);
				this.SetAbsLayoutParams(1, 0.5, 0.5, 0.7, 0.7, true, -1);
				this.SetAbsLayoutParams(2, 0.0, 1.0, 1.0, 0.15, false, -1);
				this.SetAbsLayoutParams(3, 0.0, 0.2, 0.22, 0.22, true, -1);
				this.SetAbsLayoutParams(4, 0.0, 0.5, 0.22, 0.22, true, -1);
				this.SetAbsLayoutParams(5, 0.0, 0.8, 0.22, 0.22, true, -1);
				this.SetAbsLayoutParams(6, 1.0, 0.22, 0.35, 0.35, true, -1);
				this.SetAbsLayoutParams(7, 1.0, 0.78, 0.35, 0.35, true, -1);
				this.sqLayout.LowerChild(base.Items[1]);
			}
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x0033B580 File Offset: 0x00339780
		protected void SetAbsLayoutParams(int itemIdx, double x, double y, double w, double h, bool isSquare, AbsoluteLayoutFlags flags = -1)
		{
			if (itemIdx < base.Items.Count)
			{
				DashboardItem dashboardItem = base.Items[itemIdx];
				AbsoluteLayout.SetLayoutBounds(dashboardItem, default(Rectangle));
				AbsoluteLayout.SetLayoutBounds(dashboardItem, new Rectangle(x, y, w, h));
				AbsoluteLayout.SetLayoutFlags(dashboardItem, flags);
				SquareLayout.SetIsSquare(dashboardItem, isSquare);
			}
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x0033920B File Offset: 0x0033740B
		[CompilerGenerated]
		private bool <CreateGrid>b__9_0(View x)
		{
			return !base.Items.Contains(x);
		}

		// Token: 0x04002873 RID: 10355
		protected SquareLayout sqLayout;
	}
}
