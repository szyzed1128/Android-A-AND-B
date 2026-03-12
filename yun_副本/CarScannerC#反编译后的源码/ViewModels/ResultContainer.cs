using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000737 RID: 1847
	internal class ResultContainer
	{
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x0032DCBA File Offset: 0x0032BEBA
		// (set) Token: 0x06003EC4 RID: 16068 RVA: 0x0032DCC2 File Offset: 0x0032BEC2
		public string RequestHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x0032DCCB File Offset: 0x0032BECB
		// (set) Token: 0x06003EC6 RID: 16070 RVA: 0x0032DCD3 File Offset: 0x0032BED3
		public string ResponseHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x0032DCDC File Offset: 0x0032BEDC
		// (set) Token: 0x06003EC8 RID: 16072 RVA: 0x0032DCE4 File Offset: 0x0032BEE4
		public string Command
		{
			[CompilerGenerated]
			get
			{
				return this.<Command>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Command>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x0032DCED File Offset: 0x0032BEED
		// (set) Token: 0x06003ECA RID: 16074 RVA: 0x0032DCF5 File Offset: 0x0032BEF5
		public string ResponseRaw
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseRaw>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseRaw>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x0032DCFE File Offset: 0x0032BEFE
		// (set) Token: 0x06003ECC RID: 16076 RVA: 0x0032DD06 File Offset: 0x0032BF06
		public string ResponseDecoded
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseDecoded>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseDecoded>k__BackingField = value;
			}
		} = "";

		// Token: 0x06003ECD RID: 16077 RVA: 0x0032DD0F File Offset: 0x0032BF0F
		public ResultContainer()
		{
		}

		// Token: 0x04002685 RID: 9861
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x04002686 RID: 9862
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x04002687 RID: 9863
		[CompilerGenerated]
		private string <Command>k__BackingField;

		// Token: 0x04002688 RID: 9864
		[CompilerGenerated]
		private string <ResponseRaw>k__BackingField;

		// Token: 0x04002689 RID: 9865
		[CompilerGenerated]
		private string <ResponseDecoded>k__BackingField;
	}
}
