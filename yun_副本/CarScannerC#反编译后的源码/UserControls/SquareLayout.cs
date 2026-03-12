using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005E3 RID: 1507
	public class SquareLayout : AbsoluteLayout
	{
		// Token: 0x060035DF RID: 13791 RVA: 0x0026AA17 File Offset: 0x00268C17
		public static bool GetIsSquare(BindableObject view)
		{
			return (bool)view.GetValue(SquareLayout.IsSquareProperty);
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x0026AA29 File Offset: 0x00268C29
		public static void SetIsSquare(BindableObject view, bool value)
		{
			view.SetValue(SquareLayout.IsSquareProperty, value);
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x0026AA3C File Offset: 0x00268C3C
		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			foreach (View view in base.Children)
			{
				if (SquareLayout.GetIsSquare(view))
				{
					Rectangle layoutBounds = AbsoluteLayout.GetLayoutBounds(view);
					AbsoluteLayoutFlags layoutFlags = AbsoluteLayout.GetLayoutFlags(view);
					bool flag = (layoutFlags & 4) > 0;
					bool flag2 = (layoutFlags & 8) > 0;
					double num = (flag ? (layoutBounds.Width * width) : layoutBounds.Width);
					double num2 = (flag2 ? (layoutBounds.Height * height) : layoutBounds.Height);
					double num3 = Math.Min(num, num2);
					AbsoluteLayout.SetLayoutBounds(view, new Rectangle(layoutBounds.X, layoutBounds.Y, flag ? (num3 / width) : num3, flag2 ? (num3 / height) : num3));
				}
			}
			base.LayoutChildren(x, y, width, height);
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x0026AB24 File Offset: 0x00268D24
		public SquareLayout()
		{
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0026AB2C File Offset: 0x00268D2C
		// Note: this type is marked as 'beforefieldinit'.
		static SquareLayout()
		{
		}

		// Token: 0x04002017 RID: 8215
		public static readonly BindableProperty IsSquareProperty = BindableProperty.CreateAttached("IsSquare", typeof(bool), typeof(SquareLayout), true, 2, null, null, null, null, null);
	}
}
