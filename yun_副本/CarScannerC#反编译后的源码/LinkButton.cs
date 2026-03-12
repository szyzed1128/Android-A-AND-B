using System;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200019E RID: 414
	public class LinkButton : Button
	{
		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x000A0669 File Offset: 0x0009E869
		// (set) Token: 0x06001671 RID: 5745 RVA: 0x000A067B File Offset: 0x0009E87B
		public bool IgnoreScaling
		{
			get
			{
				return (bool)base.GetValue(LinkButton.IgnoreScalingProperty);
			}
			set
			{
				base.SetValue(LinkButton.IgnoreScalingProperty, value);
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x000A068E File Offset: 0x0009E88E
		public LinkButton()
		{
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x000A0698 File Offset: 0x0009E898
		// Note: this type is marked as 'beforefieldinit'.
		static LinkButton()
		{
		}

		// Token: 0x0400069A RID: 1690
		public static readonly BindableProperty IgnoreScalingProperty = BindableProperty.Create("IgnoreScaling", typeof(bool), typeof(LinkButton), null, 2, null, null, null, null, null);
	}
}
