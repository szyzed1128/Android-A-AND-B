using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200019A RID: 410
	public class ExtendedSlider : Slider
	{
		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x0600163E RID: 5694 RVA: 0x0009F93A File Offset: 0x0009DB3A
		// (set) Token: 0x0600163F RID: 5695 RVA: 0x0009F94C File Offset: 0x0009DB4C
		public double StepValue
		{
			get
			{
				return (double)base.GetValue(ExtendedSlider.CurrentStepValueProperty);
			}
			set
			{
				base.SetValue(ExtendedSlider.CurrentStepValueProperty, value);
			}
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0009F95F File Offset: 0x0009DB5F
		public ExtendedSlider()
		{
			base.ValueChanged += this.OnSliderValueChanged;
			base.SizeChanged += this.Handle_SizeChanged;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0009F98C File Offset: 0x0009DB8C
		private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
		{
			double num = Math.Round(e.NewValue / this.StepValue);
			base.Value = num * this.StepValue;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0009F9BC File Offset: 0x0009DBBC
		// Note: this type is marked as 'beforefieldinit'.
		static ExtendedSlider()
		{
		}

		// Token: 0x04000687 RID: 1671
		public static readonly BindableProperty CurrentStepValueProperty = BindableProperty.Create<ExtendedSlider, double>((ExtendedSlider p) => p.StepValue, 1.0, 2, null, null, null, null, null);

		// Token: 0x0200019B RID: 411
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001644 RID: 5700 RVA: 0x0009FA1B File Offset: 0x0009DC1B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001645 RID: 5701 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x04000688 RID: 1672
			public static readonly ExtendedSlider.<>c <>9 = new ExtendedSlider.<>c();
		}
	}
}
