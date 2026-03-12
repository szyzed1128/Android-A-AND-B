using System;
using System.Collections.Generic;
using System.IO;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000542 RID: 1346
	internal static class DTCDecoder
	{
		// Token: 0x0600324E RID: 12878 RVA: 0x0022E4C8 File Offset: 0x0022C6C8
		public static List<DTCItemV2> DecodeData(OBDRequest request, string header, byte[] data)
		{
			List<DTCItemV2> list = new List<DTCItemV2>();
			DTCReadingMode dtcreadingMode;
			if (request.Command == "03" || request.Command == "07" || request.Command == "0A" || request.Command == "13")
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
				{
					dtcreadingMode = DTCReadingMode.CAN;
				}
				else
				{
					dtcreadingMode = DTCReadingMode.ISO;
				}
			}
			else if (request.Command == "19004000" && SharedSettings.Current.SelectedBrand == "KTM")
			{
				dtcreadingMode = DTCReadingMode.KWPMode18;
			}
			else if (request.Command.StartsWith("19", StringComparison.OrdinalIgnoreCase))
			{
				dtcreadingMode = DTCReadingMode.UDSMode19;
			}
			else if (request.Command.StartsWith("18", StringComparison.OrdinalIgnoreCase))
			{
				dtcreadingMode = DTCReadingMode.KWPMode18;
			}
			else if (request.Command.StartsWith("17", StringComparison.OrdinalIgnoreCase))
			{
				dtcreadingMode = DTCReadingMode.KWPMode18;
			}
			else if (request.Command.StartsWith("13", StringComparison.OrdinalIgnoreCase))
			{
				if (request.ELMFormat == ELMFormat.CAN11bit || request.ELMFormat == ELMFormat.CAN29bit)
				{
					dtcreadingMode = DTCReadingMode.CAN;
				}
				else
				{
					dtcreadingMode = DTCReadingMode.ISO;
				}
			}
			else if (App.OBDReader.IsNissanConsult2Protocol || request.Command == "A3")
			{
				dtcreadingMode = DTCReadingMode.NissanConsult2;
			}
			else if (request.Command == "1201")
			{
				dtcreadingMode = DTCReadingMode.GM12;
			}
			else
			{
				dtcreadingMode = DTCReadingMode.KWP_VAZ;
			}
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				switch (dtcreadingMode)
				{
				case DTCReadingMode.CAN:
					DTCDecoder.DecodeCAN(memoryStream, list, request, header);
					break;
				case DTCReadingMode.ISO:
					DTCDecoder.DecodeISO(memoryStream, list, request, header);
					break;
				case DTCReadingMode.NissanConsult2:
					DTCDecoder.DecodeNissanConsult2(memoryStream, list, request, header);
					break;
				case DTCReadingMode.UDSMode19:
					if (request.Command.StartsWith("190F") && SharedSettings.Current.HideArchiveDTC)
					{
						return list;
					}
					DTCDecoder.DecodeUDSMode19(memoryStream, list, request, header);
					break;
				case DTCReadingMode.KWPMode18:
					DTCDecoder.DecodeKWPMode18(memoryStream, list, request, header);
					break;
				case DTCReadingMode.GM12:
					DTCDecoder.DecodeGM12(memoryStream, list, request, header);
					break;
				}
			}
			return list;
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x0022E6DC File Offset: 0x0022C8DC
		private static void DecodeUDSMode19(MemoryStream ms, List<DTCItemV2> result, OBDRequest request, string header)
		{
			if (!request.Command.StartsWith("19D2") || (App.OBDReader.CurrentProtocolNumber != 1 && App.OBDReader.CurrentProtocolNumber != 2))
			{
				ms.ReadByte();
			}
			while (ms.Position <= ms.Length - 4L)
			{
				try
				{
					byte[] array = new byte[3];
					ms.Read(array, 0, 3);
					byte b = (byte)ms.ReadByte();
					if ((array[0] != 170 || array[1] != 170 || array[2] != 170) && (array[0] != 0 || array[1] != 0 || array[2] != 0) && (array[0] != 0 || array[1] != 0 || !request.Command.StartsWith("19D2") || (App.OBDReader.CurrentProtocolNumber != 1 && App.OBDReader.CurrentProtocolNumber != 2)))
					{
						if (request.Command.StartsWith("19") && (App.OBDReader.CurrentProtocolNumber == 1 || App.OBDReader.CurrentProtocolNumber == 2))
						{
							byte[] array2 = new byte[]
							{
								array[0],
								array[1]
							};
							b = array[2];
							DTCItemV2 dtcitemV = new DTCItemV2(array2, request.Header, header, request.Command, b, request.Payload);
							result.Add(dtcitemV);
						}
						else
						{
							DTCItemV2 dtcitemV2 = new DTCItemV2(array, request.Header, header, request.Command, b, request.Payload);
							result.Add(dtcitemV2);
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x0022E86C File Offset: 0x0022CA6C
		private static void DecodeKWPMode18(MemoryStream ms, List<DTCItemV2> result, OBDRequest request, string header)
		{
			ELMFormat elmformat = request.ELMFormat;
			if (elmformat == ELMFormat.Unknown)
			{
				elmformat = App.OBDReader.CurrentELMFormat;
			}
			if (request.Command == "18FFFFFF" && elmformat == ELMFormat.KWP)
			{
				if (!(SharedSettings.Current.SelectedBrand == "BMW"))
				{
					if (!(SharedSettings.Current.SelectedBrand == "Mini"))
					{
						goto IL_00D4;
					}
				}
				while (ms.Position <= ms.Length - 3L)
				{
					byte[] array = new byte[2];
					ms.Read(array, 0, 2);
					byte b = (byte)ms.ReadByte();
					if ((array[0] != 170 || array[1] != 170 || array[3] != 170) && (array[0] != 0 || array[1] != 0))
					{
						DTCItemV2 dtcitemV = new DTCItemV2(array, request.Header, header, request.Command, b, request.Payload);
						result.Add(dtcitemV);
					}
				}
				return;
			}
			IL_00D4:
			if (elmformat == ELMFormat.KWP && (SharedSettings.Current.BrandForDTC == "Delphi" || SharedSettings.Current.ProfileUpdateAlias == "2236304d4ed9492e8c84e291523253b6" || SharedSettings.Current.ProfileUpdateAlias == "ECU Delphi MT20U" || SharedSettings.Current.ProfileUpdateAlias == "9e4a3193ea8544939dc7feac4227c84d" || SharedSettings.Current.ProfileUpdateAlias == "ECU Delphi MT20U2"))
			{
				int num = ms.ReadByte();
				if (num == 0)
				{
					return;
				}
				int num2 = 0;
				while (ms.Position <= ms.Length - 3L)
				{
					if (num2 >= num)
					{
						return;
					}
					byte[] array2 = new byte[2];
					ms.Read(array2, 0, 2);
					byte b2 = array2[0];
					array2[0] = array2[1];
					array2[1] = b2;
					byte b3 = (byte)ms.ReadByte();
					if ((array2[0] != 170 || array2[1] != 170 || array2[3] != 170) && (array2[0] != 0 || array2[1] != 0))
					{
						DTCItemV2 dtcitemV2 = new DTCItemV2(array2, request.Header, header, request.Command, b3, request.Payload);
						result.Add(dtcitemV2);
						num2++;
					}
				}
			}
			else
			{
				int num3 = ms.ReadByte();
				if (num3 == 0)
				{
					return;
				}
				int num4 = 0;
				while (ms.Position <= ms.Length - 3L && num4 < num3)
				{
					byte[] array3 = new byte[2];
					ms.Read(array3, 0, 2);
					byte b4 = (byte)ms.ReadByte();
					if ((array3[0] != 170 || array3[1] != 170 || array3[3] != 170) && (array3[0] != 0 || array3[1] != 0))
					{
						DTCItemV2 dtcitemV3 = new DTCItemV2(array3, request.Header, header, request.Command, b4, request.Payload);
						result.Add(dtcitemV3);
					}
					num4++;
				}
			}
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x0022EB24 File Offset: 0x0022CD24
		private static void DecodeCAN(MemoryStream ms, List<DTCItemV2> result, OBDRequest request, string header)
		{
			int num = ms.ReadByte();
			if (num == 0)
			{
				return;
			}
			while (ms.Position <= ms.Length - 2L)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				if ((array[0] != 0 || array[1] != 0) && (array[0] != 170 || array[1] != 170))
				{
					DTCStatusHelper.DTCStatus dtcstatus = DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC;
					if (request.Command == "07")
					{
						dtcstatus = DTCStatusHelper.DTCStatus.uds_bit2_pendingDTC;
					}
					else if (request.Command == "0A")
					{
						dtcstatus = DTCStatusHelper.DTCStatus.obd_0A_permanent;
					}
					DTCItemV2 dtcitemV = new DTCItemV2(array, request.Header, header, request.Command, dtcstatus, request.Payload);
					result.Add(dtcitemV);
				}
				if (result.Count == num)
				{
					break;
				}
			}
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x0022EBDC File Offset: 0x0022CDDC
		private static void DecodeISO(MemoryStream ms, List<DTCItemV2> result, OBDRequest request, string header)
		{
			while (ms.Position <= ms.Length - 2L)
			{
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				if (array[0] != 0 || array[1] != 0)
				{
					DTCStatusHelper.DTCStatus dtcstatus = DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC;
					if (request.Command == "07")
					{
						dtcstatus = DTCStatusHelper.DTCStatus.uds_bit2_pendingDTC;
					}
					else if (request.Command == "0A")
					{
						dtcstatus = DTCStatusHelper.DTCStatus.obd_0A_permanent;
					}
					DTCItemV2 dtcitemV = new DTCItemV2(array, request.Header, header, request.Command, dtcstatus, request.Payload);
					result.Add(dtcitemV);
				}
			}
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x0022EC68 File Offset: 0x0022CE68
		private static void DecodeNissanConsult2(MemoryStream ms, List<DTCItemV2> result, OBDRequest request, string header)
		{
			int num = ms.ReadByte();
			while (ms.Position <= ms.Length - 3L)
			{
				byte[] array = new byte[3];
				ms.Read(array, 0, 3);
				byte b = array[2];
				if (array[0] != 0 || array[1] != 0)
				{
					DTCStatusHelper.DTCStatus dtcstatus = DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC;
					DTCItemV2 dtcitemV = new DTCItemV2(array, request.Header, header, request.Command, dtcstatus, request.Payload);
					result.Add(dtcitemV);
				}
				if (result.Count == num)
				{
					break;
				}
			}
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x0022ECDC File Offset: 0x0022CEDC
		internal static DTCItemV2 DecodeGMA981(byte[] data, OBDRequest request, string header)
		{
			DTCItemV2 dtcitemV;
			try
			{
				if (data == null || data.Length < 5)
				{
					dtcitemV = null;
				}
				else if (data[0] != 129)
				{
					dtcitemV = null;
				}
				else if (data[1] == 0 && data[2] == 0)
				{
					dtcitemV = null;
				}
				else
				{
					byte[] array = new byte[]
					{
						data[1],
						data[2]
					};
					byte b = data[3];
					byte b2 = data[4];
					DTCItemV2 dtcitemV2 = new DTCItemV2(array, request.Header, header, request.Command, b2, "");
					string gmfailureType = DTCDecoder.GetGMFailureType((int)b);
					dtcitemV2.Payload = gmfailureType;
					dtcitemV = dtcitemV2;
				}
			}
			catch (Exception)
			{
				dtcitemV = null;
			}
			return dtcitemV;
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x0022ED6C File Offset: 0x0022CF6C
		private static void DecodeGM12(MemoryStream ms, List<DTCItemV2> result, OBDRequest request, string header)
		{
			if (ms.Position >= ms.Length)
			{
				return;
			}
			ms.ReadByte();
			while (ms.Position <= ms.Length - 4L)
			{
				ms.ReadByte();
				byte[] array = new byte[2];
				ms.Read(array, 0, 2);
				int num = ms.ReadByte();
				if (array[0] != 0 || array[1] != 0)
				{
					DTCItemV2 dtcitemV = new DTCItemV2(array, request.Header, header, request.Command, DTCStatusHelper.DTCStatus.GM_Failure, request.Payload);
					string gmfailureType = DTCDecoder.GetGMFailureType(num);
					dtcitemV.Payload = gmfailureType;
					result.Add(dtcitemV);
				}
			}
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x0022EDFC File Offset: 0x0022CFFC
		private static string GetGMFailureType(int b)
		{
			int num = b >> 4;
			string text = "";
			switch (num)
			{
			case 0:
				text = "General Electrical Failures";
				break;
			case 1:
				text = "Additional General Electrical Failures";
				break;
			case 2:
				text = "FM/PWM (Frequency/Pulse Width Modulated) Failures";
				break;
			case 3:
				text = "ECU Internal Failures";
				break;
			case 4:
				text = "ECU Programming Failures";
				break;
			case 5:
				text = "Algorithm Based Failures";
				break;
			case 6:
				text = "Mechanical Failures";
				break;
			case 7:
				text = "Bus Signal/Message Failures";
				break;
			}
			string text2 = "";
			switch (b)
			{
			case 1:
				text2 = "short to battery";
				break;
			case 2:
				text2 = "short to ground";
				break;
			case 3:
				text2 = "voltage below threshold";
				break;
			case 4:
				text2 = "open circuit";
				break;
			case 5:
				text2 = "short to battery or open";
				break;
			case 6:
				text2 = "short to ground or open";
				break;
			case 7:
				text2 = "voltage above threshold";
				break;
			case 8:
				text2 = "signal invalid";
				break;
			case 9:
				text2 = "rate of change above threshold";
				break;
			case 10:
				text2 = "rate of change below threshold";
				break;
			case 11:
				text2 = "current above threshold";
				break;
			case 12:
				text2 = "current below threshold";
				break;
			case 13:
				text2 = "resistance above threshold";
				break;
			case 14:
				text2 = "resistance below threshold";
				break;
			case 15:
				text2 = "erratic";
				break;
			case 17:
				text2 = "above maximum threshold ";
				break;
			case 18:
				text2 = "below minimum threshold";
				break;
			case 19:
				text2 = "voltage low/high temperature ";
				break;
			case 20:
				text2 = "voltage high/low temperature";
				break;
			case 21:
				text2 = "signal rising time failure";
				break;
			case 22:
				text2 = "signal falling time failure";
				break;
			case 23:
				text2 = "signal shape/ waveform failure ";
				break;
			case 24:
				text2 = "signal amplitude < minimum";
				break;
			case 25:
				text2 = "signal amplitude > maximum ";
				break;
			case 26:
				text2 = "bias level out of range";
				break;
			case 27:
				text2 = "signal cross coupled";
				break;
			case 31:
				text2 = "intermittent";
				break;
			case 33:
				text2 = "incorrect period";
				break;
			case 34:
				text2 = "low time < minimum";
				break;
			case 35:
				text2 = "low time > maximum ";
				break;
			case 36:
				text2 = "high time < minimum";
				break;
			case 37:
				text2 = "high time > maximum";
				break;
			case 38:
				text2 = "frequency too low";
				break;
			case 39:
				text2 = "frequency too high";
				break;
			case 40:
				text2 = "incorrect frequency";
				break;
			case 41:
				text2 = "too few pulses";
				break;
			case 42:
				text2 = "too many pulses";
				break;
			case 43:
				text2 = "missing reference";
				break;
			case 49:
				text2 = "general checksum failure";
				break;
			case 50:
				text2 = "general memory failure ";
				break;
			case 51:
				text2 = "special memory failure ";
				break;
			case 52:
				text2 = "RAM failure";
				break;
			case 53:
				text2 = "ROM failure";
				break;
			case 54:
				text2 = "EEPROM failure";
				break;
			case 55:
				text2 = "watchdog/safety µC failure";
				break;
			case 56:
				text2 = "supervision software failure ";
				break;
			case 57:
				text2 = "internal electronic failure";
				break;
			case 58:
				text2 = "incorrect component installed ";
				break;
			case 59:
				text2 = "Internal Self Test Failed";
				break;
			case 60:
				text2 = "Internal Communications Failure";
				break;
			case 65:
				text2 = "operational software/calibration set not programmed";
				break;
			case 66:
				text2 = "calibration data set not programmed";
				break;
			case 67:
				text2 = "EEPROM error";
				break;
			case 68:
				text2 = "security access not activated ";
				break;
			case 69:
				text2 = "variant not programmed";
				break;
			case 70:
				text2 = "vehicle configuration not programmed";
				break;
			case 71:
				text2 = "VIN not programmed";
				break;
			case 72:
				text2 = "theft/security data not programmed";
				break;
			case 73:
				text2 = "RAM error";
				break;
			case 74:
				text2 = "checksum error";
				break;
			case 75:
				text2 = "calibration not learned";
				break;
			case 76:
				text2 = "DTC memory full";
				break;
			case 77:
				text2 = "stack overflow";
				break;
			case 83:
				text2 = "temperature low ";
				break;
			case 84:
				text2 = "temperature high";
				break;
			case 85:
				text2 = "expected number of transitions/events not reached";
				break;
			case 86:
				text2 = "allowable number of transitions/events exceeded";
				break;
			case 88:
				text2 = "incorrect reaction after event";
				break;
			case 89:
				text2 = "circuit/component protection time-out";
				break;
			case 90:
				text2 = "plausibility failure";
				break;
			case 97:
				text2 = "actuator stuck";
				break;
			case 98:
				text2 = "actuator stuck open";
				break;
			case 99:
				text2 = "actuator stuck closed";
				break;
			case 100:
				text2 = "actuator slipping";
				break;
			case 101:
				text2 = "emergency position not reachable";
				break;
			case 102:
				text2 = "wrong mounting position";
				break;
			case 103:
				text2 = "incorrect assembly ";
				break;
			case 113:
				text2 = "invalid serial data received";
				break;
			case 114:
				text2 = "alive counter incorrect/not updated";
				break;
			case 115:
				text2 = "parity error";
				break;
			case 116:
				text2 = "value of signal protection calculation incorrect";
				break;
			case 117:
				text2 = "signal above allowable range ";
				break;
			case 118:
				text2 = "signal below allowable range";
				break;
			case 127:
				text2 = "Erratic";
				break;
			}
			string text3 = text;
			if (!string.IsNullOrEmpty(text2))
			{
				text3 = text3 + ": " + text2;
			}
			text3 = text3.Trim();
			return text3 + " [0x" + b.ToString("X2") + "]";
		}
	}
}
