using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000008 RID: 8
	public class DialogSettings
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000026BA File Offset: 0x000008BA
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000026C2 File Offset: 0x000008C2
		public Color BackgroundColor
		{
			[CompilerGenerated]
			get
			{
				return this.<BackgroundColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BackgroundColor>k__BackingField = value;
			}
		} = Color.FromHex("#40000000");

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000026CB File Offset: 0x000008CB
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000026D3 File Offset: 0x000008D3
		public Color DialogColor
		{
			[CompilerGenerated]
			get
			{
				return this.<DialogColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DialogColor>k__BackingField = value;
			}
		} = Color.White;

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000026DC File Offset: 0x000008DC
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000026E4 File Offset: 0x000008E4
		public Color TextColor
		{
			[CompilerGenerated]
			get
			{
				return this.<TextColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TextColor>k__BackingField = value;
			}
		} = Color.Black;

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000026ED File Offset: 0x000008ED
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000026F5 File Offset: 0x000008F5
		public string OkButtonText
		{
			[CompilerGenerated]
			get
			{
				return this.<OkButtonText>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OkButtonText>k__BackingField = value;
			}
		} = "OK";

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000026FE File Offset: 0x000008FE
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002706 File Offset: 0x00000906
		public string CancelButtonText
		{
			[CompilerGenerated]
			get
			{
				return this.<CancelButtonText>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CancelButtonText>k__BackingField = value;
			}
		} = "Cancel";

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000270F File Offset: 0x0000090F
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002717 File Offset: 0x00000917
		public bool DialogAnimation
		{
			[CompilerGenerated]
			get
			{
				return this.<DialogAnimation>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DialogAnimation>k__BackingField = value;
			}
		} = true;

		// Token: 0x06000022 RID: 34 RVA: 0x00002720 File Offset: 0x00000920
		public DialogSettings()
		{
		}

		// Token: 0x04000015 RID: 21
		[CompilerGenerated]
		private Color <BackgroundColor>k__BackingField;

		// Token: 0x04000016 RID: 22
		[CompilerGenerated]
		private Color <DialogColor>k__BackingField;

		// Token: 0x04000017 RID: 23
		[CompilerGenerated]
		private Color <TextColor>k__BackingField;

		// Token: 0x04000018 RID: 24
		[CompilerGenerated]
		private string <OkButtonText>k__BackingField;

		// Token: 0x04000019 RID: 25
		[CompilerGenerated]
		private string <CancelButtonText>k__BackingField;

		// Token: 0x0400001A RID: 26
		[CompilerGenerated]
		private bool <DialogAnimation>k__BackingField;
	}
}
