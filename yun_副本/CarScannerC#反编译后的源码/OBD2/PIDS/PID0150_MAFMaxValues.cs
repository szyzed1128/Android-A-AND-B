using System;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200041C RID: 1052
	public class PID0150_MAFMaxValues : PID_MaxValues
	{
		// Token: 0x06002D13 RID: 11539 RVA: 0x001FE0F6 File Offset: 0x001FC2F6
		public PID0150_MAFMaxValues()
			: base(PID.GetResourceString("PID_0150"), "0150")
		{
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x001FE10D File Offset: 0x001FC30D
		public override void Decode(byte[] data, TimeSpan timeSpan, string response_header)
		{
			base.TimeStamp = timeSpan;
			if (data[0] > 0)
			{
				PIDWithFloatValueFormula.MAFScalingPerBit = (double)(data[0] * 10);
			}
		}
	}
}
