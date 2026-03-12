using System;
using org.mariuszgromada.math.mxparser;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003EB RID: 1003
	internal class Float64Func : FunctionExtension
	{
		// Token: 0x06002885 RID: 10373 RVA: 0x001ECD88 File Offset: 0x001EAF88
		public double calculate()
		{
			byte[] array = new byte[]
			{
				(byte)this.A,
				(byte)this.B,
				(byte)this.C,
				(byte)this.D,
				(byte)this.E,
				(byte)this.F,
				(byte)this.G,
				(byte)this.H
			};
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(array);
			}
			return BitConverter.ToDouble(array, 0);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x001ECE00 File Offset: 0x001EB000
		public FunctionExtension clone()
		{
			return new Float64Func();
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x001ECD4C File Offset: 0x001EAF4C
		public int getParametersNumber()
		{
			return 4;
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x001ECE08 File Offset: 0x001EB008
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
			case 4:
				this.E = parameterValue;
				return;
			case 5:
				this.F = parameterValue;
				return;
			case 6:
				this.G = parameterValue;
				return;
			case 7:
				this.H = parameterValue;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string getParameterName(int parameterIndex)
		{
			return "";
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x00002050 File Offset: 0x00000250
		public Float64Func()
		{
		}

		// Token: 0x0400169F RID: 5791
		private double A;

		// Token: 0x040016A0 RID: 5792
		private double B;

		// Token: 0x040016A1 RID: 5793
		private double C;

		// Token: 0x040016A2 RID: 5794
		private double D;

		// Token: 0x040016A3 RID: 5795
		private double E;

		// Token: 0x040016A4 RID: 5796
		private double F;

		// Token: 0x040016A5 RID: 5797
		private double G;

		// Token: 0x040016A6 RID: 5798
		private double H;
	}
}
