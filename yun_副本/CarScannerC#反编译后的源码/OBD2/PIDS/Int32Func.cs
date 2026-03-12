using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003EE RID: 1006
	internal class Int32Func : FunctionExtension
	{
		// Token: 0x06002897 RID: 10391 RVA: 0x001ECF14 File Offset: 0x001EB114
		public double calculate()
		{
			return this.A * 16777216.0 + this.B * 65536.0 + this.C * 256.0 + this.D;
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x001ECF4F File Offset: 0x001EB14F
		public FunctionExtension clone()
		{
			return new Int32Func();
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x001ECD4C File Offset: 0x001EAF4C
		public int getParametersNumber()
		{
			return 4;
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x001ECF56 File Offset: 0x001EB156
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
			case 3:
				this.D = parameterValue;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x00002050 File Offset: 0x00000250
		public Int32Func()
		{
		}

		// Token: 0x040016AC RID: 5804
		private double A;

		// Token: 0x040016AD RID: 5805
		private double B;

		// Token: 0x040016AE RID: 5806
		private double C;

		// Token: 0x040016AF RID: 5807
		private double D;
	}
}
