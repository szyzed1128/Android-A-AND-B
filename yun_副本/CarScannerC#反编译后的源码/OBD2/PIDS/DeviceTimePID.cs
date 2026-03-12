using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using Xamarin.Forms;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F5 RID: 1013
	internal class DeviceTimePID : PIDWithStringValue
	{
		// Token: 0x060028C2 RID: 10434 RVA: 0x001ED138 File Offset: 0x001EB338
		public DeviceTimePID()
			: base(Translate.GetString("ios_CurrentTime"), "", (byte[] data) => string.Empty, false)
		{
			try
			{
				Device.StartTimer(TimeSpan.FromSeconds(1.0), delegate
				{
					try
					{
						DateTime nowSafe = DateTimeNowHelper.NowSafe;
						base.TimeStamp = new TimeSpan(nowSafe.Ticks);
						base.Value = string.Concat(new string[]
						{
							nowSafe.Hour.ToString("00"),
							":",
							nowSafe.Minute.ToString("00"),
							":",
							nowSafe.Second.ToString("00")
						});
						this.OnValueChanged();
					}
					catch (Exception)
					{
					}
					return true;
				});
			}
			catch (Exception)
			{
			}
			this.Command = "";
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000027D4 File Offset: 0x000009D4
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x060028C5 RID: 10437 RVA: 0x000027D4 File Offset: 0x000009D4
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

		// Token: 0x060028C6 RID: 10438 RVA: 0x001ED1BC File Offset: 0x001EB3BC
		[CompilerGenerated]
		private bool <.ctor>b__0_1()
		{
			try
			{
				DateTime nowSafe = DateTimeNowHelper.NowSafe;
				base.TimeStamp = new TimeSpan(nowSafe.Ticks);
				base.Value = string.Concat(new string[]
				{
					nowSafe.Hour.ToString("00"),
					":",
					nowSafe.Minute.ToString("00"),
					":",
					nowSafe.Second.ToString("00")
				});
				this.OnValueChanged();
			}
			catch (Exception)
			{
			}
			return true;
		}

		// Token: 0x020003F6 RID: 1014
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060028C7 RID: 10439 RVA: 0x001ED264 File Offset: 0x001EB464
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060028C8 RID: 10440 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060028C9 RID: 10441 RVA: 0x001ED270 File Offset: 0x001EB470
			internal string <.ctor>b__0_0(byte[] data)
			{
				return string.Empty;
			}

			// Token: 0x040016BB RID: 5819
			public static readonly DeviceTimePID.<>c <>9 = new DeviceTimePID.<>c();

			// Token: 0x040016BC RID: 5820
			public static Func<byte[], string> <>9__0_0;
		}
	}
}
