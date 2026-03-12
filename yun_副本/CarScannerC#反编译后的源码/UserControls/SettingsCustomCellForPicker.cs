using System;
using System.Linq;
using System.Runtime.CompilerServices;
using AiForms.Renderers;
using Xamarin.Forms;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005E1 RID: 1505
	internal class SettingsCustomCellForPicker : CustomCell
	{
		// Token: 0x060035D9 RID: 13785 RVA: 0x0026A949 File Offset: 0x00268B49
		public SettingsCustomCellForPicker()
		{
			base.Tapped += this.SettingsCustomCellForPicker_Tapped;
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x0026A963 File Offset: 0x00268B63
		private void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			(sender as Picker).HorizontalOptions = LayoutOptions.CenterAndExpand;
			(sender as Picker).HorizontalOptions = LayoutOptions.Center;
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x0026A988 File Offset: 0x00268B88
		private void SettingsCustomCellForPicker_Tapped(object sender, EventArgs e)
		{
			if (base.Content is Picker)
			{
				((Picker)base.Content).Focus();
				return;
			}
			if (base.Content is Layout)
			{
				View view = ((Layout<View>)base.Content).Children.FirstOrDefault((View x) => x is Picker);
				if (view != null)
				{
					view.Focus();
				}
			}
		}

		// Token: 0x020005E2 RID: 1506
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060035DC RID: 13788 RVA: 0x0026AA00 File Offset: 0x00268C00
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060035DD RID: 13789 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060035DE RID: 13790 RVA: 0x0026AA0C File Offset: 0x00268C0C
			internal bool <SettingsCustomCellForPicker_Tapped>b__2_0(View x)
			{
				return x is Picker;
			}

			// Token: 0x04002015 RID: 8213
			public static readonly SettingsCustomCellForPicker.<>c <>9 = new SettingsCustomCellForPicker.<>c();

			// Token: 0x04002016 RID: 8214
			public static Func<View, bool> <>9__2_0;
		}
	}
}
