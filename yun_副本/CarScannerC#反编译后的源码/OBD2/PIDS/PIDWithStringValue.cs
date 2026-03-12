using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000412 RID: 1042
	public class PIDWithStringValue : PID, IPIDWithStringValue, IPID, INotifyPropertyChanged
	{
		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06002CAD RID: 11437 RVA: 0x001FC67C File Offset: 0x001FA87C
		// (set) Token: 0x06002CAE RID: 11438 RVA: 0x001FC684 File Offset: 0x001FA884
		public string Value
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

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06002CAF RID: 11439 RVA: 0x001FC693 File Offset: 0x001FA893
		// (set) Token: 0x06002CB0 RID: 11440 RVA: 0x001FC69B File Offset: 0x001FA89B
		public int IntValue
		{
			get
			{
				return this._IntValue;
			}
			protected set
			{
				this._IntValue = value;
				this.NotifyPropertyChanged("IntValue");
			}
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x001FC6AF File Offset: 0x001FA8AF
		public PIDWithStringValue(string Command, Func<byte[], string> Formula, bool useOnlySelectedHeader = false)
			: base(PID.GetResourceString("PID_" + Command), Command)
		{
			this.Formula = Formula;
			this.UseOnlySelectedHeader = useOnlySelectedHeader;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x001FC6E1 File Offset: 0x001FA8E1
		public PIDWithStringValue(string Name, string Command, Func<byte[], string> Formula, bool useOnlySelectedHeader = false)
			: base(Name, Command)
		{
			this.Formula = Formula;
			this.UseOnlySelectedHeader = useOnlySelectedHeader;
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x001FC708 File Offset: 0x001FA908
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			if (this.UseOnlySelectedHeader && !App.OBDSimulator.IsActive)
			{
				try
				{
					if (response_header != App.OBDReader.ECUHeaders[App.OBDReader.SelectedECU].Id)
					{
						return;
					}
				}
				catch
				{
				}
			}
			base.TimeStamp = timeStamp;
			if (data.Length != 0)
			{
				this.IntValue = (int)data[0];
			}
			this.Value = this.Formula(data);
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x001FC78C File Offset: 0x001FA98C
		public static PIDWithStringValue PID011F_RunTimeSinceEngineStart()
		{
			return new PIDWithStringValue("011F", (byte[] data) => TimeSpan.FromSeconds(PIDWithStringValue.ABFormula(data)).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture), false)
			{
				Id = 39
			};
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x001FC7C0 File Offset: 0x001FA9C0
		public static PIDWithStringValue PID014D_TimeRunWithMILon()
		{
			return new PIDWithStringValue("014D", (byte[] data) => TimeSpan.FromMinutes(PIDWithStringValue.ABFormula(data)).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture), false)
			{
				Id = 101
			};
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x001FC7F4 File Offset: 0x001FA9F4
		public static PIDWithStringValue PID014E_TimeSinceTroubleCodesCleared()
		{
			return new PIDWithStringValue("014E", (byte[] data) => TimeSpan.FromMinutes(PIDWithStringValue.ABFormula(data)).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture), false)
			{
				Id = 102
			};
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x001FC828 File Offset: 0x001FAA28
		public static PIDWithStringValue PID011C_ObdStandard()
		{
			return new PIDWithStringValue("011C", delegate(byte[] data)
			{
				string[] array = new string[]
				{
					"Unknown", "OBD-II as defined by the CARB", "OBD as defined by the EPA", "OBD and OBD-II", "OBD-I", "Not OBD compliant", "EOBD (Europe)", "EOBD and OBD-II", "EOBD and OBD", "EOBD, OBD and OBD II",
					"JOBD (Japan)", "JOBD and OBD II", "JOBD and EOBD", "JOBD, EOBD, and OBD II", "Reserved", "Reserved", "Reserved", "Engine Manufacturer Diagnostics (EMD)", "Engine Manufacturer Diagnostics Enhanced (EMD+)", "Heavy Duty On-Board Diagnostics (Child/Partial) (HD OBD-C)",
					"Heavy Duty On-Board Diagnostics (HD OBD)", "World Wide Harmonized OBD (WWH OBD)", "Reserved", "Heavy Duty Euro OBD Stage I without NOx control (HD EOBD-I)", "Heavy Duty Euro OBD Stage I with NOx control (HD EOBD-I N)", "Heavy Duty Euro OBD Stage II without NOx control (HD EOBD-II)", "Heavy Duty Euro OBD Stage II with NOx control (HD EOBD-II N)", "Reserved", "Brazil OBD Phase 1 (OBDBr-1)", "Brazil OBD Phase 2 (OBDBr-2)",
					"Korean OBD (KOBD)", "India OBD I (IOBD I)", "India OBD II (IOBD II)", "Heavy Duty Euro OBD Stage VI (HD EOBD-IV)"
				};
				if ((int)data[0] < array.Length)
				{
					return array[(int)data[0]];
				}
				return array[0];
			}, false)
			{
				Id = 36
			};
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x001FC85C File Offset: 0x001FAA5C
		public static PIDWithStringValue PID0112_CommandedSecondaryAirStatus()
		{
			return new PIDWithStringValue("0112", delegate(byte[] data)
			{
				byte b = data[0];
				switch (b)
				{
				case 1:
					return "Upstream";
				case 2:
					return "Downstream of catalytic converter";
				case 3:
					break;
				case 4:
					return "From the outside atmosphere or off";
				default:
					if (b == 8)
					{
						return "Pump commanded on for diagnostics";
					}
					break;
				}
				return "None";
			}, false)
			{
				Id = 18
			};
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x001FC890 File Offset: 0x001FAA90
		public static PIDWithStringValue PID011E_AuxillaryInputStatus()
		{
			return new PIDWithStringValue("011E", delegate(byte[] data)
			{
				if (data[0] == 1)
				{
					return "Power take off: active";
				}
				return "Power take off: not active";
			}, false)
			{
				Id = 38
			};
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x001FC8C4 File Offset: 0x001FAAC4
		public static PIDWithStringValue PID0151_FuelType()
		{
			return new PIDWithStringValue("0151", delegate(byte[] data)
			{
				switch (data[0])
				{
				case 1:
					return "Gasoline";
				case 2:
					return "Methanol";
				case 3:
					return "Ethanol";
				case 4:
					return "Diesel";
				case 5:
					return "LPG";
				case 6:
					return "CNG";
				case 7:
					return "Propane";
				case 8:
					return "Electric";
				case 9:
					return "Bifuel\u00a0running Gasoline";
				case 10:
					return "Bifuel running Methanol";
				case 11:
					return "Bifuel running Ethanol";
				case 12:
					return "Bifuel running LPG";
				case 13:
					return "Bifuel running CNG";
				case 14:
					return "Bifuel running Propane";
				case 15:
					return "Bifuel running Electricity";
				case 16:
					return "Bifuel running electric and combustion engine";
				case 17:
					return "Hybrid gasoline";
				case 18:
					return "Hybrid Ethanol";
				case 19:
					return "Hybrid Diesel";
				case 20:
					return "Hybrid Electric";
				case 21:
					return "Hybrid running electric and combustion engine";
				case 22:
					return "Hybrid Regenerative";
				case 23:
					return "Bifuel running diesel";
				}
				return "Not available";
			}, false)
			{
				Id = 106
			};
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x001FC8F8 File Offset: 0x001FAAF8
		public static PIDWithStringValue PID0102_FreezFrameDTC()
		{
			return new PIDWithStringValue("0102", delegate(byte[] data)
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && data != null && data.Length > 2)
				{
					data = new byte[]
					{
						data[0],
						data[1]
					};
				}
				return new DTCItemV2(data, "", "", "03", DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC, "").Code;
			}, false);
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x001FC924 File Offset: 0x001FAB24
		internal static string FilterVin(string vin)
		{
			return new string((from c in vin.ToCharArray()
				where "0123456789ABCDEFGHJKLMNPRSTUVWXYZ".Contains(c)
				select c).ToArray<char>());
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x001FC95A File Offset: 0x001FAB5A
		private static string FilterECUName(string name)
		{
			return new string((from c in name.ToCharArray()
				where "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ :,./;'~-=+_".Contains(c)
				select c).ToArray<char>());
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x001FC990 File Offset: 0x001FAB90
		public static PIDWithStringValue PID0902_VIN()
		{
			return new PIDWithStringValue("0902", (byte[] arg) => PIDWithStringValue.FilterVin(PIDWithStringValue.Mode09TextFormula(arg)), false)
			{
				Id = 242
			};
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x001FC9C7 File Offset: 0x001FABC7
		public static PIDWithStringValue PID0904_CalibrationID()
		{
			return new PIDWithStringValue("0904", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.Mode09TextFormula(arg)), true)
			{
				Id = 243
			};
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x001FC9FE File Offset: 0x001FABFE
		public static PIDWithStringValue PID0906_CalibrationVerificationNumber()
		{
			return new PIDWithStringValue("0906", (byte[] arg) => PIDWithStringValue.Mode09HexString(arg), true)
			{
				Id = 813
			};
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x001FCA35 File Offset: 0x001FAC35
		public static PIDWithStringValue PID090A_EcuName()
		{
			return new PIDWithStringValue("090A", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.Mode09TextFormula(arg)), true)
			{
				Id = 244
			};
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x001FCA6C File Offset: 0x001FAC6C
		public static PIDWithStringValue PID017F_TotalEngineRunTime()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_017F_1"), "017F", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length >= 5)
				{
					byte[] array = new byte[]
					{
						data[1],
						data[2],
						data[3],
						data[4]
					};
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					return TimeSpan.FromSeconds((double)((ulong)BitConverter.ToUInt32(array, 0))).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
				}
				return "n/a";
			}, false)
			{
				Id = 213
			};
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x001FCAB8 File Offset: 0x001FACB8
		public static PIDWithStringValue PID017F_TotalIdleRunTime()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_017F_2"), "017F", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2))
				{
					byte[] array = new byte[]
					{
						data[5],
						data[6],
						data[7],
						data[8]
					};
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					return TimeSpan.FromSeconds((double)((ulong)BitConverter.ToUInt32(array, 0))).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
				}
				return "";
			}, false)
			{
				Id = 214
			};
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x001FCB04 File Offset: 0x001FAD04
		public static PIDWithStringValue PID017F_TotalRunTimeWithPTOActive()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_017F_3"), "017F", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3))
				{
					byte[] array = new byte[]
					{
						data[9],
						data[10],
						data[11],
						data[12]
					};
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					return TimeSpan.FromSeconds((double)((ulong)BitConverter.ToUInt32(array, 0))).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
				}
				return "";
			}, false)
			{
				Id = 215
			};
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x001FCB50 File Offset: 0x001FAD50
		public static PIDWithStringValue PID1A90_KWP_VIN()
		{
			string @string = Translate.GetString("PID_0902");
			return new PIDWithStringValue("1A90", (byte[] arg) => PIDWithStringValue.FilterVin(PIDWithStringValue.ASCIITextFormula(arg)), false)
			{
				Id = 546,
				Name = @string,
				ShortName = @string
			};
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x001FCBAB File Offset: 0x001FADAB
		public static PIDWithStringValue PID1A91_KWP_VehicleManufacturerECUHardwareNumber()
		{
			return new PIDWithStringValue("1A91", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 547
			};
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x001FCBE2 File Offset: 0x001FADE2
		public static PIDWithStringValue PID1A92_KWP_SystemSupplierECUHardwareNumber()
		{
			return new PIDWithStringValue("1A92", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 548
			};
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x001FCC19 File Offset: 0x001FAE19
		public static PIDWithStringValue PID1A93_KWP_SystemSupplierECUHardwareVersion()
		{
			return new PIDWithStringValue("1A93", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 549
			};
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x001FCC50 File Offset: 0x001FAE50
		public static PIDWithStringValue PID1A94_KWP_SystemSupplierECUSoftwareNumber()
		{
			return new PIDWithStringValue("1A94", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 550
			};
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x001FCC87 File Offset: 0x001FAE87
		public static PIDWithStringValue PID1A95_KWP_SystemSupplierECUSoftwareVersion()
		{
			return new PIDWithStringValue("1A95", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 551
			};
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x001FCCBE File Offset: 0x001FAEBE
		public static PIDWithStringValue PID1A96_KWP_ExhaustRegulationOrTypeApprovalNumber()
		{
			return new PIDWithStringValue("1A96", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 552
			};
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x001FCCF5 File Offset: 0x001FAEF5
		public static PIDWithStringValue PID1A97_KWP_SystemNameOrEngineType()
		{
			return new PIDWithStringValue("1A97", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 553
			};
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x001FCD2C File Offset: 0x001FAF2C
		public static PIDWithStringValue PID1A98_KWP_RepairShopCodeOrTesterSerialNumber()
		{
			return new PIDWithStringValue("1A98", (byte[] arg) => PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg)), true)
			{
				Id = 554
			};
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x001FCD63 File Offset: 0x001FAF63
		public static PIDWithStringValue PID1A99_KWP_ProgrammingDate()
		{
			return new PIDWithStringValue("1A99", delegate(byte[] arg)
			{
				if (arg.Length < 4)
				{
					return "";
				}
				string text = arg[0].ToString("X2") + arg[1].ToString("X2");
				string text2 = arg[2].ToString("X2");
				string text3 = arg[3].ToString("X2");
				return string.Concat(new string[] { text, ".", text2, ".", text3 });
			}, true)
			{
				Id = 555
			};
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x001FCD9C File Offset: 0x001FAF9C
		public static PIDWithStringValue[] GetPIDs_0181()
		{
			PIDWithStringValue[] array = new PIDWithStringValue[10];
			int num = 665;
			for (int i = 0; i < array.Length; i++)
			{
				int sup_bit = i / 2;
				int start_byte = 1 + i * 4;
				array[i] = new PIDWithStringValue(PID.GetResourceString("PID_0181_" + (i + 1).ToString(CultureInfo.InvariantCulture.NumberFormat)), "0181", (byte[] data) => PIDWithStringValue.Time4BytesWithSupportedBit(data, sup_bit, start_byte), false)
				{
					Id = num + i
				};
			}
			return array;
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x001FCE28 File Offset: 0x001FB028
		public static PIDWithStringValue[] GetPIDs_0182()
		{
			PIDWithStringValue[] array = new PIDWithStringValue[10];
			int num = 676;
			for (int i = 0; i < array.Length; i++)
			{
				int sup_bit = i / 2;
				int start_byte = 1 + i * 4;
				array[i] = new PIDWithStringValue(PID.GetResourceString("PID_0182_" + (i + 1).ToString(CultureInfo.InvariantCulture.NumberFormat)), "0182", (byte[] data) => PIDWithStringValue.Time4BytesWithSupportedBit(data, sup_bit, start_byte), false)
				{
					Id = num + i
				};
			}
			return array;
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x001FCEB4 File Offset: 0x001FB0B4
		public static PIDWithStringValue PID_0185_4()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_0185_4"), "0185", (byte[] data) => PIDWithStringValue.Time4BytesWithSupportedBit(data, 3, 6), false)
			{
				Id = 687
			};
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x001FCF00 File Offset: 0x001FB100
		public static PIDWithStringValue PID_0188_1()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_0188_1"), "0188", delegate(byte[] data)
			{
				if (data.Length <= 6)
				{
					return "";
				}
				byte b = data[0];
				StringBuilder stringBuilder = new StringBuilder();
				if (BitHelpers.GetBit_1_8(b, 8))
				{
					stringBuilder.Append("Active");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				else
				{
					stringBuilder.Append("Disabled");
				}
				b = data[1];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 10K history (0 - 10,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				b = data[3];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 20K history (10,000 - 20,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				b = data[4];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 30K history (20,000  - 30,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				b = data[5];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 40K history (30,000 - 40,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				return stringBuilder.ToString();
			}, false)
			{
				Id = 688
			};
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x001FCF4C File Offset: 0x001FB14C
		public static PIDWithStringValue[] GetPIDs_0189()
		{
			PIDWithStringValue[] array = new PIDWithStringValue[10];
			int num = 689;
			for (int i = 0; i < array.Length; i++)
			{
				int sup_bit = i / 2;
				int start_byte = 1 + i * 4;
				array[i] = new PIDWithStringValue(PID.GetResourceString("PID_0189_" + (i + 1).ToString(CultureInfo.InvariantCulture.NumberFormat)), "0189", (byte[] data) => PIDWithStringValue.Time4BytesWithSupportedBit(data, sup_bit, start_byte), false)
				{
					Id = num + i
				};
			}
			return array;
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x001FCFD8 File Offset: 0x001FB1D8
		public static PIDWithStringValue[] GetPIDs_018A()
		{
			PIDWithStringValue[] array = new PIDWithStringValue[10];
			int num = 700;
			for (int i = 0; i < array.Length; i++)
			{
				int sup_bit = i / 2;
				int start_byte = 1 + i * 4;
				array[i] = new PIDWithStringValue(PID.GetResourceString("PID_018A_" + (i + 1).ToString(CultureInfo.InvariantCulture.NumberFormat)), "018A", (byte[] data) => PIDWithStringValue.Time4BytesWithSupportedBit(data, sup_bit, start_byte), false)
				{
					Id = num + i
				};
			}
			return array;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x001FD064 File Offset: 0x001FB264
		public static PIDWithStringValue PID_0190_1()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_0190_1"), "0190", delegate(byte[] data)
			{
				int num = (data[0] >> 2) & 15;
				switch (num)
				{
				case 0:
					return "MI Off";
				case 1:
					return "On Demand MI";
				case 2:
					return "Short MI";
				case 3:
					return "Continuous MI";
				default:
					if (num == 14)
					{
						return "Error";
					}
					if (num != 15)
					{
						return "Unknown";
					}
					return "Not available/Not required of this vehicle";
				}
			}, false)
			{
				Id = 711
			};
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x001FD0B0 File Offset: 0x001FB2B0
		public static PIDWithStringValue PID_0191_1()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_0191_1"), "0191", delegate(byte[] data)
			{
				int num = (int)(data[0] & 15);
				switch (num)
				{
				case 0:
					return "MI Off";
				case 1:
					return "On Demand MI";
				case 2:
					return "Short MI";
				case 3:
					return "Continuous MI";
				default:
					if (num == 14)
					{
						return "Error";
					}
					if (num != 15)
					{
						return "Unknown";
					}
					return "Not available/Not required of this vehicle";
				}
			}, false)
			{
				Id = 712
			};
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x001FD0FC File Offset: 0x001FB2FC
		public static PIDWithStringValue PID_019A_1()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_019A_1"), "019A", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (!(ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : (BitHelpers.GetBit_1_8(data[0], 1) || BitHelpers.GetBit_1_8(data[0], 4))))
				{
					return "";
				}
				if (BitHelpers.GetBit_1_8(data[0], 4))
				{
					bool bit_1_ = BitHelpers.GetBit_1_8(data[1], 2);
					bool bit_1_2 = BitHelpers.GetBit_1_8(data[1], 3);
					if (!bit_1_ && !bit_1_2)
					{
						return "Charge Sustaining Mode (PSA) / Not a PHEV";
					}
					if (!bit_1_ && bit_1_2)
					{
						return "Charge Depleting Mode (PSA)";
					}
					if (bit_1_ && !bit_1_2)
					{
						return "Charge Increasing Mode (PSA)";
					}
					return "Not PSA";
				}
				else
				{
					if (BitHelpers.GetBit_1_8(data[1], 1))
					{
						return "Charge Depleting Mode";
					}
					return "Charge Sustaining Mode / Not a PHEV";
				}
			}, false)
			{
				Id = 789
			};
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x001FD148 File Offset: 0x001FB348
		public static PIDWithStringValue PID_019B_1()
		{
			return new PIDWithStringValue(PID.GetResourceString("PID_019B_1"), "019B", delegate(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (!(ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)))
				{
					return "";
				}
				int num = (data[0] >> 4) & 15;
				switch (num)
				{
				case 0:
					return "Urea Concentration Too High";
				case 1:
					return "Urea Concentration Too Low";
				case 2:
					return "Fluid is Diesel/Other";
				case 3:
					return "DEF is proper mixture";
				default:
					if (num == 13)
					{
						return "No Concentration Sensor fault, no results available/Not able to determine diesel exhaust fluid property (fluid type unknown)";
					}
					if (num != 14)
					{
						return "Unknown";
					}
					return "Concentration Sensor fault, no results available/Error with diesel exhaust fluid";
				}
			}, false)
			{
				Id = 713
			};
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x001FD194 File Offset: 0x001FB394
		// Note: this type is marked as 'beforefieldinit'.
		static PIDWithStringValue()
		{
		}

		// Token: 0x040018EC RID: 6380
		private int _IntValue;

		// Token: 0x040018ED RID: 6381
		private string _Value = "";

		// Token: 0x040018EE RID: 6382
		private Func<byte[], string> Formula;

		// Token: 0x040018EF RID: 6383
		private static Func<byte[], double> ABFormula = (byte[] data) => (double)((int)data[0] * 256 + (int)data[1]);

		// Token: 0x040018F0 RID: 6384
		private static Func<byte[], int, int, string> Time4BytesWithSupportedBit = delegate(byte[] data, int supported_bit, int start_byte)
		{
			if (supported_bit < 0 && supported_bit > 7)
			{
				return "";
			}
			bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
			if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], supported_bit + 1)) && start_byte + 4 <= data.Length)
			{
				return TimeSpan.FromSeconds(((int)data[start_byte] << 24) | ((int)data[start_byte + 1] << 16) | ((int)data[start_byte + 2] << 8) | (int)data[start_byte + 3]).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
			}
			return "n/a";
		};

		// Token: 0x040018F1 RID: 6385
		private bool UseOnlySelectedHeader;

		// Token: 0x040018F2 RID: 6386
		private static Func<byte[], string> ASCIITextFormula = (byte[] data) => Encoding.ASCII.GetString(data);

		// Token: 0x040018F3 RID: 6387
		private static Func<byte[], string> Mode09HexString = delegate(byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder(data.Length);
			foreach (byte b in data)
			{
				stringBuilder.Append(b.ToString("X2"));
			}
			return stringBuilder.ToString();
		};

		// Token: 0x040018F4 RID: 6388
		internal static Func<byte[], string> Mode09TextFormula = delegate(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream(data);
			StringBuilder stringBuilder2 = new StringBuilder();
			if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
			{
				while (memoryStream.Position < memoryStream.Length)
				{
					memoryStream.ReadByte();
					byte[] array = new byte[4];
					memoryStream.Read(array, 0, array.Length);
					foreach (byte b2 in array)
					{
						stringBuilder2.Append((char)b2);
					}
				}
			}
			else
			{
				int num = memoryStream.ReadByte();
				for (int k = 0; k < num; k++)
				{
					while (memoryStream.Position < memoryStream.Length)
					{
						int num2 = memoryStream.ReadByte();
						if (num2 < 0)
						{
							break;
						}
						stringBuilder2.Append((char)num2);
					}
					stringBuilder2.AppendLine();
				}
			}
			return stringBuilder2.ToString().Trim().Replace('ÿ', ' ')
				.Trim();
		};

		// Token: 0x040018F5 RID: 6389
		private const string vin_symbols = "0123456789ABCDEFGHJKLMNPRSTUVWXYZ";

		// Token: 0x040018F6 RID: 6390
		private const string eng_symbols = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ :,./;'~-=+_";

		// Token: 0x02000413 RID: 1043
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002CDA RID: 11482 RVA: 0x001FD20A File Offset: 0x001FB40A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002CDB RID: 11483 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002CDC RID: 11484 RVA: 0x001FD218 File Offset: 0x001FB418
			internal string <PID011F_RunTimeSinceEngineStart>b__15_0(byte[] data)
			{
				return TimeSpan.FromSeconds(PIDWithStringValue.ABFormula(data)).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
			}

			// Token: 0x06002CDD RID: 11485 RVA: 0x001FD248 File Offset: 0x001FB448
			internal string <PID014D_TimeRunWithMILon>b__16_0(byte[] data)
			{
				return TimeSpan.FromMinutes(PIDWithStringValue.ABFormula(data)).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
			}

			// Token: 0x06002CDE RID: 11486 RVA: 0x001FD278 File Offset: 0x001FB478
			internal string <PID014E_TimeSinceTroubleCodesCleared>b__17_0(byte[] data)
			{
				return TimeSpan.FromMinutes(PIDWithStringValue.ABFormula(data)).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
			}

			// Token: 0x06002CDF RID: 11487 RVA: 0x001FD2A8 File Offset: 0x001FB4A8
			internal string <PID011C_ObdStandard>b__18_0(byte[] data)
			{
				string[] array = new string[]
				{
					"Unknown", "OBD-II as defined by the CARB", "OBD as defined by the EPA", "OBD and OBD-II", "OBD-I", "Not OBD compliant", "EOBD (Europe)", "EOBD and OBD-II", "EOBD and OBD", "EOBD, OBD and OBD II",
					"JOBD (Japan)", "JOBD and OBD II", "JOBD and EOBD", "JOBD, EOBD, and OBD II", "Reserved", "Reserved", "Reserved", "Engine Manufacturer Diagnostics (EMD)", "Engine Manufacturer Diagnostics Enhanced (EMD+)", "Heavy Duty On-Board Diagnostics (Child/Partial) (HD OBD-C)",
					"Heavy Duty On-Board Diagnostics (HD OBD)", "World Wide Harmonized OBD (WWH OBD)", "Reserved", "Heavy Duty Euro OBD Stage I without NOx control (HD EOBD-I)", "Heavy Duty Euro OBD Stage I with NOx control (HD EOBD-I N)", "Heavy Duty Euro OBD Stage II without NOx control (HD EOBD-II)", "Heavy Duty Euro OBD Stage II with NOx control (HD EOBD-II N)", "Reserved", "Brazil OBD Phase 1 (OBDBr-1)", "Brazil OBD Phase 2 (OBDBr-2)",
					"Korean OBD (KOBD)", "India OBD I (IOBD I)", "India OBD II (IOBD II)", "Heavy Duty Euro OBD Stage VI (HD EOBD-IV)"
				};
				if ((int)data[0] < array.Length)
				{
					return array[(int)data[0]];
				}
				return array[0];
			}

			// Token: 0x06002CE0 RID: 11488 RVA: 0x001FD3F8 File Offset: 0x001FB5F8
			internal string <PID0112_CommandedSecondaryAirStatus>b__19_0(byte[] data)
			{
				byte b = data[0];
				switch (b)
				{
				case 1:
					return "Upstream";
				case 2:
					return "Downstream of catalytic converter";
				case 3:
					break;
				case 4:
					return "From the outside atmosphere or off";
				default:
					if (b == 8)
					{
						return "Pump commanded on for diagnostics";
					}
					break;
				}
				return "None";
			}

			// Token: 0x06002CE1 RID: 11489 RVA: 0x001FD444 File Offset: 0x001FB644
			internal string <PID011E_AuxillaryInputStatus>b__20_0(byte[] data)
			{
				if (data[0] == 1)
				{
					return "Power take off: active";
				}
				return "Power take off: not active";
			}

			// Token: 0x06002CE2 RID: 11490 RVA: 0x001FD458 File Offset: 0x001FB658
			internal string <PID0151_FuelType>b__21_0(byte[] data)
			{
				switch (data[0])
				{
				case 1:
					return "Gasoline";
				case 2:
					return "Methanol";
				case 3:
					return "Ethanol";
				case 4:
					return "Diesel";
				case 5:
					return "LPG";
				case 6:
					return "CNG";
				case 7:
					return "Propane";
				case 8:
					return "Electric";
				case 9:
					return "Bifuel\u00a0running Gasoline";
				case 10:
					return "Bifuel running Methanol";
				case 11:
					return "Bifuel running Ethanol";
				case 12:
					return "Bifuel running LPG";
				case 13:
					return "Bifuel running CNG";
				case 14:
					return "Bifuel running Propane";
				case 15:
					return "Bifuel running Electricity";
				case 16:
					return "Bifuel running electric and combustion engine";
				case 17:
					return "Hybrid gasoline";
				case 18:
					return "Hybrid Ethanol";
				case 19:
					return "Hybrid Diesel";
				case 20:
					return "Hybrid Electric";
				case 21:
					return "Hybrid running electric and combustion engine";
				case 22:
					return "Hybrid Regenerative";
				case 23:
					return "Bifuel running diesel";
				}
				return "Not available";
			}

			// Token: 0x06002CE3 RID: 11491 RVA: 0x001FD564 File Offset: 0x001FB764
			internal string <PID0102_FreezFrameDTC>b__22_0(byte[] data)
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && data != null && data.Length > 2)
				{
					data = new byte[]
					{
						data[0],
						data[1]
					};
				}
				return new DTCItemV2(data, "", "", "03", DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC, "").Code;
			}

			// Token: 0x06002CE4 RID: 11492 RVA: 0x001FD5BB File Offset: 0x001FB7BB
			internal bool <FilterVin>b__27_0(char c)
			{
				return "0123456789ABCDEFGHJKLMNPRSTUVWXYZ".Contains(c);
			}

			// Token: 0x06002CE5 RID: 11493 RVA: 0x001FD5C8 File Offset: 0x001FB7C8
			internal bool <FilterECUName>b__29_0(char c)
			{
				return "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ :,./;'~-=+_".Contains(c);
			}

			// Token: 0x06002CE6 RID: 11494 RVA: 0x001FD5D5 File Offset: 0x001FB7D5
			internal string <PID0902_VIN>b__30_0(byte[] arg)
			{
				return PIDWithStringValue.FilterVin(PIDWithStringValue.Mode09TextFormula(arg));
			}

			// Token: 0x06002CE7 RID: 11495 RVA: 0x001FD5E7 File Offset: 0x001FB7E7
			internal string <PID0904_CalibrationID>b__31_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.Mode09TextFormula(arg));
			}

			// Token: 0x06002CE8 RID: 11496 RVA: 0x001FD5F9 File Offset: 0x001FB7F9
			internal string <PID0906_CalibrationVerificationNumber>b__32_0(byte[] arg)
			{
				return PIDWithStringValue.Mode09HexString(arg);
			}

			// Token: 0x06002CE9 RID: 11497 RVA: 0x001FD5E7 File Offset: 0x001FB7E7
			internal string <PID090A_EcuName>b__33_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.Mode09TextFormula(arg));
			}

			// Token: 0x06002CEA RID: 11498 RVA: 0x001FD608 File Offset: 0x001FB808
			internal string <PID017F_TotalEngineRunTime>b__34_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)) && data.Length >= 5)
				{
					byte[] array = new byte[]
					{
						data[1],
						data[2],
						data[3],
						data[4]
					};
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					return TimeSpan.FromSeconds((double)((ulong)BitConverter.ToUInt32(array, 0))).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
				}
				return "n/a";
			}

			// Token: 0x06002CEB RID: 11499 RVA: 0x001FD68C File Offset: 0x001FB88C
			internal string <PID017F_TotalIdleRunTime>b__35_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 2))
				{
					byte[] array = new byte[]
					{
						data[5],
						data[6],
						data[7],
						data[8]
					};
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					return TimeSpan.FromSeconds((double)((ulong)BitConverter.ToUInt32(array, 0))).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
				}
				return "";
			}

			// Token: 0x06002CEC RID: 11500 RVA: 0x001FD708 File Offset: 0x001FB908
			internal string <PID017F_TotalRunTimeWithPTOActive>b__36_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 3))
				{
					byte[] array = new byte[]
					{
						data[9],
						data[10],
						data[11],
						data[12]
					};
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					return TimeSpan.FromSeconds((double)((ulong)BitConverter.ToUInt32(array, 0))).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
				}
				return "";
			}

			// Token: 0x06002CED RID: 11501 RVA: 0x001FD787 File Offset: 0x001FB987
			internal string <PID1A90_KWP_VIN>b__37_0(byte[] arg)
			{
				return PIDWithStringValue.FilterVin(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CEE RID: 11502 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A91_KWP_VehicleManufacturerECUHardwareNumber>b__38_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CEF RID: 11503 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A92_KWP_SystemSupplierECUHardwareNumber>b__39_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CF0 RID: 11504 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A93_KWP_SystemSupplierECUHardwareVersion>b__40_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CF1 RID: 11505 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A94_KWP_SystemSupplierECUSoftwareNumber>b__41_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CF2 RID: 11506 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A95_KWP_SystemSupplierECUSoftwareVersion>b__42_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CF3 RID: 11507 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A96_KWP_ExhaustRegulationOrTypeApprovalNumber>b__43_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CF4 RID: 11508 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A97_KWP_SystemNameOrEngineType>b__44_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CF5 RID: 11509 RVA: 0x001FD799 File Offset: 0x001FB999
			internal string <PID1A98_KWP_RepairShopCodeOrTesterSerialNumber>b__45_0(byte[] arg)
			{
				return PIDWithStringValue.FilterECUName(PIDWithStringValue.ASCIITextFormula(arg));
			}

			// Token: 0x06002CF6 RID: 11510 RVA: 0x001FD7AC File Offset: 0x001FB9AC
			internal string <PID1A99_KWP_ProgrammingDate>b__46_0(byte[] arg)
			{
				if (arg.Length < 4)
				{
					return "";
				}
				string text = arg[0].ToString("X2") + arg[1].ToString("X2");
				string text2 = arg[2].ToString("X2");
				string text3 = arg[3].ToString("X2");
				return string.Concat(new string[] { text, ".", text2, ".", text3 });
			}

			// Token: 0x06002CF7 RID: 11511 RVA: 0x001FD838 File Offset: 0x001FBA38
			internal string <PID_0185_4>b__49_0(byte[] data)
			{
				return PIDWithStringValue.Time4BytesWithSupportedBit(data, 3, 6);
			}

			// Token: 0x06002CF8 RID: 11512 RVA: 0x001FD848 File Offset: 0x001FBA48
			internal string <PID_0188_1>b__50_0(byte[] data)
			{
				if (data.Length <= 6)
				{
					return "";
				}
				byte b = data[0];
				StringBuilder stringBuilder = new StringBuilder();
				if (BitHelpers.GetBit_1_8(b, 8))
				{
					stringBuilder.Append("Active");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				else
				{
					stringBuilder.Append("Disabled");
				}
				b = data[1];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 10K history (0 - 10,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				b = data[3];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 20K history (10,000 - 20,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				b = data[4];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 30K history (20,000  - 30,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				b = data[5];
				if ((b & 15) != 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append("SCR inducement system state 40K history (30,000 - 40,000 km): ");
					if (BitHelpers.GetBit_1_8(b, 1))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- reagent level too low");
					}
					if (BitHelpers.GetBit_1_8(b, 2))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- incorrect reagent");
					}
					if (BitHelpers.GetBit_1_8(b, 3))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- deviation of reagent consumption");
					}
					if (BitHelpers.GetBit_1_8(b, 4))
					{
						stringBuilder.Append("\n");
						stringBuilder.Append("- NOx emissions too high");
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06002CF9 RID: 11513 RVA: 0x001FDBC0 File Offset: 0x001FBDC0
			internal string <PID_0190_1>b__53_0(byte[] data)
			{
				int num = (data[0] >> 2) & 15;
				switch (num)
				{
				case 0:
					return "MI Off";
				case 1:
					return "On Demand MI";
				case 2:
					return "Short MI";
				case 3:
					return "Continuous MI";
				default:
					if (num == 14)
					{
						return "Error";
					}
					if (num != 15)
					{
						return "Unknown";
					}
					return "Not available/Not required of this vehicle";
				}
			}

			// Token: 0x06002CFA RID: 11514 RVA: 0x001FDC24 File Offset: 0x001FBE24
			internal string <PID_0191_1>b__54_0(byte[] data)
			{
				int num = (int)(data[0] & 15);
				switch (num)
				{
				case 0:
					return "MI Off";
				case 1:
					return "On Demand MI";
				case 2:
					return "Short MI";
				case 3:
					return "Continuous MI";
				default:
					if (num == 14)
					{
						return "Error";
					}
					if (num != 15)
					{
						return "Unknown";
					}
					return "Not available/Not required of this vehicle";
				}
			}

			// Token: 0x06002CFB RID: 11515 RVA: 0x001FDC84 File Offset: 0x001FBE84
			internal string <PID_019A_1>b__55_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (!(ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : (BitHelpers.GetBit_1_8(data[0], 1) || BitHelpers.GetBit_1_8(data[0], 4))))
				{
					return "";
				}
				if (BitHelpers.GetBit_1_8(data[0], 4))
				{
					bool bit_1_ = BitHelpers.GetBit_1_8(data[1], 2);
					bool bit_1_2 = BitHelpers.GetBit_1_8(data[1], 3);
					if (!bit_1_ && !bit_1_2)
					{
						return "Charge Sustaining Mode (PSA) / Not a PHEV";
					}
					if (!bit_1_ && bit_1_2)
					{
						return "Charge Depleting Mode (PSA)";
					}
					if (bit_1_ && !bit_1_2)
					{
						return "Charge Increasing Mode (PSA)";
					}
					return "Not PSA";
				}
				else
				{
					if (BitHelpers.GetBit_1_8(data[1], 1))
					{
						return "Charge Depleting Mode";
					}
					return "Charge Sustaining Mode / Not a PHEV";
				}
			}

			// Token: 0x06002CFC RID: 11516 RVA: 0x001FDD20 File Offset: 0x001FBF20
			internal string <PID_019B_1>b__56_0(byte[] data)
			{
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if (!(ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], 1)))
				{
					return "";
				}
				int num = (data[0] >> 4) & 15;
				switch (num)
				{
				case 0:
					return "Urea Concentration Too High";
				case 1:
					return "Urea Concentration Too Low";
				case 2:
					return "Fluid is Diesel/Other";
				case 3:
					return "DEF is proper mixture";
				default:
					if (num == 13)
					{
						return "No Concentration Sensor fault, no results available/Not able to determine diesel exhaust fluid property (fluid type unknown)";
					}
					if (num != 14)
					{
						return "Unknown";
					}
					return "Concentration Sensor fault, no results available/Error with diesel exhaust fluid";
				}
			}

			// Token: 0x06002CFD RID: 11517 RVA: 0x001FC223 File Offset: 0x001FA423
			internal double <.cctor>b__57_0(byte[] data)
			{
				return (double)((int)data[0] * 256 + (int)data[1]);
			}

			// Token: 0x06002CFE RID: 11518 RVA: 0x001FDDA4 File Offset: 0x001FBFA4
			internal string <.cctor>b__57_1(byte[] data, int supported_bit, int start_byte)
			{
				if (supported_bit < 0 && supported_bit > 7)
				{
					return "";
				}
				bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
				if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], supported_bit + 1)) && start_byte + 4 <= data.Length)
				{
					return TimeSpan.FromSeconds(((int)data[start_byte] << 24) | ((int)data[start_byte + 1] << 16) | ((int)data[start_byte + 2] << 8) | (int)data[start_byte + 3]).ToString("d\\.hh\\:mm\\:ss", CultureInfo.InvariantCulture);
				}
				return "n/a";
			}

			// Token: 0x06002CFF RID: 11519 RVA: 0x001FDE21 File Offset: 0x001FC021
			internal string <.cctor>b__57_2(byte[] data)
			{
				return Encoding.ASCII.GetString(data);
			}

			// Token: 0x06002D00 RID: 11520 RVA: 0x001FDE30 File Offset: 0x001FC030
			internal string <.cctor>b__57_3(byte[] data)
			{
				StringBuilder stringBuilder = new StringBuilder(data.Length);
				foreach (byte b in data)
				{
					stringBuilder.Append(b.ToString("X2"));
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06002D01 RID: 11521 RVA: 0x001FDE74 File Offset: 0x001FC074
			internal string <.cctor>b__57_4(byte[] data)
			{
				MemoryStream memoryStream = new MemoryStream(data);
				StringBuilder stringBuilder = new StringBuilder();
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					while (memoryStream.Position < memoryStream.Length)
					{
						memoryStream.ReadByte();
						byte[] array = new byte[4];
						memoryStream.Read(array, 0, array.Length);
						foreach (byte b in array)
						{
							stringBuilder.Append((char)b);
						}
					}
				}
				else
				{
					int num = memoryStream.ReadByte();
					for (int j = 0; j < num; j++)
					{
						while (memoryStream.Position < memoryStream.Length)
						{
							int num2 = memoryStream.ReadByte();
							if (num2 < 0)
							{
								break;
							}
							stringBuilder.Append((char)num2);
						}
						stringBuilder.AppendLine();
					}
				}
				return stringBuilder.ToString().Trim().Replace('ÿ', ' ')
					.Trim();
			}

			// Token: 0x040018F7 RID: 6391
			public static readonly PIDWithStringValue.<>c <>9 = new PIDWithStringValue.<>c();

			// Token: 0x040018F8 RID: 6392
			public static Func<byte[], string> <>9__15_0;

			// Token: 0x040018F9 RID: 6393
			public static Func<byte[], string> <>9__16_0;

			// Token: 0x040018FA RID: 6394
			public static Func<byte[], string> <>9__17_0;

			// Token: 0x040018FB RID: 6395
			public static Func<byte[], string> <>9__18_0;

			// Token: 0x040018FC RID: 6396
			public static Func<byte[], string> <>9__19_0;

			// Token: 0x040018FD RID: 6397
			public static Func<byte[], string> <>9__20_0;

			// Token: 0x040018FE RID: 6398
			public static Func<byte[], string> <>9__21_0;

			// Token: 0x040018FF RID: 6399
			public static Func<byte[], string> <>9__22_0;

			// Token: 0x04001900 RID: 6400
			public static Func<char, bool> <>9__27_0;

			// Token: 0x04001901 RID: 6401
			public static Func<char, bool> <>9__29_0;

			// Token: 0x04001902 RID: 6402
			public static Func<byte[], string> <>9__30_0;

			// Token: 0x04001903 RID: 6403
			public static Func<byte[], string> <>9__31_0;

			// Token: 0x04001904 RID: 6404
			public static Func<byte[], string> <>9__32_0;

			// Token: 0x04001905 RID: 6405
			public static Func<byte[], string> <>9__33_0;

			// Token: 0x04001906 RID: 6406
			public static Func<byte[], string> <>9__34_0;

			// Token: 0x04001907 RID: 6407
			public static Func<byte[], string> <>9__35_0;

			// Token: 0x04001908 RID: 6408
			public static Func<byte[], string> <>9__36_0;

			// Token: 0x04001909 RID: 6409
			public static Func<byte[], string> <>9__37_0;

			// Token: 0x0400190A RID: 6410
			public static Func<byte[], string> <>9__38_0;

			// Token: 0x0400190B RID: 6411
			public static Func<byte[], string> <>9__39_0;

			// Token: 0x0400190C RID: 6412
			public static Func<byte[], string> <>9__40_0;

			// Token: 0x0400190D RID: 6413
			public static Func<byte[], string> <>9__41_0;

			// Token: 0x0400190E RID: 6414
			public static Func<byte[], string> <>9__42_0;

			// Token: 0x0400190F RID: 6415
			public static Func<byte[], string> <>9__43_0;

			// Token: 0x04001910 RID: 6416
			public static Func<byte[], string> <>9__44_0;

			// Token: 0x04001911 RID: 6417
			public static Func<byte[], string> <>9__45_0;

			// Token: 0x04001912 RID: 6418
			public static Func<byte[], string> <>9__46_0;

			// Token: 0x04001913 RID: 6419
			public static Func<byte[], string> <>9__49_0;

			// Token: 0x04001914 RID: 6420
			public static Func<byte[], string> <>9__50_0;

			// Token: 0x04001915 RID: 6421
			public static Func<byte[], string> <>9__53_0;

			// Token: 0x04001916 RID: 6422
			public static Func<byte[], string> <>9__54_0;

			// Token: 0x04001917 RID: 6423
			public static Func<byte[], string> <>9__55_0;

			// Token: 0x04001918 RID: 6424
			public static Func<byte[], string> <>9__56_0;
		}

		// Token: 0x02000414 RID: 1044
		[CompilerGenerated]
		private sealed class <>c__DisplayClass47_0
		{
			// Token: 0x06002D02 RID: 11522 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass47_0()
			{
			}

			// Token: 0x06002D03 RID: 11523 RVA: 0x001FDF4B File Offset: 0x001FC14B
			internal string <GetPIDs_0181>b__0(byte[] data)
			{
				return PIDWithStringValue.Time4BytesWithSupportedBit(data, this.sup_bit, this.start_byte);
			}

			// Token: 0x04001919 RID: 6425
			public int sup_bit;

			// Token: 0x0400191A RID: 6426
			public int start_byte;
		}

		// Token: 0x02000415 RID: 1045
		[CompilerGenerated]
		private sealed class <>c__DisplayClass48_0
		{
			// Token: 0x06002D04 RID: 11524 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass48_0()
			{
			}

			// Token: 0x06002D05 RID: 11525 RVA: 0x001FDF64 File Offset: 0x001FC164
			internal string <GetPIDs_0182>b__0(byte[] data)
			{
				return PIDWithStringValue.Time4BytesWithSupportedBit(data, this.sup_bit, this.start_byte);
			}

			// Token: 0x0400191B RID: 6427
			public int sup_bit;

			// Token: 0x0400191C RID: 6428
			public int start_byte;
		}

		// Token: 0x02000416 RID: 1046
		[CompilerGenerated]
		private sealed class <>c__DisplayClass51_0
		{
			// Token: 0x06002D06 RID: 11526 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass51_0()
			{
			}

			// Token: 0x06002D07 RID: 11527 RVA: 0x001FDF7D File Offset: 0x001FC17D
			internal string <GetPIDs_0189>b__0(byte[] data)
			{
				return PIDWithStringValue.Time4BytesWithSupportedBit(data, this.sup_bit, this.start_byte);
			}

			// Token: 0x0400191D RID: 6429
			public int sup_bit;

			// Token: 0x0400191E RID: 6430
			public int start_byte;
		}

		// Token: 0x02000417 RID: 1047
		[CompilerGenerated]
		private sealed class <>c__DisplayClass52_0
		{
			// Token: 0x06002D08 RID: 11528 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass52_0()
			{
			}

			// Token: 0x06002D09 RID: 11529 RVA: 0x001FDF96 File Offset: 0x001FC196
			internal string <GetPIDs_018A>b__0(byte[] data)
			{
				return PIDWithStringValue.Time4BytesWithSupportedBit(data, this.sup_bit, this.start_byte);
			}

			// Token: 0x0400191F RID: 6431
			public int sup_bit;

			// Token: 0x04001920 RID: 6432
			public int start_byte;
		}
	}
}
