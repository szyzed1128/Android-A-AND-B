using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000009 RID: 9
	public class ColorDialogSettings : DialogSettings
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002776 File Offset: 0x00000976
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000277E File Offset: 0x0000097E
		public Color EditorsColor
		{
			[CompilerGenerated]
			get
			{
				return this.<EditorsColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EditorsColor>k__BackingField = value;
			}
		} = Color.White;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002787 File Offset: 0x00000987
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0000278F File Offset: 0x0000098F
		public Color ColorPreviewBorderColor
		{
			[CompilerGenerated]
			get
			{
				return this.<ColorPreviewBorderColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ColorPreviewBorderColor>k__BackingField = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002798 File Offset: 0x00000998
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000027A0 File Offset: 0x000009A0
		public bool EditAlfa
		{
			[CompilerGenerated]
			get
			{
				return this.<EditAlfa>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EditAlfa>k__BackingField = value;
			}
		} = true;

		// Token: 0x06000029 RID: 41 RVA: 0x000027A9 File Offset: 0x000009A9
		public ColorDialogSettings()
		{
		}

		// Token: 0x0400001B RID: 27
		[CompilerGenerated]
		private Color <EditorsColor>k__BackingField;

		// Token: 0x0400001C RID: 28
		[CompilerGenerated]
		private Color <ColorPreviewBorderColor>k__BackingField;

		// Token: 0x0400001D RID: 29
		[CompilerGenerated]
		private bool <EditAlfa>k__BackingField;
	}
}
