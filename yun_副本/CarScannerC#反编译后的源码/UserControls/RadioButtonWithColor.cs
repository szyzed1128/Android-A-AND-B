using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005DD RID: 1501
	public class RadioButtonWithColor : RadioButton
	{
		// Token: 0x060035BF RID: 13759 RVA: 0x00269500 File Offset: 0x00267700
		public RadioButtonWithColor()
		{
			if (Device.RuntimePlatform == "iOS")
			{
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.Tap_Tapped;
				base.GestureRecognizers.Add(tapGestureRecognizer);
			}
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x00269548 File Offset: 0x00267748
		private void Tap_Tapped(object sender, EventArgs e)
		{
			base.SetValue(RadioButton.IsCheckedProperty, true);
		}
	}
}
