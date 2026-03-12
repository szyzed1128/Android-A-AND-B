using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000418 RID: 1048
	internal class PID_ATRV : PIDWithFloatValueFormula
	{
		// Token: 0x06002D0A RID: 11530 RVA: 0x001FDFB0 File Offset: 0x001FC1B0
		public PID_ATRV()
			: base("ATRV", delegate(byte[] data)
			{
				double num = (double)data[1];
				num /= 10.0;
				double num2 = (double)data[2];
				num2 /= 100.0;
				return (double)data[0] + num + num2;
			}, UnitsHelper.Units.volts)
		{
			base.Minimum = 9.0;
			base.Maximum = 15.0;
			base.Id = 0;
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x06002D0C RID: 11532 RVA: 0x000027D4 File Offset: 0x000009D4
		public override bool IsAvailable
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x02000419 RID: 1049
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002D0D RID: 11533 RVA: 0x001FE00E File Offset: 0x001FC20E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002D0E RID: 11534 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002D0F RID: 11535 RVA: 0x001FE01C File Offset: 0x001FC21C
			internal double <.ctor>b__0_0(byte[] data)
			{
				double num = (double)data[1];
				num /= 10.0;
				double num2 = (double)data[2];
				num2 /= 100.0;
				return (double)data[0] + num + num2;
			}

			// Token: 0x04001921 RID: 6433
			public static readonly PID_ATRV.<>c <>9 = new PID_ATRV.<>c();

			// Token: 0x04001922 RID: 6434
			public static Func<byte[], double> <>9__0_0;
		}
	}
}
