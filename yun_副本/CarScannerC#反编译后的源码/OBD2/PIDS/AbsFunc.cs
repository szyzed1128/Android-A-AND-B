using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003E9 RID: 1001
	internal class AbsFunc : FunctionExtension
	{
		// Token: 0x06002879 RID: 10361 RVA: 0x001ECCD4 File Offset: 0x001EAED4
		public double calculate()
		{
			return Math.Abs(this.a);
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x001ECCE1 File Offset: 0x001EAEE1
		public FunctionExtension clone()
		{
			return new AbsFunc();
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public int getParametersNumber()
		{
			return 1;
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x001ECCE8 File Offset: 0x001EAEE8
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.a = parameterValue;
			}
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x00002050 File Offset: 0x00000250
		public AbsFunc()
		{
		}

		// Token: 0x0400169A RID: 5786
		private double a;
	}
}
