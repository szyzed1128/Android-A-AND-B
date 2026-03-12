using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003EA RID: 1002
	internal class Float32Func : FunctionExtension
	{
		// Token: 0x0600287F RID: 10367 RVA: 0x001ECCF4 File Offset: 0x001EAEF4
		public double calculate()
		{
			byte[] array = new byte[]
			{
				(byte)this.A,
				(byte)this.B,
				(byte)this.C,
				(byte)this.D
			};
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(array);
			}
			return (double)BitConverter.ToSingle(array, 0);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x001ECD45 File Offset: 0x001EAF45
		public FunctionExtension clone()
		{
			return new Float32Func();
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x001ECD4C File Offset: 0x001EAF4C
		public int getParametersNumber()
		{
			return 4;
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x001ECD4F File Offset: 0x001EAF4F
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

		// Token: 0x06002883 RID: 10371 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x00002050 File Offset: 0x00000250
		public Float32Func()
		{
		}

		// Token: 0x0400169B RID: 5787
		private double A;

		// Token: 0x0400169C RID: 5788
		private double B;

		// Token: 0x0400169D RID: 5789
		private double C;

		// Token: 0x0400169E RID: 5790
		private double D;
	}
}
