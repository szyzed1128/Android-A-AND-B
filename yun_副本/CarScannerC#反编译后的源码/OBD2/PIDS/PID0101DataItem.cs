using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200041F RID: 1055
	public class PID0101DataItem
	{
		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x001FE6BC File Offset: 0x001FC8BC
		// (set) Token: 0x06002D1F RID: 11551 RVA: 0x001FE6C4 File Offset: 0x001FC8C4
		public bool MIL_ON
		{
			[CompilerGenerated]
			get
			{
				return this.<MIL_ON>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MIL_ON>k__BackingField = value;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06002D20 RID: 11552 RVA: 0x001FE6CD File Offset: 0x001FC8CD
		// (set) Token: 0x06002D21 RID: 11553 RVA: 0x001FE6D5 File Offset: 0x001FC8D5
		public int DTCs
		{
			[CompilerGenerated]
			get
			{
				return this.<DTCs>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DTCs>k__BackingField = value;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06002D22 RID: 11554 RVA: 0x001FE6DE File Offset: 0x001FC8DE
		// (set) Token: 0x06002D23 RID: 11555 RVA: 0x001FE6E6 File Offset: 0x001FC8E6
		public PID0101DataItem.VehicleTypes VehicleType
		{
			[CompilerGenerated]
			get
			{
				return this.<VehicleType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<VehicleType>k__BackingField = value;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06002D24 RID: 11556 RVA: 0x001FE6EF File Offset: 0x001FC8EF
		// (set) Token: 0x06002D25 RID: 11557 RVA: 0x001FE6F7 File Offset: 0x001FC8F7
		public ECUTest[] ECUTests
		{
			[CompilerGenerated]
			get
			{
				return this.<ECUTests>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ECUTests>k__BackingField = value;
			}
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x001FE700 File Offset: 0x001FC900
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(PID.GetResourceString("PID_Status_MIL") + (this.MIL_ON ? PID.GetResourceString("PID_Status_MIL_ON") : PID.GetResourceString("PID_Status_MIL_OFF")));
			stringBuilder.AppendLine(PID.GetResourceString("PID_Status_DTCnumber") + this.DTCs.ToString());
			if (this.ECUTests != null)
			{
				foreach (ECUTest ecutest in this.ECUTests)
				{
					stringBuilder.AppendLine(ecutest.ToString());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x00002050 File Offset: 0x00000250
		public PID0101DataItem()
		{
		}

		// Token: 0x04001925 RID: 6437
		[CompilerGenerated]
		private bool <MIL_ON>k__BackingField;

		// Token: 0x04001926 RID: 6438
		[CompilerGenerated]
		private int <DTCs>k__BackingField;

		// Token: 0x04001927 RID: 6439
		[CompilerGenerated]
		private PID0101DataItem.VehicleTypes <VehicleType>k__BackingField;

		// Token: 0x04001928 RID: 6440
		[CompilerGenerated]
		private ECUTest[] <ECUTests>k__BackingField;

		// Token: 0x02000420 RID: 1056
		public enum VehicleTypes
		{
			// Token: 0x0400192A RID: 6442
			SparkIgnition,
			// Token: 0x0400192B RID: 6443
			CompressionIgnition
		}
	}
}
