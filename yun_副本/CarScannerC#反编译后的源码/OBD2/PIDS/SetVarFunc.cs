using System;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F1 RID: 1009
	internal class SetVarFunc : FunctionExtension
	{
		// Token: 0x060028A9 RID: 10409 RVA: 0x001ED031 File Offset: 0x001EB231
		public double calculate()
		{
			SetVarFunc.dict[this.key] = this.value;
			return this.value;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x001ED04F File Offset: 0x001EB24F
		public FunctionExtension clone()
		{
			return new SetVarFunc();
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x001ECB08 File Offset: 0x001EAD08
		public int getParametersNumber()
		{
			return 2;
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x001ED056 File Offset: 0x001EB256
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

		// Token: 0x060028AE RID: 10414 RVA: 0x00002050 File Offset: 0x00000250
		public SetVarFunc()
		{
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x001ED06E File Offset: 0x001EB26E
		// Note: this type is marked as 'beforefieldinit'.
		static SetVarFunc()
		{
		}

		// Token: 0x040016B4 RID: 5812
		internal static Dictionary<int, double> dict = new Dictionary<int, double>();

		// Token: 0x040016B5 RID: 5813
		private int key;

		// Token: 0x040016B6 RID: 5814
		private double value;
	}
}
