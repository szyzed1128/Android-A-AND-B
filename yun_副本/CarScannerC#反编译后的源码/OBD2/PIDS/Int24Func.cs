using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003ED RID: 1005
	internal class Int24Func : FunctionExtension
	{
		// Token: 0x06002891 RID: 10385 RVA: 0x001ECEB4 File Offset: 0x001EB0B4
		public double calculate()
		{
			return this.A * 65536.0 + this.B * 256.0 + this.C;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x001ECEDE File Offset: 0x001EB0DE
		public FunctionExtension clone()
		{
			return new Int24Func();
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		public int getParametersNumber()
		{
			return 3;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x001ECEE8 File Offset: 0x001EB0E8
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			switch (parameterIndex)
			{
			case 0:
				this.A = parameterValue;
				return;
			case 1:
				this.B = parameterValue;
				return;
			case 2:
				this.C = parameterValue;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x00002050 File Offset: 0x00000250
		public Int24Func()
		{
		}

		// Token: 0x040016A9 RID: 5801
		private double A;

		// Token: 0x040016AA RID: 5802
		private double B;

		// Token: 0x040016AB RID: 5803
		private double C;
	}
}
