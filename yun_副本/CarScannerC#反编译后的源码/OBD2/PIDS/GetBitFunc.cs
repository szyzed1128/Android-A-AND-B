using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E0 RID: 992
	internal class GetBitFunc : FunctionExtension
	{
		// Token: 0x06002841 RID: 10305 RVA: 0x001ECAC3 File Offset: 0x001EACC3
		public static bool GetBit0_7(byte b, int bitNumber)
		{
			return ((int)b & (1 << bitNumber)) != 0;
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x001ECAD0 File Offset: 0x001EACD0
		public double calculate()
		{
			if (GetBitFunc.GetBit0_7((byte)this.x, (int)this.y))
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x001ECAFA File Offset: 0x001EACFA
		public FunctionExtension clone()
		{
			return new GetBitFunc();
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x001ECB0B File Offset: 0x001EAD0B
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

		// Token: 0x06002847 RID: 10311 RVA: 0x00002050 File Offset: 0x00000250
		public GetBitFunc()
		{
		}

		// Token: 0x04001689 RID: 5769
		private double x;

		// Token: 0x0400168A RID: 5770
		private double y;
	}
}
