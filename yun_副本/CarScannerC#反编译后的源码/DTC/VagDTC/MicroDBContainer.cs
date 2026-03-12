using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.DTC.VagDTC
{
	// Token: 0x0200057E RID: 1406
	internal class MicroDBContainer
	{
		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x0600339B RID: 13211 RVA: 0x00242DFA File Offset: 0x00240FFA
		// (set) Token: 0x0600339C RID: 13212 RVA: 0x00242E02 File Offset: 0x00241002
		public bool IsBaseVariant
		{
			[CompilerGenerated]
			get
			{
				return this.<IsBaseVariant>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsBaseVariant>k__BackingField = value;
			}
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x0600339D RID: 13213 RVA: 0x00242E0B File Offset: 0x0024100B
		// (set) Token: 0x0600339E RID: 13214 RVA: 0x00242E13 File Offset: 0x00241013
		public List<string> Projects
		{
			[CompilerGenerated]
			get
			{
				return this.<Projects>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Projects>k__BackingField = value;
			}
		} = new List<string>();

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x0600339F RID: 13215 RVA: 0x00242E1C File Offset: 0x0024101C
		// (set) Token: 0x060033A0 RID: 13216 RVA: 0x00242E24 File Offset: 0x00241024
		public List<string> ASAMCollection
		{
			[CompilerGenerated]
			get
			{
				return this.<ASAMCollection>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ASAMCollection>k__BackingField = value;
			}
		} = new List<string>();

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x060033A1 RID: 13217 RVA: 0x00242E2D File Offset: 0x0024102D
		// (set) Token: 0x060033A2 RID: 13218 RVA: 0x00242E35 File Offset: 0x00241035
		public List<VagMicroItem> EventsCollection
		{
			[CompilerGenerated]
			get
			{
				return this.<EventsCollection>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EventsCollection>k__BackingField = value;
			}
		} = new List<VagMicroItem>();

		// Token: 0x060033A3 RID: 13219 RVA: 0x00242E3E File Offset: 0x0024103E
		public MicroDBContainer()
		{
		}

		// Token: 0x04001E5C RID: 7772
		[CompilerGenerated]
		private bool <IsBaseVariant>k__BackingField;

		// Token: 0x04001E5D RID: 7773
		[CompilerGenerated]
		private List<string> <Projects>k__BackingField;

		// Token: 0x04001E5E RID: 7774
		[CompilerGenerated]
		private List<string> <ASAMCollection>k__BackingField;

		// Token: 0x04001E5F RID: 7775
		[CompilerGenerated]
		private List<VagMicroItem> <EventsCollection>k__BackingField;
	}
}
