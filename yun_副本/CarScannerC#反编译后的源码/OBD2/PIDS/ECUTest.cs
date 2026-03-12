using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F9 RID: 1017
	public class ECUTest
	{
		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x001ED3DC File Offset: 0x001EB5DC
		// (set) Token: 0x060028D3 RID: 10451 RVA: 0x001ED3E4 File Offset: 0x001EB5E4
		public bool Available
		{
			[CompilerGenerated]
			get
			{
				return this.<Available>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Available>k__BackingField = value;
			}
		}

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x001ED3ED File Offset: 0x001EB5ED
		// (set) Token: 0x060028D5 RID: 10453 RVA: 0x001ED3F5 File Offset: 0x001EB5F5
		public bool Complete
		{
			[CompilerGenerated]
			get
			{
				return this.<Complete>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Complete>k__BackingField = value;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x001ED3FE File Offset: 0x001EB5FE
		// (set) Token: 0x060028D7 RID: 10455 RVA: 0x001ED406 File Offset: 0x001EB606
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x001ED40F File Offset: 0x001EB60F
		// (set) Token: 0x060028D9 RID: 10457 RVA: 0x001ED417 File Offset: 0x001EB617
		public TestPidCycle Cycle
		{
			[CompilerGenerated]
			get
			{
				return this.<Cycle>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Cycle>k__BackingField = value;
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x001ED420 File Offset: 0x001EB620
		public ECUTest(string Name)
		{
			this.Name = Name;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x001ED430 File Offset: 0x001EB630
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(5);
			stringBuilder.Append(this.Name);
			stringBuilder.Append(": ");
			if (this.Available)
			{
				stringBuilder.Append(Translate.GetString("test_Available"));
			}
			else
			{
				stringBuilder.Append(Translate.GetString("test_NotAvailable"));
			}
			stringBuilder.Append("/");
			if (this.Complete)
			{
				stringBuilder.Append(Translate.GetString("test_Completed"));
			}
			else
			{
				stringBuilder.Append(Translate.GetString("test_NotCompleted"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040016C2 RID: 5826
		[CompilerGenerated]
		private bool <Available>k__BackingField;

		// Token: 0x040016C3 RID: 5827
		[CompilerGenerated]
		private bool <Complete>k__BackingField;

		// Token: 0x040016C4 RID: 5828
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x040016C5 RID: 5829
		[CompilerGenerated]
		private TestPidCycle <Cycle>k__BackingField;
	}
}
