using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200041D RID: 1053
	internal class PID_OxygenSensorsPresent : PID
	{
		// Token: 0x06002D15 RID: 11541 RVA: 0x001FE128 File Offset: 0x001FC328
		public PID_OxygenSensorsPresent(string Command)
			: base("Oxygen sensors present", Command)
		{
			this.Value = new bool[8];
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x001FE142 File Offset: 0x001FC342
		// (set) Token: 0x06002D17 RID: 11543 RVA: 0x001FE14A File Offset: 0x001FC34A
		public bool[] Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x001FE154 File Offset: 0x001FC354
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			BitArray bitArray = new BitArray(data);
			for (int i = 0; i < this.Value.Length; i++)
			{
				this.Value[i] = bitArray.Get(i);
			}
			this.OnValueChanged();
		}

		// Token: 0x04001923 RID: 6435
		[CompilerGenerated]
		private bool[] <Value>k__BackingField;
	}
}
