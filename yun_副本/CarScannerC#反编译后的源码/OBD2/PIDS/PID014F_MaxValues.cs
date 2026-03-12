using System;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200041B RID: 1051
	public class PID014F_MaxValues : PID_MaxValues
	{
		// Token: 0x06002D11 RID: 11537 RVA: 0x001FE05D File Offset: 0x001FC25D
		public PID014F_MaxValues()
			: base(PID.GetResourceString("PID_014F"), "014F")
		{
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x001FE074 File Offset: 0x001FC274
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			if (data[0] > 0)
			{
				PIDWithFloatValueFormula.EquivalenceRatioScalingPerBit = (double)data[0] / 65535.0;
			}
			if (data[1] > 0)
			{
				PIDWithFloatValueFormula.OxygenSensorVoltageScalingPerBit = (double)data[1] / 65535.0;
			}
			if (data[2] > 0)
			{
				PIDWithFloatValueFormula.OxygenSensorCurrentScalingPerBit = (double)data[2] / 32768.0;
			}
			if (data[3] > 0)
			{
				PIDWithFloatValueFormula.IntakeManifoldAbsolutPressureScalingPerBit = (double)data[3] * 10.0 / 255.0;
			}
		}
	}
}
