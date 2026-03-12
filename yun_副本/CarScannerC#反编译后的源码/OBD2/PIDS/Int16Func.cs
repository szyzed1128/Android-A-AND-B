using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003EC RID: 1004
	internal class Int16Func : FunctionExtension
	{
		// Token: 0x0600288B RID: 10379 RVA: 0x001ECE7B File Offset: 0x001EB07B
		public double calculate()
		{
			return this.A * 256.0 + this.B;
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x001ECE94 File Offset: 0x001EB094
		public FunctionExtension clone()
		{
			return new Int16Func();
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x001ECE9B File Offset: 0x001EB09B
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.A = parameterValue;
				return;
			}
			if (parameterIndex != 1)
			{
				return;
			}
			this.B = parameterValue;
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x00002050 File Offset: 0x00000250
		public Int16Func()
		{
		}

		// Token: 0x040016A7 RID: 5799
		private double A;

		// Token: 0x040016A8 RID: 5800
		private double B;
	}
}
