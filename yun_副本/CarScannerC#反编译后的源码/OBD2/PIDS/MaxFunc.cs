using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E7 RID: 999
	internal class MaxFunc : FunctionExtension
	{
		// Token: 0x0600286D RID: 10349 RVA: 0x001ECC5E File Offset: 0x001EAE5E
		public double calculate()
		{
			if (this.a <= this.b)
			{
				return this.b;
			}
			return this.a;
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x001ECC7B File Offset: 0x001EAE7B
		public FunctionExtension clone()
		{
			return new MaxFunc();
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x001ECC82 File Offset: 0x001EAE82
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.a = parameterValue;
			}
			if (parameterIndex == 1)
			{
				this.b = parameterValue;
			}
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x00002050 File Offset: 0x00000250
		public MaxFunc()
		{
		}

		// Token: 0x04001696 RID: 5782
		private double a;

		// Token: 0x04001697 RID: 5783
		private double b;
	}
}
