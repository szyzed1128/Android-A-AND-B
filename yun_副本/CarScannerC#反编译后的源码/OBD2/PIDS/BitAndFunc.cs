using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E4 RID: 996
	internal class BitAndFunc : FunctionExtension
	{
		// Token: 0x0600285B RID: 10331 RVA: 0x001ECBC8 File Offset: 0x001EADC8
		public double calculate()
		{
			return (double)((int)this.b & (int)this.bitNumber);
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x001ECBDA File Offset: 0x001EADDA
		public FunctionExtension clone()
		{
			return new BitAndFunc();
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x001ECBE1 File Offset: 0x001EADE1
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.b = parameterValue;
			}
			if (parameterIndex == 1)
			{
				this.bitNumber = parameterValue;
			}
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x00002050 File Offset: 0x00000250
		public BitAndFunc()
		{
		}

		// Token: 0x04001690 RID: 5776
		private double b;

		// Token: 0x04001691 RID: 5777
		private double bitNumber;
	}
}
