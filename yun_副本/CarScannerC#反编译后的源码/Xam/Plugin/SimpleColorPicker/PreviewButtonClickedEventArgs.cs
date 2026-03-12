using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x0200000E RID: 14
	public class PreviewButtonClickedEventArgs
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000314A File Offset: 0x0000134A
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00003152 File Offset: 0x00001352
		public Color Color
		{
			[CompilerGenerated]
			get
			{
				return this.<Color>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Color>k__BackingField = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000315B File Offset: 0x0000135B
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003163 File Offset: 0x00001363
		public bool Handled
		{
			[CompilerGenerated]
			get
			{
				return this.<Handled>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Handled>k__BackingField = value;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002050 File Offset: 0x00000250
		public PreviewButtonClickedEventArgs()
		{
		}

		// Token: 0x04000039 RID: 57
		[CompilerGenerated]
		private Color <Color>k__BackingField;

		// Token: 0x0400003A RID: 58
		[CompilerGenerated]
		private bool <Handled>k__BackingField;
	}
}
