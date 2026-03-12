using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E6 RID: 998
	internal class BitShlFunc : FunctionExtension
	{
		// Token: 0x06002867 RID: 10343 RVA: 0x001ECC2B File Offset: 0x001EAE2B
		public double calculate()
		{
			return (double)((int)this.b << (int)this.bitNumber);
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x001ECC40 File Offset: 0x001EAE40
		public FunctionExtension clone()
		{
			return new BitShlFunc();
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x001ECC47 File Offset: 0x001EAE47
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

		// Token: 0x0600286B RID: 10347 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x00002050 File Offset: 0x00000250
		public BitShlFunc()
		{
		}

		// Token: 0x04001694 RID: 5780
		private double b;

		// Token: 0x04001695 RID: 5781
		private double bitNumber;
	}
}
