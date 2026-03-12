using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000408 RID: 1032
	internal class PID0102_FreezeFrame : PIDWithStringValue
	{
		// Token: 0x06002A6F RID: 10863 RVA: 0x001F2456 File Offset: 0x001F0656
		public PID0102_FreezeFrame()
			: base(PID.GetResourceString("PID_FreezeFrameDTC"), "0102", (byte[] data) => string.Empty, false)
		{
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x001F2490 File Offset: 0x001F0690
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = base.TimeStamp;
			foreach (byte b in data)
			{
				base.Value += b.ToString("X2", CultureInfo.InvariantCulture);
			}
			this.OnValueChanged();
		}

		// Token: 0x02000409 RID: 1033
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002A71 RID: 10865 RVA: 0x001F24E5 File Offset: 0x001F06E5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002A72 RID: 10866 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002A73 RID: 10867 RVA: 0x001ED270 File Offset: 0x001EB470
			internal string <.ctor>b__0_0(byte[] data)
			{
				return string.Empty;
			}

			// Token: 0x040017EA RID: 6122
			public static readonly PID0102_FreezeFrame.<>c <>9 = new PID0102_FreezeFrame.<>c();

			// Token: 0x040017EB RID: 6123
			public static Func<byte[], string> <>9__0_0;
		}
	}
}
