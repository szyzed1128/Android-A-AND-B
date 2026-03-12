using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200004F RID: 79
	public class HyperLinkLabel : Label
	{
		// Token: 0x060001DC RID: 476 RVA: 0x00016878 File Offset: 0x00014A78
		static HyperLinkLabel()
		{
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0001690C File Offset: 0x00014B0C
		public HyperLinkLabel()
		{
			this.NavigateCommand = new Command(delegate
			{
				Device.OpenUri(new Uri(this.NavigateUri));
			});
			this._tapGestureRecognizer = new TapGestureRecognizer
			{
				Command = this.NavigateCommand
			};
			base.GestureRecognizers.Add(this._tapGestureRecognizer);
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0001695E File Offset: 0x00014B5E
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00016970 File Offset: 0x00014B70
		public string Subject
		{
			get
			{
				return (string)base.GetValue(HyperLinkLabel.SubjectProperty);
			}
			set
			{
				base.SetValue(HyperLinkLabel.SubjectProperty, value);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0001697E File Offset: 0x00014B7E
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00016990 File Offset: 0x00014B90
		public string NavigateUri
		{
			get
			{
				return (string)base.GetValue(HyperLinkLabel.NavigateUriProperty);
			}
			set
			{
				base.SetValue(HyperLinkLabel.NavigateUriProperty, value);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0001699E File Offset: 0x00014B9E
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x000169B0 File Offset: 0x00014BB0
		public ICommand NavigateCommand
		{
			get
			{
				return (ICommand)base.GetValue(HyperLinkLabel.NavigateCommandProperty);
			}
			set
			{
				base.SetValue(HyperLinkLabel.NavigateCommandProperty, value);
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000169C0 File Offset: 0x00014BC0
		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == "NavigateCommand")
			{
				base.GestureRecognizers.Remove(this._tapGestureRecognizer);
				this._tapGestureRecognizer = new TapGestureRecognizer
				{
					Command = this.NavigateCommand
				};
				base.GestureRecognizers.Add(this._tapGestureRecognizer);
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00016A1B File Offset: 0x00014C1B
		[CompilerGenerated]
		private void <.ctor>b__5_0()
		{
			Device.OpenUri(new Uri(this.NavigateUri));
		}

		// Token: 0x04000191 RID: 401
		public static readonly BindableProperty SubjectProperty = BindableProperty.Create("Subject", typeof(string), typeof(HyperLinkLabel), string.Empty, 2, null, null, null, null, null);

		// Token: 0x04000192 RID: 402
		public static readonly BindableProperty NavigateUriProperty = BindableProperty.Create("NavigateUri", typeof(string), typeof(HyperLinkLabel), string.Empty, 2, null, null, null, null, null);

		// Token: 0x04000193 RID: 403
		public static readonly BindableProperty NavigateCommandProperty = BindableProperty.Create("NavigateCommand", typeof(ICommand), typeof(HyperLinkLabel), null, 2, null, null, null, null, null);

		// Token: 0x04000194 RID: 404
		private TapGestureRecognizer _tapGestureRecognizer;
	}
}
