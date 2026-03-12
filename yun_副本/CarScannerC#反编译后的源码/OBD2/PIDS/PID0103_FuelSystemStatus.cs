using System;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200040A RID: 1034
	public class PID0103_FuelSystemStatus : PID
	{
		// Token: 0x06002A74 RID: 10868 RVA: 0x001F24F1 File Offset: 0x001F06F1
		public PID0103_FuelSystemStatus()
			: base(PID.GetResourceString("PID_0103"), "0103")
		{
			this.Value = new PID0103_FuelSystemStatus.FuelSystemStatuses[]
			{
				PID0103_FuelSystemStatus.FuelSystemStatuses.None,
				PID0103_FuelSystemStatus.FuelSystemStatuses.None
			};
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x001F2530 File Offset: 0x001F0730
		public static string GetFullCaption(PID0103_FuelSystemStatus.FuelSystemStatuses fss)
		{
			if (fss < (PID0103_FuelSystemStatus.FuelSystemStatuses)PID0103_FuelSystemStatus.Fss_strings.Length)
			{
				return PID0103_FuelSystemStatus.Fss_strings[(int)fss];
			}
			return "Unknown";
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x001F2549 File Offset: 0x001F0749
		// (set) Token: 0x06002A77 RID: 10871 RVA: 0x001F2551 File Offset: 0x001F0751
		public PID0103_FuelSystemStatus.FuelSystemStatuses[] Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				this.OnValueChanged();
			}
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x001F2560 File Offset: 0x001F0760
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			PID0103_FuelSystemStatus.FuelSystemStatuses[] array = new PID0103_FuelSystemStatus.FuelSystemStatuses[2];
			byte b = data[0];
			switch (b)
			{
			case 1:
				array[0] = PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToInsufficientEngineTemperature;
				goto IL_0057;
			case 2:
				array[0] = PID0103_FuelSystemStatus.FuelSystemStatuses.ClosedLoopOK;
				goto IL_0057;
			case 3:
				break;
			case 4:
				array[0] = PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToEngineLoadOrFuelCut;
				goto IL_0057;
			default:
				if (b == 8)
				{
					array[0] = PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToSystemFailure;
					goto IL_0057;
				}
				if (b == 16)
				{
					array[0] = PID0103_FuelSystemStatus.FuelSystemStatuses.ClosedLoopButFail;
					goto IL_0057;
				}
				break;
			}
			array[0] = PID0103_FuelSystemStatus.FuelSystemStatuses.None;
			IL_0057:
			if (data.Length > 1)
			{
				b = data[1];
				switch (b)
				{
				case 1:
					array[1] = PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToInsufficientEngineTemperature;
					goto IL_00AC;
				case 2:
					array[1] = PID0103_FuelSystemStatus.FuelSystemStatuses.ClosedLoopOK;
					goto IL_00AC;
				case 3:
					break;
				case 4:
					array[1] = PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToEngineLoadOrFuelCut;
					goto IL_00AC;
				default:
					if (b == 8)
					{
						array[1] = PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToSystemFailure;
						goto IL_00AC;
					}
					if (b == 16)
					{
						array[1] = PID0103_FuelSystemStatus.FuelSystemStatuses.ClosedLoopButFail;
						goto IL_00AC;
					}
					break;
				}
				array[1] = PID0103_FuelSystemStatus.FuelSystemStatuses.None;
			}
			else
			{
				array[1] = PID0103_FuelSystemStatus.FuelSystemStatuses.None;
			}
			IL_00AC:
			this.Value = array;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x001F2620 File Offset: 0x001F0820
		// Note: this type is marked as 'beforefieldinit'.
		static PID0103_FuelSystemStatus()
		{
		}

		// Token: 0x040017EC RID: 6124
		private static string[] Fss_strings = new string[]
		{
			PID.GetResourceString("PID_FSS_OpenLoopLowTemp"),
			PID.GetResourceString("PID_FSS_ClosedOK"),
			PID.GetResourceString("PID_FSS_OpenLoadOrFuelCut"),
			PID.GetResourceString("PID_FSS_OpenLoopFail"),
			PID.GetResourceString("PID_FSS_ClosedButFail")
		};

		// Token: 0x040017ED RID: 6125
		private PID0103_FuelSystemStatus.FuelSystemStatuses[] _Value = new PID0103_FuelSystemStatus.FuelSystemStatuses[]
		{
			PID0103_FuelSystemStatus.FuelSystemStatuses.None,
			PID0103_FuelSystemStatus.FuelSystemStatuses.None
		};

		// Token: 0x0200040B RID: 1035
		public enum FuelSystemStatuses
		{
			// Token: 0x040017EF RID: 6127
			OpenLoopDueToInsufficientEngineTemperature,
			// Token: 0x040017F0 RID: 6128
			ClosedLoopOK,
			// Token: 0x040017F1 RID: 6129
			OpenLoopDueToEngineLoadOrFuelCut,
			// Token: 0x040017F2 RID: 6130
			OpenLoopDueToSystemFailure,
			// Token: 0x040017F3 RID: 6131
			ClosedLoopButFail,
			// Token: 0x040017F4 RID: 6132
			None
		}
	}
}
