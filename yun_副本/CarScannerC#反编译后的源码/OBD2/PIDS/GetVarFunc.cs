using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F3 RID: 1011
	internal class GetVarFunc : FunctionExtension
	{
		// Token: 0x060028B6 RID: 10422 RVA: 0x001ED0CC File Offset: 0x001EB2CC
		public double calculate()
		{
			double num;
			if (SetVarFunc.dict.TryGetValue(this.key, out num))
			{
				return num;
			}
			return this.value;
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x001ED0F5 File Offset: 0x001EB2F5
		public FunctionExtension clone()
		{
			return new GetVarFunc();
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x001ED0FC File Offset: 0x001EB2FC
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.key = (int)parameterValue;
			}
			if (parameterIndex == 1)
			{
				this.value = parameterValue;
			}
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x00002050 File Offset: 0x00000250
		public GetVarFunc()
		{
		}

		// Token: 0x040016B9 RID: 5817
		private int key;

		// Token: 0x040016BA RID: 5818
		private double value;
	}
}
