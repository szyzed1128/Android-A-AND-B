using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F2 RID: 1010
	internal class SetVarOnceFunc : FunctionExtension
	{
		// Token: 0x060028B0 RID: 10416 RVA: 0x001ED07A File Offset: 0x001EB27A
		public double calculate()
		{
			if (!SetVarFunc.dict.ContainsKey(this.key))
			{
				SetVarFunc.dict.TryAdd(this.key, this.value);
			}
			return this.value;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x001ED0AB File Offset: 0x001EB2AB
		public FunctionExtension clone()
		{
			return new SetVarOnceFunc();
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x001ED0B2 File Offset: 0x001EB2B2
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

		// Token: 0x060028B5 RID: 10421 RVA: 0x00002050 File Offset: 0x00000250
		public SetVarOnceFunc()
		{
		}

		// Token: 0x040016B7 RID: 5815
		private int key;

		// Token: 0x040016B8 RID: 5816
		private double value;
	}
}
