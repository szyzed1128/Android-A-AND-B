using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003F7 RID: 1015
	public class DTCPIDItem
	{
		// Token: 0x060028CA RID: 10442 RVA: 0x001ED277 File Offset: 0x001EB477
		public DTCPIDItem(string code)
		{
			this.Code = code;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x001ED291 File Offset: 0x001EB491
		public DTCPIDItem()
		{
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x001ED2A4 File Offset: 0x001EB4A4
		public DTCPIDItem(string code, byte UDSFlags)
		{
			this.Code = code;
			this.Flags = DTCPIDItem.GetUDSFlagsString(UDSFlags);
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x001ED2CA File Offset: 0x001EB4CA
		// (set) Token: 0x060028CE RID: 10446 RVA: 0x001ED2D2 File Offset: 0x001EB4D2
		public string Code
		{
			[CompilerGenerated]
			get
			{
				return this.<Code>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Code>k__BackingField = value;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x060028CF RID: 10447 RVA: 0x001ED2DB File Offset: 0x001EB4DB
		// (set) Token: 0x060028D0 RID: 10448 RVA: 0x001ED2E3 File Offset: 0x001EB4E3
		public string Flags
		{
			[CompilerGenerated]
			get
			{
				return this.<Flags>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Flags>k__BackingField = value;
			}
		} = "";

		// Token: 0x060028D1 RID: 10449 RVA: 0x001ED2EC File Offset: 0x001EB4EC
		public static string GetUDSFlagsString(byte UDSFlags)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Translate.GetString("ios_Status"));
			if ((UDSFlags & 1) == 1)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit0_testFailed"));
			}
			if ((UDSFlags & 2) == 2)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit1_testFailedThisOperationCycle"));
			}
			if ((UDSFlags & 4) == 4)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit2_pendingDTC"));
			}
			if ((UDSFlags & 8) == 8)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit3_confirmedDTC"));
			}
			if ((UDSFlags & 16) == 16)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit4_testNotCompletedSinceLastClear"));
			}
			if ((UDSFlags & 32) == 32)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit5_testFailedSinceLastClear"));
			}
			if ((UDSFlags & 64) == 64)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit6_testNotCompletedThisOperationCycle"));
			}
			if ((UDSFlags & 128) == 128)
			{
				stringBuilder.AppendLine(Translate.GetString("uds_bit7_warningIndicatorRequested"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040016BD RID: 5821
		[CompilerGenerated]
		private string <Code>k__BackingField;

		// Token: 0x040016BE RID: 5822
		[CompilerGenerated]
		private string <Flags>k__BackingField;
	}
}
