using System;
using System.Collections.Generic;
using System.Text;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000548 RID: 1352
	public static class DTCStatusHelper
	{
		// Token: 0x0600329F RID: 12959 RVA: 0x002305B4 File Offset: 0x0022E7B4
		public static List<DTCStatusHelper.DTCStatus> GetStatuses(byte data, string command)
		{
			if (string.IsNullOrEmpty(command))
			{
				return new List<DTCStatusHelper.DTCStatus>(0);
			}
			if (command.StartsWith("19") && App.OBDReader.CurrentProtocolNumber != 1 && App.OBDReader.CurrentProtocolNumber != 2)
			{
				return DTCStatusHelper.GetUDSStatuses(data);
			}
			if (command.StartsWith("18") || command.StartsWith("17") || command.StartsWith("13"))
			{
				return DTCStatusHelper.GetKWP2000Statuses(data);
			}
			if (command == "0A")
			{
				return new List<DTCStatusHelper.DTCStatus>(1) { DTCStatusHelper.DTCStatus.obd_0A_permanent };
			}
			if (command.StartsWith("19") && (App.OBDReader.CurrentProtocolNumber == 1 || App.OBDReader.CurrentProtocolNumber == 2))
			{
				return DTCStatusHelper.GetKWP2000Statuses(data);
			}
			if (command.StartsWith("A981") || command.StartsWith("03A981"))
			{
				return DTCStatusHelper.GetGMA981Status(data);
			}
			return new List<DTCStatusHelper.DTCStatus>(0);
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x0023069C File Offset: 0x0022E89C
		public static List<DTCStatusHelper.DTCStatus> GetGMA981Status(byte flags)
		{
			List<DTCStatusHelper.DTCStatus> list = new List<DTCStatusHelper.DTCStatus>(8);
			if ((flags & 1) == 1)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit0_DTCSupportedByCalibration);
			}
			if ((flags & 2) == 2)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit1_currentDTC);
			}
			if ((flags & 4) == 4)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit2_testNotPassedSinceDTCCleared);
			}
			if ((flags & 8) == 8)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit3_testFailedSinceDTCCleared);
			}
			if ((flags & 16) == 16)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit4_historyDTC);
			}
			if ((flags & 32) == 32)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit5_testNotPassedSinceCurrentPowerUp);
			}
			if ((flags & 64) == 64)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit6_currentDTCSincePowerUp);
			}
			if ((flags & 128) == 128)
			{
				list.Add(DTCStatusHelper.DTCStatus.GM_bit7_warningIndicatorRequestedState);
			}
			return list;
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x00230730 File Offset: 0x0022E930
		public static List<DTCStatusHelper.DTCStatus> GetKWP2000Statuses(byte KWP2000Flags)
		{
			List<DTCStatusHelper.DTCStatus> list = new List<DTCStatusHelper.DTCStatus>(8);
			if ((KWP2000Flags & 1) == 1)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit0_pendingFaultPresent);
			}
			if ((KWP2000Flags & 2) == 2)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit1_pendingFaultState);
			}
			if ((KWP2000Flags & 4) == 4)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit2_testRunning);
			}
			if ((KWP2000Flags & 8) == 8)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit3_testInhibit);
			}
			if ((KWP2000Flags & 16) == 16)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit4_testReadiness);
			}
			if ((KWP2000Flags & 32) == 32)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit5_DTCStorageState);
			}
			if ((KWP2000Flags & 64) == 64)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit6_validatedFaultPresent);
			}
			if ((KWP2000Flags & 128) == 128)
			{
				list.Add(DTCStatusHelper.DTCStatus.kwp2000_bit7_validatedFaultState);
			}
			return list;
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x002307C4 File Offset: 0x0022E9C4
		public static List<DTCStatusHelper.DTCStatus> GetUDSStatuses(byte UDSFlags)
		{
			List<DTCStatusHelper.DTCStatus> list = new List<DTCStatusHelper.DTCStatus>(8);
			if ((UDSFlags & 1) == 1)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit0_testFailed);
			}
			if ((UDSFlags & 2) == 2)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit1_testFailedThisOperationCycle);
			}
			if ((UDSFlags & 4) == 4)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit2_pendingDTC);
			}
			if ((UDSFlags & 8) == 8)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC);
			}
			if ((UDSFlags & 16) == 16)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit4_testNotCompletedSinceLastClear);
			}
			if ((UDSFlags & 32) == 32)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit5_testFailedSinceLastClear);
			}
			if ((UDSFlags & 64) == 64)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit6_testNotCompletedThisOperationCycle);
			}
			if ((UDSFlags & 128) == 128)
			{
				list.Add(DTCStatusHelper.DTCStatus.uds_bit7_warningIndicatorRequested);
			}
			return list;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x00230850 File Offset: 0x0022EA50
		public static string GetTitle(DTCStatusHelper.DTCStatus status)
		{
			switch (status)
			{
			case DTCStatusHelper.DTCStatus.uds_bit0_testFailed:
				return Translate.GetString("uds_bit0_testFailed");
			case DTCStatusHelper.DTCStatus.uds_bit1_testFailedThisOperationCycle:
				return Translate.GetString("uds_bit1_testFailedThisOperationCycle");
			case DTCStatusHelper.DTCStatus.uds_bit2_pendingDTC:
				return Translate.GetString("uds_bit2_pendingDTC");
			case DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC:
				return Translate.GetString("uds_bit3_confirmedDTC");
			case DTCStatusHelper.DTCStatus.uds_bit4_testNotCompletedSinceLastClear:
				return Translate.GetString("uds_bit4_testNotCompletedSinceLastClear");
			case DTCStatusHelper.DTCStatus.uds_bit5_testFailedSinceLastClear:
				return Translate.GetString("uds_bit5_testFailedSinceLastClear");
			case DTCStatusHelper.DTCStatus.uds_bit6_testNotCompletedThisOperationCycle:
				return Translate.GetString("uds_bit6_testNotCompletedThisOperationCycle");
			case DTCStatusHelper.DTCStatus.uds_bit7_warningIndicatorRequested:
				return Translate.GetString("uds_bit7_warningIndicatorRequested");
			case DTCStatusHelper.DTCStatus.obd_0A_permanent:
				return Translate.GetString("DtcPage_PermanentDTC");
			case DTCStatusHelper.DTCStatus.GM_Failure:
				return "GM failure record";
			}
			string text = status.ToString();
			if (Translate.HasString(text))
			{
				return Translate.GetString(text);
			}
			return "";
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x00230938 File Offset: 0x0022EB38
		private static string GetUDSFlagsString(byte UDSFlags)
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

		// Token: 0x02000549 RID: 1353
		public enum DTCStatus
		{
			// Token: 0x04001D5C RID: 7516
			uds_bit0_testFailed,
			// Token: 0x04001D5D RID: 7517
			uds_bit1_testFailedThisOperationCycle,
			// Token: 0x04001D5E RID: 7518
			uds_bit2_pendingDTC,
			// Token: 0x04001D5F RID: 7519
			uds_bit3_confirmedDTC,
			// Token: 0x04001D60 RID: 7520
			uds_bit4_testNotCompletedSinceLastClear,
			// Token: 0x04001D61 RID: 7521
			uds_bit5_testFailedSinceLastClear,
			// Token: 0x04001D62 RID: 7522
			uds_bit6_testNotCompletedThisOperationCycle,
			// Token: 0x04001D63 RID: 7523
			uds_bit7_warningIndicatorRequested,
			// Token: 0x04001D64 RID: 7524
			obd_0A_permanent,
			// Token: 0x04001D65 RID: 7525
			kwp2000_bit0_pendingFaultPresent,
			// Token: 0x04001D66 RID: 7526
			kwp2000_bit1_pendingFaultState,
			// Token: 0x04001D67 RID: 7527
			kwp2000_bit2_testRunning,
			// Token: 0x04001D68 RID: 7528
			kwp2000_bit3_testInhibit,
			// Token: 0x04001D69 RID: 7529
			kwp2000_bit4_testReadiness,
			// Token: 0x04001D6A RID: 7530
			kwp2000_bit5_DTCStorageState,
			// Token: 0x04001D6B RID: 7531
			kwp2000_bit6_validatedFaultPresent,
			// Token: 0x04001D6C RID: 7532
			kwp2000_bit7_validatedFaultState,
			// Token: 0x04001D6D RID: 7533
			GM_Failure,
			// Token: 0x04001D6E RID: 7534
			GM_bit7_warningIndicatorRequestedState,
			// Token: 0x04001D6F RID: 7535
			GM_bit6_currentDTCSincePowerUp,
			// Token: 0x04001D70 RID: 7536
			GM_bit5_testNotPassedSinceCurrentPowerUp,
			// Token: 0x04001D71 RID: 7537
			GM_bit4_historyDTC,
			// Token: 0x04001D72 RID: 7538
			GM_bit3_testFailedSinceDTCCleared,
			// Token: 0x04001D73 RID: 7539
			GM_bit2_testNotPassedSinceDTCCleared,
			// Token: 0x04001D74 RID: 7540
			GM_bit1_currentDTC,
			// Token: 0x04001D75 RID: 7541
			GM_bit0_DTCSupportedByCalibration
		}
	}
}
