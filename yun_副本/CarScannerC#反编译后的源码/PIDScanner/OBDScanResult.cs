using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.PIDScanner
{
	// Token: 0x020002E1 RID: 737
	public class OBDScanResult
	{
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x0600232F RID: 9007 RVA: 0x001AE174 File Offset: 0x001AC374
		// (set) Token: 0x06002330 RID: 9008 RVA: 0x001AE17C File Offset: 0x001AC37C
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
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x06002331 RID: 9009 RVA: 0x001AE185 File Offset: 0x001AC385
		// (set) Token: 0x06002332 RID: 9010 RVA: 0x001AE18D File Offset: 0x001AC38D
		public string RequestPID
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestPID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestPID>k__BackingField = value;
			}
		}

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x001AE196 File Offset: 0x001AC396
		// (set) Token: 0x06002334 RID: 9012 RVA: 0x001AE19E File Offset: 0x001AC39E
		public string Response
		{
			[CompilerGenerated]
			get
			{
				return this.<Response>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Response>k__BackingField = value;
			}
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x00002050 File Offset: 0x00000250
		public OBDScanResult()
		{
		}

		// Token: 0x040010F2 RID: 4338
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x040010F3 RID: 4339
		[CompilerGenerated]
		private string <RequestPID>k__BackingField;

		// Token: 0x040010F4 RID: 4340
		[CompilerGenerated]
		private string <Response>k__BackingField;
	}
}
