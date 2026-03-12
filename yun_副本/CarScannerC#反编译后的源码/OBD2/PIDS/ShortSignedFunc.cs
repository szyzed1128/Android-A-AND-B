using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E3 RID: 995
	internal class ShortSignedFunc : FunctionExtension
	{
		// Token: 0x06002855 RID: 10325 RVA: 0x001ECB91 File Offset: 0x001EAD91
		public double calculate()
		{
			return (double)((short)((int)this.A * 256 + (int)this.B));
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x001ECBAA File Offset: 0x001EADAA
		public FunctionExtension clone()
		{
			return new ShortSignedFunc();
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x001ECBB1 File Offset: 0x001EADB1
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.A = parameterValue;
			}
			if (parameterIndex == 1)
			{
				this.B = parameterValue;
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00002050 File Offset: 0x00000250
		public ShortSignedFunc()
		{
		}

		// Token: 0x0400168E RID: 5774
		private double A;

		// Token: 0x0400168F RID: 5775
		private double B;
	}
}
