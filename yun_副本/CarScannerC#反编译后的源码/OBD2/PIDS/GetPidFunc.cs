using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F0 RID: 1008
	internal class GetPidFunc : FunctionExtension
	{
		// Token: 0x060028A3 RID: 10403 RVA: 0x001ECFE4 File Offset: 0x001EB1E4
		public double calculate()
		{
			IPID ipid = App.OBDReader.CurrentCarData.FindPIDById((int)this.a);
			if (ipid != null)
			{
				IPIDFloatValue ipidfloatValue = ipid as IPIDFloatValue;
				if (ipidfloatValue != null)
				{
					return ipidfloatValue.Value;
				}
			}
			return double.NaN;
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x001ECCE1 File Offset: 0x001EAEE1
		public FunctionExtension clone()
		{
			return new AbsFunc();
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		public int getParametersNumber()
		{
			return 1;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x001ED025 File Offset: 0x001EB225
		public void setParameterValue(int parameterIndex, double parameterValue)
		{
			if (parameterIndex == 0)
			{
				this.a = parameterValue;
			}
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x00002050 File Offset: 0x00000250
		public GetPidFunc()
		{
		}

		// Token: 0x040016B3 RID: 5811
		private double a;
	}
}
