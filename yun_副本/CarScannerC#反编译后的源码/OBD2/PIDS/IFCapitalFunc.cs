using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003EF RID: 1007
	internal class IFCapitalFunc : FunctionExtension
	{
		// Token: 0x0600289D RID: 10397 RVA: 0x001ECF8E File Offset: 0x001EB18E
		public double calculate()
		{
			if (this.A == 1.0)
			{
				return this.B;
			}
			return this.C;
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x001ECFAE File Offset: 0x001EB1AE
		public FunctionExtension clone()
		{
			return new IFCapitalFunc();
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		public int getParametersNumber()
		{
			return 3;
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x001ECFB5 File Offset: 0x001EB1B5
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

		// Token: 0x060028A1 RID: 10401 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x00002050 File Offset: 0x00000250
		public IFCapitalFunc()
		{
		}

		// Token: 0x040016B0 RID: 5808
		private double A;

		// Token: 0x040016B1 RID: 5809
		private double B;

		// Token: 0x040016B2 RID: 5810
		private double C;
	}
}
