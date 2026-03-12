using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F4 RID: 1012
	internal class GetTimestampMsFunc : FunctionExtension
	{
		// Token: 0x060028BC RID: 10428 RVA: 0x001ED114 File Offset: 0x001EB314
		public double calculate()
		{
			return (double)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x001ED12F File Offset: 0x001EB32F
		public FunctionExtension clone()
		{
			return new GetTimestampMsFunc();
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public int getParametersNumber()
		{
			return 1;
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000027D4 File Offset: 0x000009D4
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x00002050 File Offset: 0x00000250
		public GetTimestampMsFunc()
		{
		}
	}
}
