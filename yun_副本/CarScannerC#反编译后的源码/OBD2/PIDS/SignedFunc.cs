using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E2 RID: 994
	internal class SignedFunc : FunctionExtension
	{
		// Token: 0x0600284F RID: 10319 RVA: 0x001ECB6A File Offset: 0x001EAD6A
		public double calculate()
		{
			return (double)((sbyte)((byte)this.x));
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x001ECB75 File Offset: 0x001EAD75
		public FunctionExtension clone()
		{
			return new SignedFunc();
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public int getParametersNumber()
		{
			return 1;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x001ECB7C File Offset: 0x001EAD7C
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.x = parameterValue;
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x001ECB88 File Offset: 0x001EAD88
		public string getParameterName(int parameterIndex)
		{
			return CustomPID.DICT_LETTERS[parameterIndex];
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x00002050 File Offset: 0x00000250
		public SignedFunc()
		{
		}

		// Token: 0x0400168D RID: 5773
		private double x;
	}
}
