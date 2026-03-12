using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000888 RID: 2184
	internal class LongCodingPrimitive
	{
		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x06004A47 RID: 19015 RVA: 0x0037D496 File Offset: 0x0037B696
		// (set) Token: 0x06004A48 RID: 19016 RVA: 0x0037D49E File Offset: 0x0037B69E
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Title>k__BackingField = value;
			}
		}

		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x06004A49 RID: 19017 RVA: 0x0037D4A7 File Offset: 0x0037B6A7
		// (set) Token: 0x06004A4A RID: 19018 RVA: 0x0037D4AF File Offset: 0x0037B6AF
		public Func<byte[], bool> CheckDataDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<CheckDataDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CheckDataDelegate>k__BackingField = value;
			}
		}

		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x06004A4B RID: 19019 RVA: 0x0037D4B8 File Offset: 0x0037B6B8
		// (set) Token: 0x06004A4C RID: 19020 RVA: 0x0037D4C0 File Offset: 0x0037B6C0
		public Func<byte[], byte[]> ChangeDataDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<ChangeDataDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChangeDataDelegate>k__BackingField = value;
			}
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x0037D4C9 File Offset: 0x0037B6C9
		public LongCodingPrimitive(string Title, Func<byte[], bool> CheckDataDelegate, Func<byte[], byte[]> ChangeDataDelegate)
		{
			this.Title = Title;
			this.CheckDataDelegate = CheckDataDelegate;
			this.ChangeDataDelegate = ChangeDataDelegate;
		}

		// Token: 0x04002B2A RID: 11050
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x04002B2B RID: 11051
		[CompilerGenerated]
		private Func<byte[], bool> <CheckDataDelegate>k__BackingField;

		// Token: 0x04002B2C RID: 11052
		[CompilerGenerated]
		private Func<byte[], byte[]> <ChangeDataDelegate>k__BackingField;
	}
}
