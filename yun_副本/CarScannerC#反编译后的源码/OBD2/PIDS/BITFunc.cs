using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E1 RID: 993
	internal class BITFunc : FunctionExtension
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x001ECAC3 File Offset: 0x001EACC3
		public static bool GetBit(byte b, int bitNumber)
		{
			return ((int)b & (1 << bitNumber)) != 0;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x001ECB22 File Offset: 0x001EAD22
		public double calculate()
		{
			if (BITFunc.GetBit((byte)this.x, (int)this.y))
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x001ECB4C File Offset: 0x001EAD4C
		public FunctionExtension clone()
		{
			return new BITFunc();
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x001ECB53 File Offset: 0x001EAD53
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.x = parameterValue;
			}
			if (parameterIndex == 1)
			{
				this.y = parameterValue;
			}
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x00002050 File Offset: 0x00000250
		public BITFunc()
		{
		}

		// Token: 0x0400168B RID: 5771
		private double x;

		// Token: 0x0400168C RID: 5772
		private double y;
	}
}
