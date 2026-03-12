using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E5 RID: 997
	internal class BitShrFunc : FunctionExtension
	{
		// Token: 0x06002861 RID: 10337 RVA: 0x001ECBF8 File Offset: 0x001EADF8
		public double calculate()
		{
			return (double)((int)this.b >> (int)this.bitNumber);
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x001ECC0D File Offset: 0x001EAE0D
		public FunctionExtension clone()
		{
			return new BitShrFunc();
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x001ECC14 File Offset: 0x001EAE14
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

		// Token: 0x06002865 RID: 10341 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00002050 File Offset: 0x00000250
		public BitShrFunc()
		{
		}

		// Token: 0x04001692 RID: 5778
		private double b;

		// Token: 0x04001693 RID: 5779
		private double bitNumber;
	}
}
