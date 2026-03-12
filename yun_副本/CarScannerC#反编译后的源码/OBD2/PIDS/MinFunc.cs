using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E8 RID: 1000
	internal class MinFunc : FunctionExtension
	{
		// Token: 0x06002873 RID: 10355 RVA: 0x001ECC99 File Offset: 0x001EAE99
		public double calculate()
		{
			if (this.a >= this.b)
			{
				return this.b;
			}
			return this.a;
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x001ECCB6 File Offset: 0x001EAEB6
		public FunctionExtension clone()
		{
			return new MinFunc();
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x001ECCBD File Offset: 0x001EAEBD
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

		// Token: 0x06002877 RID: 10359 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x00002050 File Offset: 0x00000250
		public MinFunc()
		{
		}

		// Token: 0x04001698 RID: 5784
		private double a;

		// Token: 0x04001699 RID: 5785
		private double b;
	}
}
