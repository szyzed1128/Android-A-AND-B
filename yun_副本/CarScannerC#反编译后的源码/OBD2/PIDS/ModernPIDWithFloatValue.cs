using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003FF RID: 1023
	public class ModernPIDWithFloatValue : PIDWithFloatValueFormula
	{
		// Token: 0x0600290B RID: 10507 RVA: 0x001ED4C8 File Offset: 0x001EB6C8
		public ModernPIDWithFloatValue(string command, UnitsHelper.Units units, int idx, int id, int bit_supported, int start_byte, int data_length, Func<byte[], double> formula)
			: base(PID.GetResourceString("PID_" + command + "_" + (idx + 1).ToString(CultureInfo.InvariantCulture.NumberFormat)), command, formula, units)
		{
			base.Id = id;
			this.bit_supported = bit_supported;
			this.start_byte = start_byte;
			this.data_length = data_length;
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x001ED52C File Offset: 0x001EB72C
		public ModernPIDWithFloatValue(string command, UnitsHelper.Units units, int idx, int id, int bit_supported, string start_byte_letter, int data_length, Func<byte[], double> formula)
			: this(command, units, idx, id, bit_supported, CustomPID.GetByteNumberFromLetter(start_byte_letter), data_length, formula)
		{
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x001ED554 File Offset: 0x001EB754
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			if (data.Length == 0)
			{
				base.Value = double.NaN;
				return;
			}
			bool ignoreSupportedFlagForPIDs0166_ = SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183;
			if ((ignoreSupportedFlagForPIDs0166_ ? ignoreSupportedFlagForPIDs0166_ : BitHelpers.GetBit_1_8(data[0], this.bit_supported + 1)) && this.start_byte + this.data_length <= data.Length)
			{
				base.TimeStamp = timeStamp;
				base.Value = this.Formula(data);
				return;
			}
			base.Value = double.NaN;
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x001ED5D3 File Offset: 0x001EB7D3
		public override Func<byte[], double> GetFormula()
		{
			if (SharedSettings.Current.IgnoreSupportedFlagForPIDs0166_0183)
			{
				return base.GetFormula();
			}
			return delegate(byte[] data)
			{
				if (data.Length == 0)
				{
					return double.NaN;
				}
				if (BitHelpers.GetBit_1_8(data[0], this.bit_supported + 1) && this.start_byte + this.data_length <= data.Length)
				{
					return this.Formula(data);
				}
				return double.NaN;
			};
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x001ED5F4 File Offset: 0x001EB7F4
		public static PID[] GetPID0185()
		{
			string text = "0185";
			PID[] array = new PID[4];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Lh, 0, 714, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.005);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Lh, 1, 715, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.005);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 2, 716, 2, 5, 1, (byte[] data) => (double)(data[5] * 100 / byte.MaxValue));
			array[3] = PIDWithStringValue.PID_0185_4();
			return array;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x001ED6B0 File Offset: 0x001EB8B0
		public static PID[] GetPID0186()
		{
			string text = "0186";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.mg_m3, 0, 717, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.005);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.mg_m3, 1, 718, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.005);
			return array;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x001ED730 File Offset: 0x001EB930
		public static PID[] GetPID0187()
		{
			string text = "0187";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 0, 719, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.03125);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 1, 720, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.03125);
			return array;
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x001ED7B0 File Offset: 0x001EB9B0
		public static PID[] GetPID0188()
		{
			PID[] array = new PID[5];
			array[0] = new PIDWithFloatValueFormula(PID.GetResourceString("PID_0188_2"), "0188", (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]), UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 65535.0,
				Id = 721
			};
			array[1] = new PIDWithFloatValueFormula(PID.GetResourceString("PID_0188_3"), "0188", (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]), UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 65535.0,
				Id = 722
			};
			array[2] = new PIDWithFloatValueFormula(PID.GetResourceString("PID_0188_4"), "0188", (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]), UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 65535.0,
				Id = 723
			};
			array[3] = new PIDWithFloatValueFormula(PID.GetResourceString("PID_0188_5"), "0188", (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]), UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 65535.0,
				Id = 724
			};
			array[4] = new PIDWithFloatValueFormula(PID.GetResourceString("PID_0188_6"), "0188", (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]), UnitsHelper.Units.km)
			{
				Minimum = 0.0,
				Maximum = 65535.0,
				Id = 725
			};
			return array;
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x001ED9A4 File Offset: 0x001EBBA4
		public static PID[] GetPID018B()
		{
			string text = "018B";
			PID[] array = new PID[6];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 0, 726, 0, 1, 1, delegate(byte[] data)
			{
				if (!BitHelpers.GetBit_1_8(data[1], 1))
				{
					return 0.0;
				}
				if (BitHelpers.GetBit_1_8(data[1], 2))
				{
					return 2.0;
				}
				return 1.0;
			})
			{
				ShortName = Translate.GetString("PID_018B_1_Short")
			};
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 2, 728, 2, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 3) > false);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 3, 729, 3, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 4) > false);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 4, 730, 4, 2, 1, (byte[] data) => (double)(data[2] * 100 / byte.MaxValue));
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.minutes, 5, 731, 5, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]));
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.km, 6, 732, 6, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]));
			return array;
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x001EDAFC File Offset: 0x001EBCFC
		public static PID[] GetPID018C()
		{
			string text = "018C";
			PID[] array = new PID[8];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 0, 733, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.001526);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 1, 734, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.001526);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 2, 735, 2, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]) * 0.001526);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 3, 736, 3, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]) * 0.001526);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 4, 737, 4, 9, 2, (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]) * 0.000122);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 5, 738, 5, 11, 2, (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]) * 0.000122);
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 6, 739, 6, 13, 2, (byte[] data) => (double)((int)data[13] * 256 + (int)data[14]) * 0.000122);
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 7, 740, 7, 15, 2, (byte[] data) => (double)((int)data[15] * 256 + (int)data[16]) * 0.000122);
			return array;
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x001EDCB0 File Offset: 0x001EBEB0
		public static PID[] GetPID018F()
		{
			string text = "018F";
			PID[] array = new PID[6];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 0, 741, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 1) > false);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 1, 742, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 2) > false);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.minutes, 2, 743, 1, 2, 2, (byte[] data) => (double)((short)((int)data[2] * 256 + (int)data[3])) * 0.01);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 3, 744, 2, 4, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[4], 1) > false);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 4, 745, 2, 4, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[4], 2) > false);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.minutes, 5, 746, 3, 5, 2, (byte[] data) => (double)((short)((int)data[5] * 256 + (int)data[6])) * 0.01);
			return array;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x001EDDF8 File Offset: 0x001EBFF8
		public static PID[] GetPID0192()
		{
			string text = "0192";
			PID[] array = new PID[8];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 0, 747, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 1) > false);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 1, 748, 1, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 2) > false);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 2, 749, 2, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 3) > false);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 3, 750, 3, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 4) > false);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 4, 751, 4, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 5) > false);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 5, 752, 5, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 6) > false);
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 6, 753, 6, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 7) > false);
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 7, 754, 7, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 8) > false);
			return array;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x001EDFA4 File Offset: 0x001EC1A4
		public static PID GetPID0193()
		{
			return new ModernPIDWithFloatValue("0193", UnitsHelper.Units.Hours, 0, 755, 0, 1, 2, (byte[] data) => (double)((short)((int)data[1] * 256 + (int)data[2])));
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x001EDFE8 File Offset: 0x001EC1E8
		public static PID[] GetPID0194()
		{
			string text = "0194";
			PID[] array = new PID[6];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 0, 756, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_1_8(data[1], 1) > false);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Hours, 4, 757, 3, 2, 2, (byte[] data) => (double)((int)data[2] * 256 + (int)data[3]));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Hours, 5, 758, 3, 4, 2, (byte[] data) => (double)((int)data[4] * 256 + (int)data[5]));
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Hours, 6, 759, 3, 6, 2, (byte[] data) => (double)((int)data[6] * 256 + (int)data[7]));
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Hours, 7, 760, 3, 8, 2, (byte[] data) => (double)((int)data[8] * 256 + (int)data[9]));
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Hours, 8, 761, 3, 10, 2, (byte[] data) => (double)((int)data[10] * 256 + (int)data[11]));
			return array;
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x001EE134 File Offset: 0x001EC334
		public static PID[] GetPID0198()
		{
			string text = "0198";
			PID[] array = new PID[4];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 0, 762, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 1, 763, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 2, 764, 2, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 3, 765, 3, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0);
			return array;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x001EE21C File Offset: 0x001EC41C
		public static PID[] GetPID0199()
		{
			string text = "0199";
			PID[] array = new PID[4];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 0, 766, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 1, 767, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 2, 768, 2, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 3, 769, 3, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0);
			return array;
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x001EE304 File Offset: 0x001EC504
		public static PID[] GetPID019A_2_3()
		{
			string text = "019A";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 1, 790, 1, 2, 2, (byte[] data) => (double)((int)data[2] * 256 + (int)data[3]) * 0.015625)
			{
				Role = Roles.EV_BatteryVoltage
			};
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 2, 791, 2, 4, 2, (byte[] data) => (double)((short)((int)data[4] * 256 + (int)data[5])) * 0.1)
			{
				Role = Roles.EV_BatteryCurrent
			};
			return array;
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x001EE394 File Offset: 0x001EC594
		public static PID[] GetPID019B()
		{
			string text = "019B";
			PID[] array = new PID[3];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 1, 770, 1, 1, 1, (byte[] data) => (double)data[1] * 0.25);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 2, 771, 2, 2, 1, (byte[] data) => (double)(data[2] - 40));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 3, 772, 3, 3, 1, (byte[] data) => (double)(data[3] * 100 / byte.MaxValue));
			return array;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x001EE448 File Offset: 0x001EC648
		public static PID[] GetPID019C()
		{
			string text = "019C";
			PID[] array = new PID[8];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 0, 773, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.001526);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 1, 774, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.001526);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 2, 775, 2, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]) * 0.001526);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 3, 776, 3, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]) * 0.001526);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 4, 777, 4, 9, 2, (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]) * 0.000122);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 5, 778, 5, 11, 2, (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]) * 0.000122);
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 6, 779, 6, 13, 2, (byte[] data) => (double)((int)data[13] * 256 + (int)data[14]) * 0.000122);
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 7, 780, 7, 15, 2, (byte[] data) => (double)((int)data[15] * 256 + (int)data[16]) * 0.000122);
			return array;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x001EE5FC File Offset: 0x001EC7FC
		public static PID[] GetPID019F()
		{
			string text = "019F";
			PID[] array = new PID[8];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 0, 781, 0, 1, 1, (byte[] data) => (double)(data[1] * 100 / byte.MaxValue));
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 1, 782, 1, 2, 1, (byte[] data) => (double)(data[2] * 100 / byte.MaxValue));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 2, 783, 2, 3, 1, (byte[] data) => (double)(data[3] * 100 / byte.MaxValue));
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 3, 784, 3, 4, 1, (byte[] data) => (double)(data[4] * 100 / byte.MaxValue));
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 4, 785, 4, 5, 1, (byte[] data) => (double)(data[5] * 100 / byte.MaxValue));
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 5, 786, 5, 6, 1, (byte[] data) => (double)(data[6] * 100 / byte.MaxValue));
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 6, 787, 6, 7, 1, (byte[] data) => (double)(data[7] * 100 / byte.MaxValue));
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 7, 788, 7, 8, 1, (byte[] data) => (double)(data[8] * 100 / byte.MaxValue));
			return array;
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x001EE7B0 File Offset: 0x001EC9B0
		public static PID[] GetPID01A1()
		{
			string text = "01A1";
			PID[] array = new PID[4];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 0, 792, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]));
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 1, 793, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 2, 794, 2, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]));
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 3, 795, 3, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]));
			return array;
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x001EE898 File Offset: 0x001ECA98
		public static PID[] GetPID01A5()
		{
			string text = "01A5";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 0, 798, 0, 1, 1, (byte[] data) => (double)data[1] * 0.5);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.liters, 1, 799, 1, 2, 2, (byte[] data) => (double)((int)data[2] * 256 + (int)data[3]) * 0.0005);
			return array;
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x001EE918 File Offset: 0x001ECB18
		public static PID[] GetPID01A7()
		{
			string text = "01A7";
			PID[] array = new PID[4];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 0, 556, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]));
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 1, 557, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 2, 558, 2, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]));
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 3, 559, 3, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]));
			return array;
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x001EEA00 File Offset: 0x001ECC00
		public static PID[] GetPID01A8()
		{
			string text = "01A8";
			PID[] array = new PID[4];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 0, 560, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]));
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 1, 561, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 2, 562, 2, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]));
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.ppm, 3, 563, 3, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]));
			return array;
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x001EEAE8 File Offset: 0x001ECCE8
		public static PID[] GetPID01A9()
		{
			string text = "01A9";
			PID[] array = new PID[1];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 0, 564, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_0_7(data[1], 0) > false);
			return array;
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x001EEB34 File Offset: 0x001ECD34
		public static PID[] GetPID01AB()
		{
			string text = "01AB";
			PID[] array = new PID[5];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 0, 566, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.03125);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 1, 567, 1, 3, 1, (byte[] data) => (double)(data[3] - 40));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 2, 568, 2, 4, 2, (byte[] data) => (double)((int)data[4] * 256 + (int)data[5]) * 0.125);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 3, 569, 3, 6, 2, (byte[] data) => (double)((int)data[6] * 256 + (int)data[7]));
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 4, 570, 4, 8, 1, (byte[] data) => (double)((int)(data[8] * 2) - 256));
			return array;
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x001EEC4C File Offset: 0x001ECE4C
		public static PID[] GetPID01AC()
		{
			string text = "01AC";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 0, 571, 0, 1, 1, (byte[] data) => (double)data[1]);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.grams_hour, 1, 572, 1, 2, 2, (byte[] data) => (double)((int)data[2] * 256 + (int)data[3]) * 0.3);
			return array;
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x001EECCC File Offset: 0x001ECECC
		public static PID[] GetPID01AD()
		{
			string text = "01AD";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Pa, 0, 573, 0, 1, 2, (byte[] data) => PIDWithFloatValueFormula.Signed16Bit(data[1], data[2]) * 0.25);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.rpm, 1, 574, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]));
			return array;
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x001EED4C File Offset: 0x001ECF4C
		public static PID[] GetPID01AE()
		{
			string text = "01AE";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Pa, 0, 575, 0, 1, 2, (byte[] data) => PIDWithFloatValueFormula.Signed16Bit(data[1], data[2]) * 0.25);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.Pa, 1, 576, 1, 3, 2, (byte[] data) => PIDWithFloatValueFormula.Signed16Bit(data[3], data[4]) * 2.0);
			return array;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x001EEDCC File Offset: 0x001ECFCC
		public static PID[] GetPID01B0()
		{
			string text = "01B0";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kg_h, 0, 578, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.05);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kg_h, 1, 579, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.05);
			return array;
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x001EEE4C File Offset: 0x001ED04C
		public static PID[] GetPID01B1()
		{
			string text = "01B1";
			PID[] array = new PID[10];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 0, 580, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_0_7(data[1], 0) > false);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 1, 581, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_0_7(data[1], 1) > false);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 2, 582, 0, 1, 1, (byte[] data) => BitHelpers.GetBit_0_7(data[1], 2) > false);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 3, 583, 1, 1, 1, (byte[] data) => BitHelpers.GetBit_0_7(data[1], 4) > false);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 4, 584, 1, 1, 1, (byte[] data) => BitHelpers.GetBit_0_7(data[1], 5) > false);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.None, 5, 585, 1, 1, 1, (byte[] data) => BitHelpers.GetBit_0_7(data[1], 6) > false);
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 6, 586, 2, 2, 1, (byte[] data) => (double)data[2] * 100.0 / 128.0 - 100.0);
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 7, 587, 3, 3, 1, (byte[] data) => (double)data[3] * 100.0 / 128.0 - 100.0);
			array[8] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 8, 588, 4, 4, 1, (byte[] data) => (double)data[4] * 100.0 / 128.0 - 100.0);
			array[9] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 9, 589, 5, 5, 1, (byte[] data) => (double)data[5] * 100.0 / 128.0 - 100.0);
			return array;
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x001EF060 File Offset: 0x001ED260
		public static PID[] GetPID01B3()
		{
			string text = "01B3";
			PID[] array = new PID[9];
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.05, UnitsHelper.Units.kW)
			{
				Minimum = 0.0,
				Maximum = 3000.0,
				Name = PID.GetResourceString("PID_01B3_0"),
				ShortName = PID.GetResourceString("PID_01B3_0"),
				Id = 591
			};
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 0, 592, 0, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.05);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 1, 593, 1, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]) * 0.05);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 2, 594, 2, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]) * 0.05);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 3, 595, 3, 9, 2, (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]) * 0.05);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 4, 596, 4, 11, 2, (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]) * 0.05);
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 5, 597, 5, 13, 2, (byte[] data) => (double)((int)data[13] * 256 + (int)data[14]) * 0.05);
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 6, 598, 6, 15, 2, (byte[] data) => (double)((int)data[15] * 256 + (int)data[16]) * 0.05);
			array[8] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kW, 7, 599, 7, 17, 2, (byte[] data) => (double)((int)data[17] * 256 + (int)data[18]) * 0.05);
			return array;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x001EF28C File Offset: 0x001ED48C
		public static PID[] GetPID01B4()
		{
			string text = "01B4";
			PID[] array = new PID[9];
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)(data[1] - 40), UnitsHelper.Units.celicium)
			{
				Minimum = -40.0,
				Maximum = 200.0,
				Name = PID.GetResourceString("PID_01B4_0"),
				ShortName = PID.GetResourceString("PID_01B4_0"),
				Id = 600
			};
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 0, 601, 0, 2, 1, (byte[] data) => (double)(data[2] - 40));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 1, 602, 1, 3, 1, (byte[] data) => (double)(data[3] - 40));
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 2, 603, 3, 4, 1, (byte[] data) => (double)(data[4] - 40));
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 3, 604, 4, 5, 1, (byte[] data) => (double)(data[5] - 40));
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 4, 605, 4, 6, 1, (byte[] data) => (double)(data[6] - 40));
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 5, 606, 5, 7, 1, (byte[] data) => (double)(data[7] - 40));
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 6, 607, 6, 8, 1, (byte[] data) => (double)(data[8] - 40));
			array[8] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 7, 608, 6, 9, 1, (byte[] data) => (double)(data[9] - 40));
			return array;
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x001EF4B4 File Offset: 0x001ED6B4
		public static PID[] GetPID01B5()
		{
			string text = "01B5";
			PID[] array = new PID[9];
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.05 - 1600.0, UnitsHelper.Units.A)
			{
				Minimum = -1500.0,
				Maximum = 1500.0,
				Name = PID.GetResourceString("PID_01B5_0"),
				ShortName = PID.GetResourceString("PID_01B5_0"),
				Id = 609
			};
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 0, 610, 0, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.05 - 1600.0);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 1, 611, 1, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]) * 0.05 - 1600.0);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 2, 612, 2, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]) * 0.05 - 1600.0);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 3, 613, 3, 9, 2, (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]) * 0.05 - 1600.0);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 4, 614, 4, 11, 2, (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]) * 0.05 - 1600.0);
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 5, 615, 5, 13, 2, (byte[] data) => (double)((int)data[13] * 256 + (int)data[14]) * 0.05 - 1600.0);
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 6, 616, 6, 15, 2, (byte[] data) => (double)((int)data[15] * 256 + (int)data[16]) * 0.05 - 1600.0);
			array[8] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.A, 7, 617, 7, 17, 2, (byte[] data) => (double)((int)data[17] * 256 + (int)data[18]) * 0.05 - 1600.0);
			return array;
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x001EF6E0 File Offset: 0x001ED8E0
		public static PID[] GetPID01B6()
		{
			string text = "01B6";
			PID[] array = new PID[9];
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.05, UnitsHelper.Units.volts)
			{
				Minimum = -1500.0,
				Maximum = 1500.0,
				Name = PID.GetResourceString("PID_01B6_0"),
				ShortName = PID.GetResourceString("PID_01B6_0"),
				Id = 618
			};
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 0, 619, 0, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.05);
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 1, 620, 1, 5, 2, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]) * 0.05);
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 2, 621, 2, 7, 2, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]) * 0.05);
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 3, 622, 3, 9, 2, (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]) * 0.05);
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 4, 623, 4, 11, 2, (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]) * 0.05);
			array[6] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 5, 624, 5, 13, 2, (byte[] data) => (double)((int)data[13] * 256 + (int)data[14]) * 0.05);
			array[7] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 6, 625, 6, 15, 2, (byte[] data) => (double)((int)data[15] * 256 + (int)data[16]) * 0.05);
			array[8] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.volts, 7, 626, 7, 17, 2, (byte[] data) => (double)((int)data[17] * 256 + (int)data[18]) * 0.05);
			return array;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x001EF90C File Offset: 0x001EDB0C
		public static PID[] GetPID01B7()
		{
			string text = "01B7";
			PID[] array = new PID[6];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 0, 817, 0, 1, 1, (byte[] data) => (double)(data[1] - 40));
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 1, 818, 1, 2, 1, (byte[] data) => (double)(data[2] - 40));
			array[2] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 2, 819, 2, 3, 1, (byte[] data) => (double)(data[3] - 40));
			array[3] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 3, 820, 3, 4, 1, (byte[] data) => (double)(data[4] - 40));
			array[4] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 4, 821, 4, 5, 1, (byte[] data) => (double)(data[5] - 40));
			array[5] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.celicium, 5, 822, 4, 5, 1, (byte[] data) => (double)data[6]);
			return array;
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x001EFA58 File Offset: 0x001EDC58
		public static PID[] GetPID01BB()
		{
			string text = "01BB";
			PID[] array = new PID[9];
			UnitsHelper.Units units = UnitsHelper.Units.kWh;
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 16777216 + (int)data[2] * 65536 + (int)data[3] * 256 + (int)data[4]), units)
			{
				Minimum = 0.0,
				Maximum = 1000000.0,
				Name = PID.GetResourceString("PID_" + text + "_0"),
				ShortName = PID.GetResourceString("PID_" + text + "_0"),
				Id = 829
			};
			array[1] = new ModernPIDWithFloatValue(text, units, 0, 830, 0, 5, 4, (byte[] data) => (double)((int)data[5] * 16777216 + (int)data[6] * 65536 + (int)data[7] * 256 + (int)data[8]));
			array[2] = new ModernPIDWithFloatValue(text, units, 1, 831, 1, 9, 4, (byte[] data) => (double)((int)data[9] * 16777216 + (int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]));
			array[3] = new ModernPIDWithFloatValue(text, units, 2, 832, 2, 13, 4, (byte[] data) => (double)((int)data[13] * 16777216 + (int)data[14] * 65536 + (int)data[15] * 256 + (int)data[16]));
			array[4] = new ModernPIDWithFloatValue(text, units, 3, 833, 3, 16, 4, (byte[] data) => (double)((int)data[16] * 16777216 + (int)data[17] * 65536 + (int)data[18] * 256 + (int)data[19]));
			array[5] = new ModernPIDWithFloatValue(text, units, 4, 834, 4, 20, 4, (byte[] data) => (double)((int)data[20] * 16777216 + (int)data[21] * 65536 + (int)data[22] * 256 + (int)data[23]));
			array[6] = new ModernPIDWithFloatValue(text, units, 5, 835, 5, 21, 4, (byte[] data) => (double)((int)data[24] * 16777216 + (int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]));
			array[7] = new ModernPIDWithFloatValue(text, units, 6, 836, 6, 28, 4, (byte[] data) => (double)((int)data[28] * 16777216 + (int)data[29] * 65536 + (int)data[30] * 256 + (int)data[31]));
			array[8] = new ModernPIDWithFloatValue(text, units, 7, 837, 7, 32, 4, (byte[] data) => (double)((int)data[32] * 16777216 + (int)data[33] * 65536 + (int)data[34] * 256 + (int)data[35]));
			return array;
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x001EFC94 File Offset: 0x001EDE94
		public static PID[] GetPID01BC()
		{
			string text = "01BC";
			PID[] array = new PID[9];
			UnitsHelper.Units units = UnitsHelper.Units.kWh;
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 16777216 + (int)data[2] * 65536 + (int)data[3] * 256 + (int)data[4]), units)
			{
				Minimum = 0.0,
				Maximum = 1000000.0,
				Name = PID.GetResourceString("PID_" + text + "_0"),
				ShortName = PID.GetResourceString("PID_" + text + "_0"),
				Id = 838
			};
			array[1] = new ModernPIDWithFloatValue(text, units, 0, 839, 0, 5, 4, (byte[] data) => (double)((int)data[5] * 16777216 + (int)data[6] * 65536 + (int)data[7] * 256 + (int)data[8]));
			array[2] = new ModernPIDWithFloatValue(text, units, 1, 840, 1, 9, 4, (byte[] data) => (double)((int)data[9] * 16777216 + (int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]));
			array[3] = new ModernPIDWithFloatValue(text, units, 2, 841, 2, 13, 4, (byte[] data) => (double)((int)data[13] * 16777216 + (int)data[14] * 65536 + (int)data[15] * 256 + (int)data[16]));
			array[4] = new ModernPIDWithFloatValue(text, units, 3, 842, 3, 16, 4, (byte[] data) => (double)((int)data[16] * 16777216 + (int)data[17] * 65536 + (int)data[18] * 256 + (int)data[19]));
			array[5] = new ModernPIDWithFloatValue(text, units, 4, 843, 4, 20, 4, (byte[] data) => (double)((int)data[20] * 16777216 + (int)data[21] * 65536 + (int)data[22] * 256 + (int)data[23]));
			array[6] = new ModernPIDWithFloatValue(text, units, 5, 844, 5, 21, 4, (byte[] data) => (double)((int)data[24] * 16777216 + (int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]));
			array[7] = new ModernPIDWithFloatValue(text, units, 6, 845, 6, 28, 4, (byte[] data) => (double)((int)data[28] * 16777216 + (int)data[29] * 65536 + (int)data[30] * 256 + (int)data[31]));
			array[8] = new ModernPIDWithFloatValue(text, units, 7, 846, 7, 32, 4, (byte[] data) => (double)((int)data[32] * 16777216 + (int)data[33] * 65536 + (int)data[34] * 256 + (int)data[35]));
			return array;
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x001EFED0 File Offset: 0x001EE0D0
		public static PID[] GetPID01BD()
		{
			string text = "01BD";
			PID[] array = new PID[9];
			UnitsHelper.Units units = UnitsHelper.Units.Wh;
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)(((int)data[1] * 16777216 + (int)data[2] * 65536 + (int)data[3] * 256 + (int)data[4]) * 100), units)
			{
				Minimum = 0.0,
				Maximum = 1000000.0,
				Name = PID.GetResourceString("PID_" + text + "_0"),
				ShortName = PID.GetResourceString("PID_" + text + "_0"),
				Id = 847
			};
			array[1] = new ModernPIDWithFloatValue(text, units, 0, 848, 0, 5, 4, (byte[] data) => (double)(((int)data[5] * 16777216 + (int)data[6] * 65536 + (int)data[7] * 256 + (int)data[8]) * 100));
			array[2] = new ModernPIDWithFloatValue(text, units, 1, 849, 1, 9, 4, (byte[] data) => (double)(((int)data[9] * 16777216 + (int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]) * 100));
			array[3] = new ModernPIDWithFloatValue(text, units, 2, 850, 2, 13, 4, (byte[] data) => (double)(((int)data[13] * 16777216 + (int)data[14] * 65536 + (int)data[15] * 256 + (int)data[16]) * 100));
			array[4] = new ModernPIDWithFloatValue(text, units, 3, 851, 3, 16, 4, (byte[] data) => (double)(((int)data[16] * 16777216 + (int)data[17] * 65536 + (int)data[18] * 256 + (int)data[19]) * 100));
			array[5] = new ModernPIDWithFloatValue(text, units, 4, 852, 4, 20, 4, (byte[] data) => (double)(((int)data[20] * 16777216 + (int)data[21] * 65536 + (int)data[22] * 256 + (int)data[23]) * 100));
			array[6] = new ModernPIDWithFloatValue(text, units, 5, 853, 5, 21, 4, (byte[] data) => (double)(((int)data[24] * 16777216 + (int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]) * 100));
			array[7] = new ModernPIDWithFloatValue(text, units, 6, 854, 6, 28, 4, (byte[] data) => (double)(((int)data[28] * 16777216 + (int)data[29] * 65536 + (int)data[30] * 256 + (int)data[31]) * 100));
			array[8] = new ModernPIDWithFloatValue(text, units, 7, 855, 7, 32, 4, (byte[] data) => (double)(((int)data[32] * 16777216 + (int)data[33] * 65536 + (int)data[34] * 256 + (int)data[35]) * 100));
			return array;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x001F010C File Offset: 0x001EE30C
		public static PID[] GetPID01BE()
		{
			string text = "01BE";
			PID[] array = new PID[9];
			UnitsHelper.Units units = UnitsHelper.Units.percent;
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)data[1] * 0.4, units)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Name = PID.GetResourceString("PID_" + text + "_0"),
				ShortName = PID.GetResourceString("PID_" + text + "_0"),
				Id = 856
			};
			array[1] = new ModernPIDWithFloatValue(text, units, 0, 857, 0, 2, 1, (byte[] data) => (double)data[2] * 0.4);
			array[2] = new ModernPIDWithFloatValue(text, units, 1, 858, 1, 3, 1, (byte[] data) => (double)data[3] * 0.4);
			array[3] = new ModernPIDWithFloatValue(text, units, 2, 859, 2, 4, 1, (byte[] data) => (double)data[4] * 0.4);
			array[4] = new ModernPIDWithFloatValue(text, units, 3, 860, 3, 5, 1, (byte[] data) => (double)data[5] * 0.4);
			array[5] = new ModernPIDWithFloatValue(text, units, 4, 861, 4, 6, 1, (byte[] data) => (double)data[6] * 0.4);
			array[6] = new ModernPIDWithFloatValue(text, units, 5, 862, 5, 7, 1, (byte[] data) => (double)data[7] * 0.4);
			array[7] = new ModernPIDWithFloatValue(text, units, 6, 863, 6, 8, 1, (byte[] data) => (double)data[8] * 0.4);
			array[8] = new ModernPIDWithFloatValue(text, units, 7, 864, 7, 9, 1, (byte[] data) => (double)data[9] * 0.4);
			return array;
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x001F0344 File Offset: 0x001EE544
		public static PID[] GetPID01BF()
		{
			string text = "01BF";
			PID[] array = new PID[9];
			UnitsHelper.Units units = UnitsHelper.Units.percent;
			int num = 2;
			double multiplier = 0.0015625;
			double divider = 1.0;
			double offset = 0.0;
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * multiplier / divider + offset, units)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Name = PID.GetResourceString("PID_" + text + "_0"),
				ShortName = PID.GetResourceString("PID_" + text + "_0"),
				Id = 865
			};
			array[1] = new ModernPIDWithFloatValue(text, units, 0, 866, 0, 3, num, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * multiplier / divider + offset);
			array[2] = new ModernPIDWithFloatValue(text, units, 1, 867, 1, 5, num, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]) * multiplier / divider + offset);
			array[3] = new ModernPIDWithFloatValue(text, units, 2, 868, 2, 7, num, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]) * multiplier / divider + offset);
			array[4] = new ModernPIDWithFloatValue(text, units, 3, 869, 3, 9, num, (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]) * multiplier / divider + offset);
			array[5] = new ModernPIDWithFloatValue(text, units, 4, 870, 4, 11, num, (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]) * multiplier / divider + offset);
			array[6] = new ModernPIDWithFloatValue(text, units, 5, 871, 5, 13, num, (byte[] data) => (double)((int)data[13] * 256 + (int)data[14]) * multiplier / divider + offset);
			array[7] = new ModernPIDWithFloatValue(text, units, 6, 872, 6, 15, num, (byte[] data) => (double)((int)data[15] * 256 + (int)data[16]) * multiplier / divider + offset);
			array[8] = new ModernPIDWithFloatValue(text, units, 7, 873, 7, 17, num, (byte[] data) => (double)((int)data[17] * 256 + (int)data[18]) * multiplier / divider + offset);
			return array;
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x001F0508 File Offset: 0x001EE708
		public static PID[] GetPID01C1()
		{
			string text = "01C1";
			PID[] array = new PID[9];
			UnitsHelper.Units units = UnitsHelper.Units.percent;
			int num = 2;
			double multiplier = 0.0015625;
			double divider = 1.0;
			double offset = 0.0;
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * multiplier / divider + offset, units)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Name = PID.GetResourceString("PID_" + text + "_0"),
				ShortName = PID.GetResourceString("PID_" + text + "_0"),
				Id = 874
			};
			array[1] = new ModernPIDWithFloatValue(text, units, 0, 875, 0, 3, num, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * multiplier / divider + offset);
			array[2] = new ModernPIDWithFloatValue(text, units, 1, 876, 1, 5, num, (byte[] data) => (double)((int)data[5] * 256 + (int)data[6]) * multiplier / divider + offset);
			array[3] = new ModernPIDWithFloatValue(text, units, 2, 877, 2, 7, num, (byte[] data) => (double)((int)data[7] * 256 + (int)data[8]) * multiplier / divider + offset);
			array[4] = new ModernPIDWithFloatValue(text, units, 3, 878, 3, 9, num, (byte[] data) => (double)((int)data[9] * 256 + (int)data[10]) * multiplier / divider + offset);
			array[5] = new ModernPIDWithFloatValue(text, units, 4, 879, 4, 11, num, (byte[] data) => (double)((int)data[11] * 256 + (int)data[12]) * multiplier / divider + offset);
			array[6] = new ModernPIDWithFloatValue(text, units, 5, 880, 5, 13, num, (byte[] data) => (double)((int)data[13] * 256 + (int)data[14]) * multiplier / divider + offset);
			array[7] = new ModernPIDWithFloatValue(text, units, 6, 881, 6, 15, num, (byte[] data) => (double)((int)data[15] * 256 + (int)data[16]) * multiplier / divider + offset);
			array[8] = new ModernPIDWithFloatValue(text, units, 7, 882, 7, 17, num, (byte[] data) => (double)((int)data[17] * 256 + (int)data[18]) * multiplier / divider + offset);
			return array;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x001F06CC File Offset: 0x001EE8CC
		public static PID[] GetPID01C2()
		{
			string text = "01C2";
			PID[] array = new PID[9];
			UnitsHelper.Units units = UnitsHelper.Units.kWh;
			int num = 3;
			double multiplier = 0.001;
			double divider = 1.0;
			double offset = 0.0;
			array[0] = new PIDWithFloatValueFormula(text, (byte[] data) => (double)((int)data[1] * 65536 + (int)data[2] * 256 + (int)data[3]) * multiplier / divider + offset, units)
			{
				Minimum = 0.0,
				Maximum = 100.0,
				Name = PID.GetResourceString("PID_" + text + "_0"),
				ShortName = PID.GetResourceString("PID_" + text + "_0"),
				Id = 883
			};
			array[1] = new ModernPIDWithFloatValue(text, units, 0, 884, 0, 4, num, (byte[] data) => (double)((int)data[4] * 65536 + (int)data[5] * 256 + (int)data[6]) * multiplier / divider + offset);
			array[2] = new ModernPIDWithFloatValue(text, units, 1, 885, 1, 7, num, (byte[] data) => (double)((int)data[7] * 65536 + (int)data[8] * 256 + (int)data[9]) * multiplier / divider + offset);
			array[3] = new ModernPIDWithFloatValue(text, units, 2, 886, 2, 10, num, (byte[] data) => (double)((int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]) * multiplier / divider + offset);
			array[4] = new ModernPIDWithFloatValue(text, units, 3, 887, 3, 13, num, (byte[] data) => (double)((int)data[13] * 65536 + (int)data[14] * 256 + (int)data[15]) * multiplier / divider + offset);
			array[5] = new ModernPIDWithFloatValue(text, units, 4, 888, 4, 16, num, (byte[] data) => (double)((int)data[16] * 65536 + (int)data[17] * 256 + (int)data[18]) * multiplier / divider + offset);
			array[6] = new ModernPIDWithFloatValue(text, units, 5, 889, 5, 19, num, (byte[] data) => (double)((int)data[19] * 65536 + (int)data[20] * 256 + (int)data[21]) * multiplier / divider + offset);
			array[7] = new ModernPIDWithFloatValue(text, units, 6, 890, 6, 22, num, (byte[] data) => (double)((int)data[22] * 65536 + (int)data[23] * 256 + (int)data[24]) * multiplier / divider + offset);
			array[8] = new ModernPIDWithFloatValue(text, units, 7, 899, 7, 25, num, (byte[] data) => (double)((int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]) * multiplier / divider + offset);
			return array;
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x001F0894 File Offset: 0x001EEA94
		public static PID[] GetPID01C3()
		{
			string text = "01C3";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 0, 891, 0, 1, 1, (byte[] data) => (double)(data[1] * 100 / byte.MaxValue));
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.percent, 1, 892, 1, 2, 1, (byte[] data) => (double)(data[2] * 100 / byte.MaxValue));
			return array;
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x001F0914 File Offset: 0x001EEB14
		public static PID[] GetPID01C5()
		{
			string text = "01C5";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 0, 895, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.079);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 1, 896, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.079);
			return array;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x001F0994 File Offset: 0x001EEB94
		public static PID[] GetPID01CB()
		{
			string text = "01CB";
			PID[] array = new PID[2];
			array[0] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 0, 897, 0, 1, 2, (byte[] data) => (double)((int)data[1] * 256 + (int)data[2]) * 0.01);
			array[1] = new ModernPIDWithFloatValue(text, UnitsHelper.Units.kPa, 1, 898, 1, 3, 2, (byte[] data) => (double)((int)data[3] * 256 + (int)data[4]) * 0.01);
			return array;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x001F0A14 File Offset: 0x001EEC14
		[CompilerGenerated]
		private double <GetFormula>b__6_0(byte[] data)
		{
			if (data.Length == 0)
			{
				return double.NaN;
			}
			if (BitHelpers.GetBit_1_8(data[0], this.bit_supported + 1) && this.start_byte + this.data_length <= data.Length)
			{
				return this.Formula(data);
			}
			return double.NaN;
		}

		// Token: 0x04001700 RID: 5888
		private int bit_supported;

		// Token: 0x04001701 RID: 5889
		private int start_byte;

		// Token: 0x04001702 RID: 5890
		private int data_length;

		// Token: 0x02000400 RID: 1024
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600293A RID: 10554 RVA: 0x001F0A69 File Offset: 0x001EEC69
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600293B RID: 10555 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600293C RID: 10556 RVA: 0x001F0A75 File Offset: 0x001EEC75
			internal double <GetPID0185>b__7_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.005;
			}

			// Token: 0x0600293D RID: 10557 RVA: 0x001F0A8F File Offset: 0x001EEC8F
			internal double <GetPID0185>b__7_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.005;
			}

			// Token: 0x0600293E RID: 10558 RVA: 0x001F0AA9 File Offset: 0x001EECA9
			internal double <GetPID0185>b__7_2(byte[] data)
			{
				return (double)(data[5] * 100 / byte.MaxValue);
			}

			// Token: 0x0600293F RID: 10559 RVA: 0x001F0A75 File Offset: 0x001EEC75
			internal double <GetPID0186>b__8_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.005;
			}

			// Token: 0x06002940 RID: 10560 RVA: 0x001F0A8F File Offset: 0x001EEC8F
			internal double <GetPID0186>b__8_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.005;
			}

			// Token: 0x06002941 RID: 10561 RVA: 0x001F0AB8 File Offset: 0x001EECB8
			internal double <GetPID0187>b__9_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.03125;
			}

			// Token: 0x06002942 RID: 10562 RVA: 0x001F0AD2 File Offset: 0x001EECD2
			internal double <GetPID0187>b__9_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.03125;
			}

			// Token: 0x06002943 RID: 10563 RVA: 0x001F0AEC File Offset: 0x001EECEC
			internal double <GetPID0188>b__10_0(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x06002944 RID: 10564 RVA: 0x001F0AFC File Offset: 0x001EECFC
			internal double <GetPID0188>b__10_1(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]);
			}

			// Token: 0x06002945 RID: 10565 RVA: 0x001F0B0C File Offset: 0x001EED0C
			internal double <GetPID0188>b__10_2(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]);
			}

			// Token: 0x06002946 RID: 10566 RVA: 0x001F0B1C File Offset: 0x001EED1C
			internal double <GetPID0188>b__10_3(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]);
			}

			// Token: 0x06002947 RID: 10567 RVA: 0x001F0B2E File Offset: 0x001EED2E
			internal double <GetPID0188>b__10_4(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]);
			}

			// Token: 0x06002948 RID: 10568 RVA: 0x001F0B40 File Offset: 0x001EED40
			internal double <GetPID018B>b__11_5(byte[] data)
			{
				if (!BitHelpers.GetBit_1_8(data[1], 1))
				{
					return 0.0;
				}
				if (BitHelpers.GetBit_1_8(data[1], 2))
				{
					return 2.0;
				}
				return 1.0;
			}

			// Token: 0x06002949 RID: 10569 RVA: 0x001F0B75 File Offset: 0x001EED75
			internal double <GetPID018B>b__11_0(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 3) > false;
			}

			// Token: 0x0600294A RID: 10570 RVA: 0x001F0B84 File Offset: 0x001EED84
			internal double <GetPID018B>b__11_1(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 4) > false;
			}

			// Token: 0x0600294B RID: 10571 RVA: 0x001F0B93 File Offset: 0x001EED93
			internal double <GetPID018B>b__11_2(byte[] data)
			{
				return (double)(data[2] * 100 / byte.MaxValue);
			}

			// Token: 0x0600294C RID: 10572 RVA: 0x001F0AEC File Offset: 0x001EECEC
			internal double <GetPID018B>b__11_3(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x0600294D RID: 10573 RVA: 0x001F0AFC File Offset: 0x001EECFC
			internal double <GetPID018B>b__11_4(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]);
			}

			// Token: 0x0600294E RID: 10574 RVA: 0x001F0BA2 File Offset: 0x001EEDA2
			internal double <GetPID018C>b__12_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.001526;
			}

			// Token: 0x0600294F RID: 10575 RVA: 0x001F0BBC File Offset: 0x001EEDBC
			internal double <GetPID018C>b__12_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.001526;
			}

			// Token: 0x06002950 RID: 10576 RVA: 0x001F0BD6 File Offset: 0x001EEDD6
			internal double <GetPID018C>b__12_2(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]) * 0.001526;
			}

			// Token: 0x06002951 RID: 10577 RVA: 0x001F0BF0 File Offset: 0x001EEDF0
			internal double <GetPID018C>b__12_3(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]) * 0.001526;
			}

			// Token: 0x06002952 RID: 10578 RVA: 0x001F0C0A File Offset: 0x001EEE0A
			internal double <GetPID018C>b__12_4(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]) * 0.000122;
			}

			// Token: 0x06002953 RID: 10579 RVA: 0x001F0C26 File Offset: 0x001EEE26
			internal double <GetPID018C>b__12_5(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]) * 0.000122;
			}

			// Token: 0x06002954 RID: 10580 RVA: 0x001F0C42 File Offset: 0x001EEE42
			internal double <GetPID018C>b__12_6(byte[] data)
			{
				return (double)((int)data[13] * 256 + (int)data[14]) * 0.000122;
			}

			// Token: 0x06002955 RID: 10581 RVA: 0x001F0C5E File Offset: 0x001EEE5E
			internal double <GetPID018C>b__12_7(byte[] data)
			{
				return (double)((int)data[15] * 256 + (int)data[16]) * 0.000122;
			}

			// Token: 0x06002956 RID: 10582 RVA: 0x001F0C7A File Offset: 0x001EEE7A
			internal double <GetPID018F>b__13_0(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 1) > false;
			}

			// Token: 0x06002957 RID: 10583 RVA: 0x001F0C89 File Offset: 0x001EEE89
			internal double <GetPID018F>b__13_1(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 2) > false;
			}

			// Token: 0x06002958 RID: 10584 RVA: 0x001F0C98 File Offset: 0x001EEE98
			internal double <GetPID018F>b__13_2(byte[] data)
			{
				return (double)((short)((int)data[2] * 256 + (int)data[3])) * 0.01;
			}

			// Token: 0x06002959 RID: 10585 RVA: 0x001F0CB3 File Offset: 0x001EEEB3
			internal double <GetPID018F>b__13_3(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[4], 1) > false;
			}

			// Token: 0x0600295A RID: 10586 RVA: 0x001F0CC2 File Offset: 0x001EEEC2
			internal double <GetPID018F>b__13_4(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[4], 2) > false;
			}

			// Token: 0x0600295B RID: 10587 RVA: 0x001F0CD1 File Offset: 0x001EEED1
			internal double <GetPID018F>b__13_5(byte[] data)
			{
				return (double)((short)((int)data[5] * 256 + (int)data[6])) * 0.01;
			}

			// Token: 0x0600295C RID: 10588 RVA: 0x001F0C7A File Offset: 0x001EEE7A
			internal double <GetPID0192>b__14_0(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 1) > false;
			}

			// Token: 0x0600295D RID: 10589 RVA: 0x001F0C89 File Offset: 0x001EEE89
			internal double <GetPID0192>b__14_1(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 2) > false;
			}

			// Token: 0x0600295E RID: 10590 RVA: 0x001F0B75 File Offset: 0x001EED75
			internal double <GetPID0192>b__14_2(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 3) > false;
			}

			// Token: 0x0600295F RID: 10591 RVA: 0x001F0B84 File Offset: 0x001EED84
			internal double <GetPID0192>b__14_3(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 4) > false;
			}

			// Token: 0x06002960 RID: 10592 RVA: 0x001F0CEC File Offset: 0x001EEEEC
			internal double <GetPID0192>b__14_4(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 5) > false;
			}

			// Token: 0x06002961 RID: 10593 RVA: 0x001F0CFB File Offset: 0x001EEEFB
			internal double <GetPID0192>b__14_5(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 6) > false;
			}

			// Token: 0x06002962 RID: 10594 RVA: 0x001F0D0A File Offset: 0x001EEF0A
			internal double <GetPID0192>b__14_6(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 7) > false;
			}

			// Token: 0x06002963 RID: 10595 RVA: 0x001F0D19 File Offset: 0x001EEF19
			internal double <GetPID0192>b__14_7(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 8) > false;
			}

			// Token: 0x06002964 RID: 10596 RVA: 0x001F0D28 File Offset: 0x001EEF28
			internal double <GetPID0193>b__15_0(byte[] data)
			{
				return (double)((short)((int)data[1] * 256 + (int)data[2]));
			}

			// Token: 0x06002965 RID: 10597 RVA: 0x001F0C7A File Offset: 0x001EEE7A
			internal double <GetPID0194>b__16_0(byte[] data)
			{
				return BitHelpers.GetBit_1_8(data[1], 1) > false;
			}

			// Token: 0x06002966 RID: 10598 RVA: 0x001F0D39 File Offset: 0x001EEF39
			internal double <GetPID0194>b__16_1(byte[] data)
			{
				return (double)((int)data[2] * 256 + (int)data[3]);
			}

			// Token: 0x06002967 RID: 10599 RVA: 0x001F0D49 File Offset: 0x001EEF49
			internal double <GetPID0194>b__16_2(byte[] data)
			{
				return (double)((int)data[4] * 256 + (int)data[5]);
			}

			// Token: 0x06002968 RID: 10600 RVA: 0x001F0D59 File Offset: 0x001EEF59
			internal double <GetPID0194>b__16_3(byte[] data)
			{
				return (double)((int)data[6] * 256 + (int)data[7]);
			}

			// Token: 0x06002969 RID: 10601 RVA: 0x001F0D69 File Offset: 0x001EEF69
			internal double <GetPID0194>b__16_4(byte[] data)
			{
				return (double)((int)data[8] * 256 + (int)data[9]);
			}

			// Token: 0x0600296A RID: 10602 RVA: 0x001F0D7A File Offset: 0x001EEF7A
			internal double <GetPID0194>b__16_5(byte[] data)
			{
				return (double)((int)data[10] * 256 + (int)data[11]);
			}

			// Token: 0x0600296B RID: 10603 RVA: 0x001F0D8C File Offset: 0x001EEF8C
			internal double <GetPID0198>b__17_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
			}

			// Token: 0x0600296C RID: 10604 RVA: 0x001F0DB0 File Offset: 0x001EEFB0
			internal double <GetPID0198>b__17_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
			}

			// Token: 0x0600296D RID: 10605 RVA: 0x001F0D8C File Offset: 0x001EEF8C
			internal double <GetPID0198>b__17_2(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
			}

			// Token: 0x0600296E RID: 10606 RVA: 0x001F0DB0 File Offset: 0x001EEFB0
			internal double <GetPID0198>b__17_3(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
			}

			// Token: 0x0600296F RID: 10607 RVA: 0x001F0D8C File Offset: 0x001EEF8C
			internal double <GetPID0199>b__18_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
			}

			// Token: 0x06002970 RID: 10608 RVA: 0x001F0DB0 File Offset: 0x001EEFB0
			internal double <GetPID0199>b__18_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
			}

			// Token: 0x06002971 RID: 10609 RVA: 0x001F0D8C File Offset: 0x001EEF8C
			internal double <GetPID0199>b__18_2(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.1 - 40.0;
			}

			// Token: 0x06002972 RID: 10610 RVA: 0x001F0DB0 File Offset: 0x001EEFB0
			internal double <GetPID0199>b__18_3(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.1 - 40.0;
			}

			// Token: 0x06002973 RID: 10611 RVA: 0x001F0DD4 File Offset: 0x001EEFD4
			internal double <GetPID019A_2_3>b__19_0(byte[] data)
			{
				return (double)((int)data[2] * 256 + (int)data[3]) * 0.015625;
			}

			// Token: 0x06002974 RID: 10612 RVA: 0x001F0DEE File Offset: 0x001EEFEE
			internal double <GetPID019A_2_3>b__19_1(byte[] data)
			{
				return (double)((short)((int)data[4] * 256 + (int)data[5])) * 0.1;
			}

			// Token: 0x06002975 RID: 10613 RVA: 0x001F0E09 File Offset: 0x001EF009
			internal double <GetPID019B>b__20_0(byte[] data)
			{
				return (double)data[1] * 0.25;
			}

			// Token: 0x06002976 RID: 10614 RVA: 0x001F0E19 File Offset: 0x001EF019
			internal double <GetPID019B>b__20_1(byte[] data)
			{
				return (double)(data[2] - 40);
			}

			// Token: 0x06002977 RID: 10615 RVA: 0x001F0E22 File Offset: 0x001EF022
			internal double <GetPID019B>b__20_2(byte[] data)
			{
				return (double)(data[3] * 100 / byte.MaxValue);
			}

			// Token: 0x06002978 RID: 10616 RVA: 0x001F0BA2 File Offset: 0x001EEDA2
			internal double <GetPID019C>b__21_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.001526;
			}

			// Token: 0x06002979 RID: 10617 RVA: 0x001F0BBC File Offset: 0x001EEDBC
			internal double <GetPID019C>b__21_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.001526;
			}

			// Token: 0x0600297A RID: 10618 RVA: 0x001F0BD6 File Offset: 0x001EEDD6
			internal double <GetPID019C>b__21_2(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]) * 0.001526;
			}

			// Token: 0x0600297B RID: 10619 RVA: 0x001F0BF0 File Offset: 0x001EEDF0
			internal double <GetPID019C>b__21_3(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]) * 0.001526;
			}

			// Token: 0x0600297C RID: 10620 RVA: 0x001F0C0A File Offset: 0x001EEE0A
			internal double <GetPID019C>b__21_4(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]) * 0.000122;
			}

			// Token: 0x0600297D RID: 10621 RVA: 0x001F0C26 File Offset: 0x001EEE26
			internal double <GetPID019C>b__21_5(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]) * 0.000122;
			}

			// Token: 0x0600297E RID: 10622 RVA: 0x001F0C42 File Offset: 0x001EEE42
			internal double <GetPID019C>b__21_6(byte[] data)
			{
				return (double)((int)data[13] * 256 + (int)data[14]) * 0.000122;
			}

			// Token: 0x0600297F RID: 10623 RVA: 0x001F0C5E File Offset: 0x001EEE5E
			internal double <GetPID019C>b__21_7(byte[] data)
			{
				return (double)((int)data[15] * 256 + (int)data[16]) * 0.000122;
			}

			// Token: 0x06002980 RID: 10624 RVA: 0x001F0E31 File Offset: 0x001EF031
			internal double <GetPID019F>b__22_0(byte[] data)
			{
				return (double)(data[1] * 100 / byte.MaxValue);
			}

			// Token: 0x06002981 RID: 10625 RVA: 0x001F0B93 File Offset: 0x001EED93
			internal double <GetPID019F>b__22_1(byte[] data)
			{
				return (double)(data[2] * 100 / byte.MaxValue);
			}

			// Token: 0x06002982 RID: 10626 RVA: 0x001F0E22 File Offset: 0x001EF022
			internal double <GetPID019F>b__22_2(byte[] data)
			{
				return (double)(data[3] * 100 / byte.MaxValue);
			}

			// Token: 0x06002983 RID: 10627 RVA: 0x001F0E40 File Offset: 0x001EF040
			internal double <GetPID019F>b__22_3(byte[] data)
			{
				return (double)(data[4] * 100 / byte.MaxValue);
			}

			// Token: 0x06002984 RID: 10628 RVA: 0x001F0AA9 File Offset: 0x001EECA9
			internal double <GetPID019F>b__22_4(byte[] data)
			{
				return (double)(data[5] * 100 / byte.MaxValue);
			}

			// Token: 0x06002985 RID: 10629 RVA: 0x001F0E4F File Offset: 0x001EF04F
			internal double <GetPID019F>b__22_5(byte[] data)
			{
				return (double)(data[6] * 100 / byte.MaxValue);
			}

			// Token: 0x06002986 RID: 10630 RVA: 0x001F0E5E File Offset: 0x001EF05E
			internal double <GetPID019F>b__22_6(byte[] data)
			{
				return (double)(data[7] * 100 / byte.MaxValue);
			}

			// Token: 0x06002987 RID: 10631 RVA: 0x001F0E6D File Offset: 0x001EF06D
			internal double <GetPID019F>b__22_7(byte[] data)
			{
				return (double)(data[8] * 100 / byte.MaxValue);
			}

			// Token: 0x06002988 RID: 10632 RVA: 0x001F0E7C File Offset: 0x001EF07C
			internal double <GetPID01A1>b__23_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]);
			}

			// Token: 0x06002989 RID: 10633 RVA: 0x001F0AEC File Offset: 0x001EECEC
			internal double <GetPID01A1>b__23_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x0600298A RID: 10634 RVA: 0x001F0AFC File Offset: 0x001EECFC
			internal double <GetPID01A1>b__23_2(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]);
			}

			// Token: 0x0600298B RID: 10635 RVA: 0x001F0B0C File Offset: 0x001EED0C
			internal double <GetPID01A1>b__23_3(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]);
			}

			// Token: 0x0600298C RID: 10636 RVA: 0x001F0E8C File Offset: 0x001EF08C
			internal double <GetPID01A5>b__24_0(byte[] data)
			{
				return (double)data[1] * 0.5;
			}

			// Token: 0x0600298D RID: 10637 RVA: 0x001F0E9C File Offset: 0x001EF09C
			internal double <GetPID01A5>b__24_1(byte[] data)
			{
				return (double)((int)data[2] * 256 + (int)data[3]) * 0.0005;
			}

			// Token: 0x0600298E RID: 10638 RVA: 0x001F0E7C File Offset: 0x001EF07C
			internal double <GetPID01A7>b__25_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]);
			}

			// Token: 0x0600298F RID: 10639 RVA: 0x001F0AEC File Offset: 0x001EECEC
			internal double <GetPID01A7>b__25_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x06002990 RID: 10640 RVA: 0x001F0AFC File Offset: 0x001EECFC
			internal double <GetPID01A7>b__25_2(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]);
			}

			// Token: 0x06002991 RID: 10641 RVA: 0x001F0B0C File Offset: 0x001EED0C
			internal double <GetPID01A7>b__25_3(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]);
			}

			// Token: 0x06002992 RID: 10642 RVA: 0x001F0E7C File Offset: 0x001EF07C
			internal double <GetPID01A8>b__26_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]);
			}

			// Token: 0x06002993 RID: 10643 RVA: 0x001F0AEC File Offset: 0x001EECEC
			internal double <GetPID01A8>b__26_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x06002994 RID: 10644 RVA: 0x001F0AFC File Offset: 0x001EECFC
			internal double <GetPID01A8>b__26_2(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]);
			}

			// Token: 0x06002995 RID: 10645 RVA: 0x001F0B0C File Offset: 0x001EED0C
			internal double <GetPID01A8>b__26_3(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]);
			}

			// Token: 0x06002996 RID: 10646 RVA: 0x001F0EB6 File Offset: 0x001EF0B6
			internal double <GetPID01A9>b__27_0(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[1], 0) > false;
			}

			// Token: 0x06002997 RID: 10647 RVA: 0x001F0AB8 File Offset: 0x001EECB8
			internal double <GetPID01AB>b__28_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.03125;
			}

			// Token: 0x06002998 RID: 10648 RVA: 0x001F0EC5 File Offset: 0x001EF0C5
			internal double <GetPID01AB>b__28_1(byte[] data)
			{
				return (double)(data[3] - 40);
			}

			// Token: 0x06002999 RID: 10649 RVA: 0x001F0ECE File Offset: 0x001EF0CE
			internal double <GetPID01AB>b__28_2(byte[] data)
			{
				return (double)((int)data[4] * 256 + (int)data[5]) * 0.125;
			}

			// Token: 0x0600299A RID: 10650 RVA: 0x001F0D59 File Offset: 0x001EEF59
			internal double <GetPID01AB>b__28_3(byte[] data)
			{
				return (double)((int)data[6] * 256 + (int)data[7]);
			}

			// Token: 0x0600299B RID: 10651 RVA: 0x001F0EE8 File Offset: 0x001EF0E8
			internal double <GetPID01AB>b__28_4(byte[] data)
			{
				return (double)((int)(data[8] * 2) - 256);
			}

			// Token: 0x0600299C RID: 10652 RVA: 0x001F0EF6 File Offset: 0x001EF0F6
			internal double <GetPID01AC>b__29_0(byte[] data)
			{
				return (double)data[1];
			}

			// Token: 0x0600299D RID: 10653 RVA: 0x001F0EFC File Offset: 0x001EF0FC
			internal double <GetPID01AC>b__29_1(byte[] data)
			{
				return (double)((int)data[2] * 256 + (int)data[3]) * 0.3;
			}

			// Token: 0x0600299E RID: 10654 RVA: 0x001F0F16 File Offset: 0x001EF116
			internal double <GetPID01AD>b__30_0(byte[] data)
			{
				return PIDWithFloatValueFormula.Signed16Bit(data[1], data[2]) * 0.25;
			}

			// Token: 0x0600299F RID: 10655 RVA: 0x001F0AEC File Offset: 0x001EECEC
			internal double <GetPID01AD>b__30_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x060029A0 RID: 10656 RVA: 0x001F0F16 File Offset: 0x001EF116
			internal double <GetPID01AE>b__31_0(byte[] data)
			{
				return PIDWithFloatValueFormula.Signed16Bit(data[1], data[2]) * 0.25;
			}

			// Token: 0x060029A1 RID: 10657 RVA: 0x001F0F2D File Offset: 0x001EF12D
			internal double <GetPID01AE>b__31_1(byte[] data)
			{
				return PIDWithFloatValueFormula.Signed16Bit(data[3], data[4]) * 2.0;
			}

			// Token: 0x060029A2 RID: 10658 RVA: 0x001F0F44 File Offset: 0x001EF144
			internal double <GetPID01B0>b__32_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.05;
			}

			// Token: 0x060029A3 RID: 10659 RVA: 0x001F0F5E File Offset: 0x001EF15E
			internal double <GetPID01B0>b__32_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.05;
			}

			// Token: 0x060029A4 RID: 10660 RVA: 0x001F0EB6 File Offset: 0x001EF0B6
			internal double <GetPID01B1>b__33_0(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[1], 0) > false;
			}

			// Token: 0x060029A5 RID: 10661 RVA: 0x001F0F78 File Offset: 0x001EF178
			internal double <GetPID01B1>b__33_1(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[1], 1) > false;
			}

			// Token: 0x060029A6 RID: 10662 RVA: 0x001F0F87 File Offset: 0x001EF187
			internal double <GetPID01B1>b__33_2(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[1], 2) > false;
			}

			// Token: 0x060029A7 RID: 10663 RVA: 0x001F0F96 File Offset: 0x001EF196
			internal double <GetPID01B1>b__33_3(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[1], 4) > false;
			}

			// Token: 0x060029A8 RID: 10664 RVA: 0x001F0FA5 File Offset: 0x001EF1A5
			internal double <GetPID01B1>b__33_4(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[1], 5) > false;
			}

			// Token: 0x060029A9 RID: 10665 RVA: 0x001F0FB4 File Offset: 0x001EF1B4
			internal double <GetPID01B1>b__33_5(byte[] data)
			{
				return BitHelpers.GetBit_0_7(data[1], 6) > false;
			}

			// Token: 0x060029AA RID: 10666 RVA: 0x001F0FC3 File Offset: 0x001EF1C3
			internal double <GetPID01B1>b__33_6(byte[] data)
			{
				return (double)data[2] * 100.0 / 128.0 - 100.0;
			}

			// Token: 0x060029AB RID: 10667 RVA: 0x001F0FE7 File Offset: 0x001EF1E7
			internal double <GetPID01B1>b__33_7(byte[] data)
			{
				return (double)data[3] * 100.0 / 128.0 - 100.0;
			}

			// Token: 0x060029AC RID: 10668 RVA: 0x001F100B File Offset: 0x001EF20B
			internal double <GetPID01B1>b__33_8(byte[] data)
			{
				return (double)data[4] * 100.0 / 128.0 - 100.0;
			}

			// Token: 0x060029AD RID: 10669 RVA: 0x001F102F File Offset: 0x001EF22F
			internal double <GetPID01B1>b__33_9(byte[] data)
			{
				return (double)data[5] * 100.0 / 128.0 - 100.0;
			}

			// Token: 0x060029AE RID: 10670 RVA: 0x001F0F44 File Offset: 0x001EF144
			internal double <GetPID01B3>b__34_8(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.05;
			}

			// Token: 0x060029AF RID: 10671 RVA: 0x001F0F5E File Offset: 0x001EF15E
			internal double <GetPID01B3>b__34_0(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.05;
			}

			// Token: 0x060029B0 RID: 10672 RVA: 0x001F1053 File Offset: 0x001EF253
			internal double <GetPID01B3>b__34_1(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]) * 0.05;
			}

			// Token: 0x060029B1 RID: 10673 RVA: 0x001F106D File Offset: 0x001EF26D
			internal double <GetPID01B3>b__34_2(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]) * 0.05;
			}

			// Token: 0x060029B2 RID: 10674 RVA: 0x001F1087 File Offset: 0x001EF287
			internal double <GetPID01B3>b__34_3(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]) * 0.05;
			}

			// Token: 0x060029B3 RID: 10675 RVA: 0x001F10A3 File Offset: 0x001EF2A3
			internal double <GetPID01B3>b__34_4(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]) * 0.05;
			}

			// Token: 0x060029B4 RID: 10676 RVA: 0x001F10BF File Offset: 0x001EF2BF
			internal double <GetPID01B3>b__34_5(byte[] data)
			{
				return (double)((int)data[13] * 256 + (int)data[14]) * 0.05;
			}

			// Token: 0x060029B5 RID: 10677 RVA: 0x001F10DB File Offset: 0x001EF2DB
			internal double <GetPID01B3>b__34_6(byte[] data)
			{
				return (double)((int)data[15] * 256 + (int)data[16]) * 0.05;
			}

			// Token: 0x060029B6 RID: 10678 RVA: 0x001F10F7 File Offset: 0x001EF2F7
			internal double <GetPID01B3>b__34_7(byte[] data)
			{
				return (double)((int)data[17] * 256 + (int)data[18]) * 0.05;
			}

			// Token: 0x060029B7 RID: 10679 RVA: 0x001F1113 File Offset: 0x001EF313
			internal double <GetPID01B4>b__35_8(byte[] data)
			{
				return (double)(data[1] - 40);
			}

			// Token: 0x060029B8 RID: 10680 RVA: 0x001F0E19 File Offset: 0x001EF019
			internal double <GetPID01B4>b__35_0(byte[] data)
			{
				return (double)(data[2] - 40);
			}

			// Token: 0x060029B9 RID: 10681 RVA: 0x001F0EC5 File Offset: 0x001EF0C5
			internal double <GetPID01B4>b__35_1(byte[] data)
			{
				return (double)(data[3] - 40);
			}

			// Token: 0x060029BA RID: 10682 RVA: 0x001F111C File Offset: 0x001EF31C
			internal double <GetPID01B4>b__35_2(byte[] data)
			{
				return (double)(data[4] - 40);
			}

			// Token: 0x060029BB RID: 10683 RVA: 0x001F1125 File Offset: 0x001EF325
			internal double <GetPID01B4>b__35_3(byte[] data)
			{
				return (double)(data[5] - 40);
			}

			// Token: 0x060029BC RID: 10684 RVA: 0x001F112E File Offset: 0x001EF32E
			internal double <GetPID01B4>b__35_4(byte[] data)
			{
				return (double)(data[6] - 40);
			}

			// Token: 0x060029BD RID: 10685 RVA: 0x001F1137 File Offset: 0x001EF337
			internal double <GetPID01B4>b__35_5(byte[] data)
			{
				return (double)(data[7] - 40);
			}

			// Token: 0x060029BE RID: 10686 RVA: 0x001F1140 File Offset: 0x001EF340
			internal double <GetPID01B4>b__35_6(byte[] data)
			{
				return (double)(data[8] - 40);
			}

			// Token: 0x060029BF RID: 10687 RVA: 0x001F1149 File Offset: 0x001EF349
			internal double <GetPID01B4>b__35_7(byte[] data)
			{
				return (double)(data[9] - 40);
			}

			// Token: 0x060029C0 RID: 10688 RVA: 0x001F1153 File Offset: 0x001EF353
			internal double <GetPID01B5>b__36_8(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C1 RID: 10689 RVA: 0x001F1177 File Offset: 0x001EF377
			internal double <GetPID01B5>b__36_0(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C2 RID: 10690 RVA: 0x001F119B File Offset: 0x001EF39B
			internal double <GetPID01B5>b__36_1(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C3 RID: 10691 RVA: 0x001F11BF File Offset: 0x001EF3BF
			internal double <GetPID01B5>b__36_2(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C4 RID: 10692 RVA: 0x001F11E3 File Offset: 0x001EF3E3
			internal double <GetPID01B5>b__36_3(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C5 RID: 10693 RVA: 0x001F1209 File Offset: 0x001EF409
			internal double <GetPID01B5>b__36_4(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C6 RID: 10694 RVA: 0x001F122F File Offset: 0x001EF42F
			internal double <GetPID01B5>b__36_5(byte[] data)
			{
				return (double)((int)data[13] * 256 + (int)data[14]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C7 RID: 10695 RVA: 0x001F1255 File Offset: 0x001EF455
			internal double <GetPID01B5>b__36_6(byte[] data)
			{
				return (double)((int)data[15] * 256 + (int)data[16]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C8 RID: 10696 RVA: 0x001F127B File Offset: 0x001EF47B
			internal double <GetPID01B5>b__36_7(byte[] data)
			{
				return (double)((int)data[17] * 256 + (int)data[18]) * 0.05 - 1600.0;
			}

			// Token: 0x060029C9 RID: 10697 RVA: 0x001F0F44 File Offset: 0x001EF144
			internal double <GetPID01B6>b__37_8(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.05;
			}

			// Token: 0x060029CA RID: 10698 RVA: 0x001F0F5E File Offset: 0x001EF15E
			internal double <GetPID01B6>b__37_0(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.05;
			}

			// Token: 0x060029CB RID: 10699 RVA: 0x001F1053 File Offset: 0x001EF253
			internal double <GetPID01B6>b__37_1(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]) * 0.05;
			}

			// Token: 0x060029CC RID: 10700 RVA: 0x001F106D File Offset: 0x001EF26D
			internal double <GetPID01B6>b__37_2(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]) * 0.05;
			}

			// Token: 0x060029CD RID: 10701 RVA: 0x001F1087 File Offset: 0x001EF287
			internal double <GetPID01B6>b__37_3(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]) * 0.05;
			}

			// Token: 0x060029CE RID: 10702 RVA: 0x001F10A3 File Offset: 0x001EF2A3
			internal double <GetPID01B6>b__37_4(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]) * 0.05;
			}

			// Token: 0x060029CF RID: 10703 RVA: 0x001F10BF File Offset: 0x001EF2BF
			internal double <GetPID01B6>b__37_5(byte[] data)
			{
				return (double)((int)data[13] * 256 + (int)data[14]) * 0.05;
			}

			// Token: 0x060029D0 RID: 10704 RVA: 0x001F10DB File Offset: 0x001EF2DB
			internal double <GetPID01B6>b__37_6(byte[] data)
			{
				return (double)((int)data[15] * 256 + (int)data[16]) * 0.05;
			}

			// Token: 0x060029D1 RID: 10705 RVA: 0x001F10F7 File Offset: 0x001EF2F7
			internal double <GetPID01B6>b__37_7(byte[] data)
			{
				return (double)((int)data[17] * 256 + (int)data[18]) * 0.05;
			}

			// Token: 0x060029D2 RID: 10706 RVA: 0x001F1113 File Offset: 0x001EF313
			internal double <GetPID01B7>b__38_0(byte[] data)
			{
				return (double)(data[1] - 40);
			}

			// Token: 0x060029D3 RID: 10707 RVA: 0x001F0E19 File Offset: 0x001EF019
			internal double <GetPID01B7>b__38_1(byte[] data)
			{
				return (double)(data[2] - 40);
			}

			// Token: 0x060029D4 RID: 10708 RVA: 0x001F0EC5 File Offset: 0x001EF0C5
			internal double <GetPID01B7>b__38_2(byte[] data)
			{
				return (double)(data[3] - 40);
			}

			// Token: 0x060029D5 RID: 10709 RVA: 0x001F111C File Offset: 0x001EF31C
			internal double <GetPID01B7>b__38_3(byte[] data)
			{
				return (double)(data[4] - 40);
			}

			// Token: 0x060029D6 RID: 10710 RVA: 0x001F1125 File Offset: 0x001EF325
			internal double <GetPID01B7>b__38_4(byte[] data)
			{
				return (double)(data[5] - 40);
			}

			// Token: 0x060029D7 RID: 10711 RVA: 0x001F12A1 File Offset: 0x001EF4A1
			internal double <GetPID01B7>b__38_5(byte[] data)
			{
				return (double)data[6];
			}

			// Token: 0x060029D8 RID: 10712 RVA: 0x001F12A7 File Offset: 0x001EF4A7
			internal double <GetPID01BB>b__39_8(byte[] data)
			{
				return (double)((int)data[1] * 16777216 + (int)data[2] * 65536 + (int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x060029D9 RID: 10713 RVA: 0x001F12CB File Offset: 0x001EF4CB
			internal double <GetPID01BB>b__39_0(byte[] data)
			{
				return (double)((int)data[5] * 16777216 + (int)data[6] * 65536 + (int)data[7] * 256 + (int)data[8]);
			}

			// Token: 0x060029DA RID: 10714 RVA: 0x001F12EF File Offset: 0x001EF4EF
			internal double <GetPID01BB>b__39_1(byte[] data)
			{
				return (double)((int)data[9] * 16777216 + (int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]);
			}

			// Token: 0x060029DB RID: 10715 RVA: 0x001F1317 File Offset: 0x001EF517
			internal double <GetPID01BB>b__39_2(byte[] data)
			{
				return (double)((int)data[13] * 16777216 + (int)data[14] * 65536 + (int)data[15] * 256 + (int)data[16]);
			}

			// Token: 0x060029DC RID: 10716 RVA: 0x001F133F File Offset: 0x001EF53F
			internal double <GetPID01BB>b__39_3(byte[] data)
			{
				return (double)((int)data[16] * 16777216 + (int)data[17] * 65536 + (int)data[18] * 256 + (int)data[19]);
			}

			// Token: 0x060029DD RID: 10717 RVA: 0x001F1367 File Offset: 0x001EF567
			internal double <GetPID01BB>b__39_4(byte[] data)
			{
				return (double)((int)data[20] * 16777216 + (int)data[21] * 65536 + (int)data[22] * 256 + (int)data[23]);
			}

			// Token: 0x060029DE RID: 10718 RVA: 0x001F138F File Offset: 0x001EF58F
			internal double <GetPID01BB>b__39_5(byte[] data)
			{
				return (double)((int)data[24] * 16777216 + (int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]);
			}

			// Token: 0x060029DF RID: 10719 RVA: 0x001F13B7 File Offset: 0x001EF5B7
			internal double <GetPID01BB>b__39_6(byte[] data)
			{
				return (double)((int)data[28] * 16777216 + (int)data[29] * 65536 + (int)data[30] * 256 + (int)data[31]);
			}

			// Token: 0x060029E0 RID: 10720 RVA: 0x001F13DF File Offset: 0x001EF5DF
			internal double <GetPID01BB>b__39_7(byte[] data)
			{
				return (double)((int)data[32] * 16777216 + (int)data[33] * 65536 + (int)data[34] * 256 + (int)data[35]);
			}

			// Token: 0x060029E1 RID: 10721 RVA: 0x001F12A7 File Offset: 0x001EF4A7
			internal double <GetPID01BC>b__40_8(byte[] data)
			{
				return (double)((int)data[1] * 16777216 + (int)data[2] * 65536 + (int)data[3] * 256 + (int)data[4]);
			}

			// Token: 0x060029E2 RID: 10722 RVA: 0x001F12CB File Offset: 0x001EF4CB
			internal double <GetPID01BC>b__40_0(byte[] data)
			{
				return (double)((int)data[5] * 16777216 + (int)data[6] * 65536 + (int)data[7] * 256 + (int)data[8]);
			}

			// Token: 0x060029E3 RID: 10723 RVA: 0x001F12EF File Offset: 0x001EF4EF
			internal double <GetPID01BC>b__40_1(byte[] data)
			{
				return (double)((int)data[9] * 16777216 + (int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]);
			}

			// Token: 0x060029E4 RID: 10724 RVA: 0x001F1317 File Offset: 0x001EF517
			internal double <GetPID01BC>b__40_2(byte[] data)
			{
				return (double)((int)data[13] * 16777216 + (int)data[14] * 65536 + (int)data[15] * 256 + (int)data[16]);
			}

			// Token: 0x060029E5 RID: 10725 RVA: 0x001F133F File Offset: 0x001EF53F
			internal double <GetPID01BC>b__40_3(byte[] data)
			{
				return (double)((int)data[16] * 16777216 + (int)data[17] * 65536 + (int)data[18] * 256 + (int)data[19]);
			}

			// Token: 0x060029E6 RID: 10726 RVA: 0x001F1367 File Offset: 0x001EF567
			internal double <GetPID01BC>b__40_4(byte[] data)
			{
				return (double)((int)data[20] * 16777216 + (int)data[21] * 65536 + (int)data[22] * 256 + (int)data[23]);
			}

			// Token: 0x060029E7 RID: 10727 RVA: 0x001F138F File Offset: 0x001EF58F
			internal double <GetPID01BC>b__40_5(byte[] data)
			{
				return (double)((int)data[24] * 16777216 + (int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]);
			}

			// Token: 0x060029E8 RID: 10728 RVA: 0x001F13B7 File Offset: 0x001EF5B7
			internal double <GetPID01BC>b__40_6(byte[] data)
			{
				return (double)((int)data[28] * 16777216 + (int)data[29] * 65536 + (int)data[30] * 256 + (int)data[31]);
			}

			// Token: 0x060029E9 RID: 10729 RVA: 0x001F13DF File Offset: 0x001EF5DF
			internal double <GetPID01BC>b__40_7(byte[] data)
			{
				return (double)((int)data[32] * 16777216 + (int)data[33] * 65536 + (int)data[34] * 256 + (int)data[35]);
			}

			// Token: 0x060029EA RID: 10730 RVA: 0x001F1407 File Offset: 0x001EF607
			internal double <GetPID01BD>b__41_8(byte[] data)
			{
				return (double)(((int)data[1] * 16777216 + (int)data[2] * 65536 + (int)data[3] * 256 + (int)data[4]) * 100);
			}

			// Token: 0x060029EB RID: 10731 RVA: 0x001F142E File Offset: 0x001EF62E
			internal double <GetPID01BD>b__41_0(byte[] data)
			{
				return (double)(((int)data[5] * 16777216 + (int)data[6] * 65536 + (int)data[7] * 256 + (int)data[8]) * 100);
			}

			// Token: 0x060029EC RID: 10732 RVA: 0x001F1455 File Offset: 0x001EF655
			internal double <GetPID01BD>b__41_1(byte[] data)
			{
				return (double)(((int)data[9] * 16777216 + (int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]) * 100);
			}

			// Token: 0x060029ED RID: 10733 RVA: 0x001F1480 File Offset: 0x001EF680
			internal double <GetPID01BD>b__41_2(byte[] data)
			{
				return (double)(((int)data[13] * 16777216 + (int)data[14] * 65536 + (int)data[15] * 256 + (int)data[16]) * 100);
			}

			// Token: 0x060029EE RID: 10734 RVA: 0x001F14AB File Offset: 0x001EF6AB
			internal double <GetPID01BD>b__41_3(byte[] data)
			{
				return (double)(((int)data[16] * 16777216 + (int)data[17] * 65536 + (int)data[18] * 256 + (int)data[19]) * 100);
			}

			// Token: 0x060029EF RID: 10735 RVA: 0x001F14D6 File Offset: 0x001EF6D6
			internal double <GetPID01BD>b__41_4(byte[] data)
			{
				return (double)(((int)data[20] * 16777216 + (int)data[21] * 65536 + (int)data[22] * 256 + (int)data[23]) * 100);
			}

			// Token: 0x060029F0 RID: 10736 RVA: 0x001F1501 File Offset: 0x001EF701
			internal double <GetPID01BD>b__41_5(byte[] data)
			{
				return (double)(((int)data[24] * 16777216 + (int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]) * 100);
			}

			// Token: 0x060029F1 RID: 10737 RVA: 0x001F152C File Offset: 0x001EF72C
			internal double <GetPID01BD>b__41_6(byte[] data)
			{
				return (double)(((int)data[28] * 16777216 + (int)data[29] * 65536 + (int)data[30] * 256 + (int)data[31]) * 100);
			}

			// Token: 0x060029F2 RID: 10738 RVA: 0x001F1557 File Offset: 0x001EF757
			internal double <GetPID01BD>b__41_7(byte[] data)
			{
				return (double)(((int)data[32] * 16777216 + (int)data[33] * 65536 + (int)data[34] * 256 + (int)data[35]) * 100);
			}

			// Token: 0x060029F3 RID: 10739 RVA: 0x001F1582 File Offset: 0x001EF782
			internal double <GetPID01BE>b__42_8(byte[] data)
			{
				return (double)data[1] * 0.4;
			}

			// Token: 0x060029F4 RID: 10740 RVA: 0x001F1592 File Offset: 0x001EF792
			internal double <GetPID01BE>b__42_0(byte[] data)
			{
				return (double)data[2] * 0.4;
			}

			// Token: 0x060029F5 RID: 10741 RVA: 0x001F15A2 File Offset: 0x001EF7A2
			internal double <GetPID01BE>b__42_1(byte[] data)
			{
				return (double)data[3] * 0.4;
			}

			// Token: 0x060029F6 RID: 10742 RVA: 0x001F15B2 File Offset: 0x001EF7B2
			internal double <GetPID01BE>b__42_2(byte[] data)
			{
				return (double)data[4] * 0.4;
			}

			// Token: 0x060029F7 RID: 10743 RVA: 0x001F15C2 File Offset: 0x001EF7C2
			internal double <GetPID01BE>b__42_3(byte[] data)
			{
				return (double)data[5] * 0.4;
			}

			// Token: 0x060029F8 RID: 10744 RVA: 0x001F15D2 File Offset: 0x001EF7D2
			internal double <GetPID01BE>b__42_4(byte[] data)
			{
				return (double)data[6] * 0.4;
			}

			// Token: 0x060029F9 RID: 10745 RVA: 0x001F15E2 File Offset: 0x001EF7E2
			internal double <GetPID01BE>b__42_5(byte[] data)
			{
				return (double)data[7] * 0.4;
			}

			// Token: 0x060029FA RID: 10746 RVA: 0x001F15F2 File Offset: 0x001EF7F2
			internal double <GetPID01BE>b__42_6(byte[] data)
			{
				return (double)data[8] * 0.4;
			}

			// Token: 0x060029FB RID: 10747 RVA: 0x001F1602 File Offset: 0x001EF802
			internal double <GetPID01BE>b__42_7(byte[] data)
			{
				return (double)data[9] * 0.4;
			}

			// Token: 0x060029FC RID: 10748 RVA: 0x001F0E31 File Offset: 0x001EF031
			internal double <GetPID01C3>b__46_0(byte[] data)
			{
				return (double)(data[1] * 100 / byte.MaxValue);
			}

			// Token: 0x060029FD RID: 10749 RVA: 0x001F0B93 File Offset: 0x001EED93
			internal double <GetPID01C3>b__46_1(byte[] data)
			{
				return (double)(data[2] * 100 / byte.MaxValue);
			}

			// Token: 0x060029FE RID: 10750 RVA: 0x001F1613 File Offset: 0x001EF813
			internal double <GetPID01C5>b__47_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.079;
			}

			// Token: 0x060029FF RID: 10751 RVA: 0x001F162D File Offset: 0x001EF82D
			internal double <GetPID01C5>b__47_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.079;
			}

			// Token: 0x06002A00 RID: 10752 RVA: 0x001F1647 File Offset: 0x001EF847
			internal double <GetPID01CB>b__48_0(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * 0.01;
			}

			// Token: 0x06002A01 RID: 10753 RVA: 0x001F1661 File Offset: 0x001EF861
			internal double <GetPID01CB>b__48_1(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * 0.01;
			}

			// Token: 0x04001703 RID: 5891
			public static readonly ModernPIDWithFloatValue.<>c <>9 = new ModernPIDWithFloatValue.<>c();

			// Token: 0x04001704 RID: 5892
			public static Func<byte[], double> <>9__7_0;

			// Token: 0x04001705 RID: 5893
			public static Func<byte[], double> <>9__7_1;

			// Token: 0x04001706 RID: 5894
			public static Func<byte[], double> <>9__7_2;

			// Token: 0x04001707 RID: 5895
			public static Func<byte[], double> <>9__8_0;

			// Token: 0x04001708 RID: 5896
			public static Func<byte[], double> <>9__8_1;

			// Token: 0x04001709 RID: 5897
			public static Func<byte[], double> <>9__9_0;

			// Token: 0x0400170A RID: 5898
			public static Func<byte[], double> <>9__9_1;

			// Token: 0x0400170B RID: 5899
			public static Func<byte[], double> <>9__10_0;

			// Token: 0x0400170C RID: 5900
			public static Func<byte[], double> <>9__10_1;

			// Token: 0x0400170D RID: 5901
			public static Func<byte[], double> <>9__10_2;

			// Token: 0x0400170E RID: 5902
			public static Func<byte[], double> <>9__10_3;

			// Token: 0x0400170F RID: 5903
			public static Func<byte[], double> <>9__10_4;

			// Token: 0x04001710 RID: 5904
			public static Func<byte[], double> <>9__11_5;

			// Token: 0x04001711 RID: 5905
			public static Func<byte[], double> <>9__11_0;

			// Token: 0x04001712 RID: 5906
			public static Func<byte[], double> <>9__11_1;

			// Token: 0x04001713 RID: 5907
			public static Func<byte[], double> <>9__11_2;

			// Token: 0x04001714 RID: 5908
			public static Func<byte[], double> <>9__11_3;

			// Token: 0x04001715 RID: 5909
			public static Func<byte[], double> <>9__11_4;

			// Token: 0x04001716 RID: 5910
			public static Func<byte[], double> <>9__12_0;

			// Token: 0x04001717 RID: 5911
			public static Func<byte[], double> <>9__12_1;

			// Token: 0x04001718 RID: 5912
			public static Func<byte[], double> <>9__12_2;

			// Token: 0x04001719 RID: 5913
			public static Func<byte[], double> <>9__12_3;

			// Token: 0x0400171A RID: 5914
			public static Func<byte[], double> <>9__12_4;

			// Token: 0x0400171B RID: 5915
			public static Func<byte[], double> <>9__12_5;

			// Token: 0x0400171C RID: 5916
			public static Func<byte[], double> <>9__12_6;

			// Token: 0x0400171D RID: 5917
			public static Func<byte[], double> <>9__12_7;

			// Token: 0x0400171E RID: 5918
			public static Func<byte[], double> <>9__13_0;

			// Token: 0x0400171F RID: 5919
			public static Func<byte[], double> <>9__13_1;

			// Token: 0x04001720 RID: 5920
			public static Func<byte[], double> <>9__13_2;

			// Token: 0x04001721 RID: 5921
			public static Func<byte[], double> <>9__13_3;

			// Token: 0x04001722 RID: 5922
			public static Func<byte[], double> <>9__13_4;

			// Token: 0x04001723 RID: 5923
			public static Func<byte[], double> <>9__13_5;

			// Token: 0x04001724 RID: 5924
			public static Func<byte[], double> <>9__14_0;

			// Token: 0x04001725 RID: 5925
			public static Func<byte[], double> <>9__14_1;

			// Token: 0x04001726 RID: 5926
			public static Func<byte[], double> <>9__14_2;

			// Token: 0x04001727 RID: 5927
			public static Func<byte[], double> <>9__14_3;

			// Token: 0x04001728 RID: 5928
			public static Func<byte[], double> <>9__14_4;

			// Token: 0x04001729 RID: 5929
			public static Func<byte[], double> <>9__14_5;

			// Token: 0x0400172A RID: 5930
			public static Func<byte[], double> <>9__14_6;

			// Token: 0x0400172B RID: 5931
			public static Func<byte[], double> <>9__14_7;

			// Token: 0x0400172C RID: 5932
			public static Func<byte[], double> <>9__15_0;

			// Token: 0x0400172D RID: 5933
			public static Func<byte[], double> <>9__16_0;

			// Token: 0x0400172E RID: 5934
			public static Func<byte[], double> <>9__16_1;

			// Token: 0x0400172F RID: 5935
			public static Func<byte[], double> <>9__16_2;

			// Token: 0x04001730 RID: 5936
			public static Func<byte[], double> <>9__16_3;

			// Token: 0x04001731 RID: 5937
			public static Func<byte[], double> <>9__16_4;

			// Token: 0x04001732 RID: 5938
			public static Func<byte[], double> <>9__16_5;

			// Token: 0x04001733 RID: 5939
			public static Func<byte[], double> <>9__17_0;

			// Token: 0x04001734 RID: 5940
			public static Func<byte[], double> <>9__17_1;

			// Token: 0x04001735 RID: 5941
			public static Func<byte[], double> <>9__17_2;

			// Token: 0x04001736 RID: 5942
			public static Func<byte[], double> <>9__17_3;

			// Token: 0x04001737 RID: 5943
			public static Func<byte[], double> <>9__18_0;

			// Token: 0x04001738 RID: 5944
			public static Func<byte[], double> <>9__18_1;

			// Token: 0x04001739 RID: 5945
			public static Func<byte[], double> <>9__18_2;

			// Token: 0x0400173A RID: 5946
			public static Func<byte[], double> <>9__18_3;

			// Token: 0x0400173B RID: 5947
			public static Func<byte[], double> <>9__19_0;

			// Token: 0x0400173C RID: 5948
			public static Func<byte[], double> <>9__19_1;

			// Token: 0x0400173D RID: 5949
			public static Func<byte[], double> <>9__20_0;

			// Token: 0x0400173E RID: 5950
			public static Func<byte[], double> <>9__20_1;

			// Token: 0x0400173F RID: 5951
			public static Func<byte[], double> <>9__20_2;

			// Token: 0x04001740 RID: 5952
			public static Func<byte[], double> <>9__21_0;

			// Token: 0x04001741 RID: 5953
			public static Func<byte[], double> <>9__21_1;

			// Token: 0x04001742 RID: 5954
			public static Func<byte[], double> <>9__21_2;

			// Token: 0x04001743 RID: 5955
			public static Func<byte[], double> <>9__21_3;

			// Token: 0x04001744 RID: 5956
			public static Func<byte[], double> <>9__21_4;

			// Token: 0x04001745 RID: 5957
			public static Func<byte[], double> <>9__21_5;

			// Token: 0x04001746 RID: 5958
			public static Func<byte[], double> <>9__21_6;

			// Token: 0x04001747 RID: 5959
			public static Func<byte[], double> <>9__21_7;

			// Token: 0x04001748 RID: 5960
			public static Func<byte[], double> <>9__22_0;

			// Token: 0x04001749 RID: 5961
			public static Func<byte[], double> <>9__22_1;

			// Token: 0x0400174A RID: 5962
			public static Func<byte[], double> <>9__22_2;

			// Token: 0x0400174B RID: 5963
			public static Func<byte[], double> <>9__22_3;

			// Token: 0x0400174C RID: 5964
			public static Func<byte[], double> <>9__22_4;

			// Token: 0x0400174D RID: 5965
			public static Func<byte[], double> <>9__22_5;

			// Token: 0x0400174E RID: 5966
			public static Func<byte[], double> <>9__22_6;

			// Token: 0x0400174F RID: 5967
			public static Func<byte[], double> <>9__22_7;

			// Token: 0x04001750 RID: 5968
			public static Func<byte[], double> <>9__23_0;

			// Token: 0x04001751 RID: 5969
			public static Func<byte[], double> <>9__23_1;

			// Token: 0x04001752 RID: 5970
			public static Func<byte[], double> <>9__23_2;

			// Token: 0x04001753 RID: 5971
			public static Func<byte[], double> <>9__23_3;

			// Token: 0x04001754 RID: 5972
			public static Func<byte[], double> <>9__24_0;

			// Token: 0x04001755 RID: 5973
			public static Func<byte[], double> <>9__24_1;

			// Token: 0x04001756 RID: 5974
			public static Func<byte[], double> <>9__25_0;

			// Token: 0x04001757 RID: 5975
			public static Func<byte[], double> <>9__25_1;

			// Token: 0x04001758 RID: 5976
			public static Func<byte[], double> <>9__25_2;

			// Token: 0x04001759 RID: 5977
			public static Func<byte[], double> <>9__25_3;

			// Token: 0x0400175A RID: 5978
			public static Func<byte[], double> <>9__26_0;

			// Token: 0x0400175B RID: 5979
			public static Func<byte[], double> <>9__26_1;

			// Token: 0x0400175C RID: 5980
			public static Func<byte[], double> <>9__26_2;

			// Token: 0x0400175D RID: 5981
			public static Func<byte[], double> <>9__26_3;

			// Token: 0x0400175E RID: 5982
			public static Func<byte[], double> <>9__27_0;

			// Token: 0x0400175F RID: 5983
			public static Func<byte[], double> <>9__28_0;

			// Token: 0x04001760 RID: 5984
			public static Func<byte[], double> <>9__28_1;

			// Token: 0x04001761 RID: 5985
			public static Func<byte[], double> <>9__28_2;

			// Token: 0x04001762 RID: 5986
			public static Func<byte[], double> <>9__28_3;

			// Token: 0x04001763 RID: 5987
			public static Func<byte[], double> <>9__28_4;

			// Token: 0x04001764 RID: 5988
			public static Func<byte[], double> <>9__29_0;

			// Token: 0x04001765 RID: 5989
			public static Func<byte[], double> <>9__29_1;

			// Token: 0x04001766 RID: 5990
			public static Func<byte[], double> <>9__30_0;

			// Token: 0x04001767 RID: 5991
			public static Func<byte[], double> <>9__30_1;

			// Token: 0x04001768 RID: 5992
			public static Func<byte[], double> <>9__31_0;

			// Token: 0x04001769 RID: 5993
			public static Func<byte[], double> <>9__31_1;

			// Token: 0x0400176A RID: 5994
			public static Func<byte[], double> <>9__32_0;

			// Token: 0x0400176B RID: 5995
			public static Func<byte[], double> <>9__32_1;

			// Token: 0x0400176C RID: 5996
			public static Func<byte[], double> <>9__33_0;

			// Token: 0x0400176D RID: 5997
			public static Func<byte[], double> <>9__33_1;

			// Token: 0x0400176E RID: 5998
			public static Func<byte[], double> <>9__33_2;

			// Token: 0x0400176F RID: 5999
			public static Func<byte[], double> <>9__33_3;

			// Token: 0x04001770 RID: 6000
			public static Func<byte[], double> <>9__33_4;

			// Token: 0x04001771 RID: 6001
			public static Func<byte[], double> <>9__33_5;

			// Token: 0x04001772 RID: 6002
			public static Func<byte[], double> <>9__33_6;

			// Token: 0x04001773 RID: 6003
			public static Func<byte[], double> <>9__33_7;

			// Token: 0x04001774 RID: 6004
			public static Func<byte[], double> <>9__33_8;

			// Token: 0x04001775 RID: 6005
			public static Func<byte[], double> <>9__33_9;

			// Token: 0x04001776 RID: 6006
			public static Func<byte[], double> <>9__34_8;

			// Token: 0x04001777 RID: 6007
			public static Func<byte[], double> <>9__34_0;

			// Token: 0x04001778 RID: 6008
			public static Func<byte[], double> <>9__34_1;

			// Token: 0x04001779 RID: 6009
			public static Func<byte[], double> <>9__34_2;

			// Token: 0x0400177A RID: 6010
			public static Func<byte[], double> <>9__34_3;

			// Token: 0x0400177B RID: 6011
			public static Func<byte[], double> <>9__34_4;

			// Token: 0x0400177C RID: 6012
			public static Func<byte[], double> <>9__34_5;

			// Token: 0x0400177D RID: 6013
			public static Func<byte[], double> <>9__34_6;

			// Token: 0x0400177E RID: 6014
			public static Func<byte[], double> <>9__34_7;

			// Token: 0x0400177F RID: 6015
			public static Func<byte[], double> <>9__35_8;

			// Token: 0x04001780 RID: 6016
			public static Func<byte[], double> <>9__35_0;

			// Token: 0x04001781 RID: 6017
			public static Func<byte[], double> <>9__35_1;

			// Token: 0x04001782 RID: 6018
			public static Func<byte[], double> <>9__35_2;

			// Token: 0x04001783 RID: 6019
			public static Func<byte[], double> <>9__35_3;

			// Token: 0x04001784 RID: 6020
			public static Func<byte[], double> <>9__35_4;

			// Token: 0x04001785 RID: 6021
			public static Func<byte[], double> <>9__35_5;

			// Token: 0x04001786 RID: 6022
			public static Func<byte[], double> <>9__35_6;

			// Token: 0x04001787 RID: 6023
			public static Func<byte[], double> <>9__35_7;

			// Token: 0x04001788 RID: 6024
			public static Func<byte[], double> <>9__36_8;

			// Token: 0x04001789 RID: 6025
			public static Func<byte[], double> <>9__36_0;

			// Token: 0x0400178A RID: 6026
			public static Func<byte[], double> <>9__36_1;

			// Token: 0x0400178B RID: 6027
			public static Func<byte[], double> <>9__36_2;

			// Token: 0x0400178C RID: 6028
			public static Func<byte[], double> <>9__36_3;

			// Token: 0x0400178D RID: 6029
			public static Func<byte[], double> <>9__36_4;

			// Token: 0x0400178E RID: 6030
			public static Func<byte[], double> <>9__36_5;

			// Token: 0x0400178F RID: 6031
			public static Func<byte[], double> <>9__36_6;

			// Token: 0x04001790 RID: 6032
			public static Func<byte[], double> <>9__36_7;

			// Token: 0x04001791 RID: 6033
			public static Func<byte[], double> <>9__37_8;

			// Token: 0x04001792 RID: 6034
			public static Func<byte[], double> <>9__37_0;

			// Token: 0x04001793 RID: 6035
			public static Func<byte[], double> <>9__37_1;

			// Token: 0x04001794 RID: 6036
			public static Func<byte[], double> <>9__37_2;

			// Token: 0x04001795 RID: 6037
			public static Func<byte[], double> <>9__37_3;

			// Token: 0x04001796 RID: 6038
			public static Func<byte[], double> <>9__37_4;

			// Token: 0x04001797 RID: 6039
			public static Func<byte[], double> <>9__37_5;

			// Token: 0x04001798 RID: 6040
			public static Func<byte[], double> <>9__37_6;

			// Token: 0x04001799 RID: 6041
			public static Func<byte[], double> <>9__37_7;

			// Token: 0x0400179A RID: 6042
			public static Func<byte[], double> <>9__38_0;

			// Token: 0x0400179B RID: 6043
			public static Func<byte[], double> <>9__38_1;

			// Token: 0x0400179C RID: 6044
			public static Func<byte[], double> <>9__38_2;

			// Token: 0x0400179D RID: 6045
			public static Func<byte[], double> <>9__38_3;

			// Token: 0x0400179E RID: 6046
			public static Func<byte[], double> <>9__38_4;

			// Token: 0x0400179F RID: 6047
			public static Func<byte[], double> <>9__38_5;

			// Token: 0x040017A0 RID: 6048
			public static Func<byte[], double> <>9__39_8;

			// Token: 0x040017A1 RID: 6049
			public static Func<byte[], double> <>9__39_0;

			// Token: 0x040017A2 RID: 6050
			public static Func<byte[], double> <>9__39_1;

			// Token: 0x040017A3 RID: 6051
			public static Func<byte[], double> <>9__39_2;

			// Token: 0x040017A4 RID: 6052
			public static Func<byte[], double> <>9__39_3;

			// Token: 0x040017A5 RID: 6053
			public static Func<byte[], double> <>9__39_4;

			// Token: 0x040017A6 RID: 6054
			public static Func<byte[], double> <>9__39_5;

			// Token: 0x040017A7 RID: 6055
			public static Func<byte[], double> <>9__39_6;

			// Token: 0x040017A8 RID: 6056
			public static Func<byte[], double> <>9__39_7;

			// Token: 0x040017A9 RID: 6057
			public static Func<byte[], double> <>9__40_8;

			// Token: 0x040017AA RID: 6058
			public static Func<byte[], double> <>9__40_0;

			// Token: 0x040017AB RID: 6059
			public static Func<byte[], double> <>9__40_1;

			// Token: 0x040017AC RID: 6060
			public static Func<byte[], double> <>9__40_2;

			// Token: 0x040017AD RID: 6061
			public static Func<byte[], double> <>9__40_3;

			// Token: 0x040017AE RID: 6062
			public static Func<byte[], double> <>9__40_4;

			// Token: 0x040017AF RID: 6063
			public static Func<byte[], double> <>9__40_5;

			// Token: 0x040017B0 RID: 6064
			public static Func<byte[], double> <>9__40_6;

			// Token: 0x040017B1 RID: 6065
			public static Func<byte[], double> <>9__40_7;

			// Token: 0x040017B2 RID: 6066
			public static Func<byte[], double> <>9__41_8;

			// Token: 0x040017B3 RID: 6067
			public static Func<byte[], double> <>9__41_0;

			// Token: 0x040017B4 RID: 6068
			public static Func<byte[], double> <>9__41_1;

			// Token: 0x040017B5 RID: 6069
			public static Func<byte[], double> <>9__41_2;

			// Token: 0x040017B6 RID: 6070
			public static Func<byte[], double> <>9__41_3;

			// Token: 0x040017B7 RID: 6071
			public static Func<byte[], double> <>9__41_4;

			// Token: 0x040017B8 RID: 6072
			public static Func<byte[], double> <>9__41_5;

			// Token: 0x040017B9 RID: 6073
			public static Func<byte[], double> <>9__41_6;

			// Token: 0x040017BA RID: 6074
			public static Func<byte[], double> <>9__41_7;

			// Token: 0x040017BB RID: 6075
			public static Func<byte[], double> <>9__42_8;

			// Token: 0x040017BC RID: 6076
			public static Func<byte[], double> <>9__42_0;

			// Token: 0x040017BD RID: 6077
			public static Func<byte[], double> <>9__42_1;

			// Token: 0x040017BE RID: 6078
			public static Func<byte[], double> <>9__42_2;

			// Token: 0x040017BF RID: 6079
			public static Func<byte[], double> <>9__42_3;

			// Token: 0x040017C0 RID: 6080
			public static Func<byte[], double> <>9__42_4;

			// Token: 0x040017C1 RID: 6081
			public static Func<byte[], double> <>9__42_5;

			// Token: 0x040017C2 RID: 6082
			public static Func<byte[], double> <>9__42_6;

			// Token: 0x040017C3 RID: 6083
			public static Func<byte[], double> <>9__42_7;

			// Token: 0x040017C4 RID: 6084
			public static Func<byte[], double> <>9__46_0;

			// Token: 0x040017C5 RID: 6085
			public static Func<byte[], double> <>9__46_1;

			// Token: 0x040017C6 RID: 6086
			public static Func<byte[], double> <>9__47_0;

			// Token: 0x040017C7 RID: 6087
			public static Func<byte[], double> <>9__47_1;

			// Token: 0x040017C8 RID: 6088
			public static Func<byte[], double> <>9__48_0;

			// Token: 0x040017C9 RID: 6089
			public static Func<byte[], double> <>9__48_1;
		}

		// Token: 0x02000401 RID: 1025
		[CompilerGenerated]
		private sealed class <>c__DisplayClass43_0
		{
			// Token: 0x06002A02 RID: 10754 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass43_0()
			{
			}

			// Token: 0x06002A03 RID: 10755 RVA: 0x001F167B File Offset: 0x001EF87B
			internal double <GetPID01BF>b__8(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A04 RID: 10756 RVA: 0x001F16A0 File Offset: 0x001EF8A0
			internal double <GetPID01BF>b__0(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A05 RID: 10757 RVA: 0x001F16C5 File Offset: 0x001EF8C5
			internal double <GetPID01BF>b__1(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A06 RID: 10758 RVA: 0x001F16EA File Offset: 0x001EF8EA
			internal double <GetPID01BF>b__2(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A07 RID: 10759 RVA: 0x001F170F File Offset: 0x001EF90F
			internal double <GetPID01BF>b__3(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A08 RID: 10760 RVA: 0x001F1736 File Offset: 0x001EF936
			internal double <GetPID01BF>b__4(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A09 RID: 10761 RVA: 0x001F175D File Offset: 0x001EF95D
			internal double <GetPID01BF>b__5(byte[] data)
			{
				return (double)((int)data[13] * 256 + (int)data[14]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A0A RID: 10762 RVA: 0x001F1784 File Offset: 0x001EF984
			internal double <GetPID01BF>b__6(byte[] data)
			{
				return (double)((int)data[15] * 256 + (int)data[16]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A0B RID: 10763 RVA: 0x001F17AB File Offset: 0x001EF9AB
			internal double <GetPID01BF>b__7(byte[] data)
			{
				return (double)((int)data[17] * 256 + (int)data[18]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x040017CA RID: 6090
			public double multiplier;

			// Token: 0x040017CB RID: 6091
			public double divider;

			// Token: 0x040017CC RID: 6092
			public double offset;
		}

		// Token: 0x02000402 RID: 1026
		[CompilerGenerated]
		private sealed class <>c__DisplayClass44_0
		{
			// Token: 0x06002A0C RID: 10764 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass44_0()
			{
			}

			// Token: 0x06002A0D RID: 10765 RVA: 0x001F17D2 File Offset: 0x001EF9D2
			internal double <GetPID01C1>b__8(byte[] data)
			{
				return (double)((int)data[1] * 256 + (int)data[2]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A0E RID: 10766 RVA: 0x001F17F7 File Offset: 0x001EF9F7
			internal double <GetPID01C1>b__0(byte[] data)
			{
				return (double)((int)data[3] * 256 + (int)data[4]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A0F RID: 10767 RVA: 0x001F181C File Offset: 0x001EFA1C
			internal double <GetPID01C1>b__1(byte[] data)
			{
				return (double)((int)data[5] * 256 + (int)data[6]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A10 RID: 10768 RVA: 0x001F1841 File Offset: 0x001EFA41
			internal double <GetPID01C1>b__2(byte[] data)
			{
				return (double)((int)data[7] * 256 + (int)data[8]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A11 RID: 10769 RVA: 0x001F1866 File Offset: 0x001EFA66
			internal double <GetPID01C1>b__3(byte[] data)
			{
				return (double)((int)data[9] * 256 + (int)data[10]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A12 RID: 10770 RVA: 0x001F188D File Offset: 0x001EFA8D
			internal double <GetPID01C1>b__4(byte[] data)
			{
				return (double)((int)data[11] * 256 + (int)data[12]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A13 RID: 10771 RVA: 0x001F18B4 File Offset: 0x001EFAB4
			internal double <GetPID01C1>b__5(byte[] data)
			{
				return (double)((int)data[13] * 256 + (int)data[14]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A14 RID: 10772 RVA: 0x001F18DB File Offset: 0x001EFADB
			internal double <GetPID01C1>b__6(byte[] data)
			{
				return (double)((int)data[15] * 256 + (int)data[16]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A15 RID: 10773 RVA: 0x001F1902 File Offset: 0x001EFB02
			internal double <GetPID01C1>b__7(byte[] data)
			{
				return (double)((int)data[17] * 256 + (int)data[18]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x040017CD RID: 6093
			public double multiplier;

			// Token: 0x040017CE RID: 6094
			public double divider;

			// Token: 0x040017CF RID: 6095
			public double offset;
		}

		// Token: 0x02000403 RID: 1027
		[CompilerGenerated]
		private sealed class <>c__DisplayClass45_0
		{
			// Token: 0x06002A16 RID: 10774 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass45_0()
			{
			}

			// Token: 0x06002A17 RID: 10775 RVA: 0x001F1929 File Offset: 0x001EFB29
			internal double <GetPID01C2>b__8(byte[] data)
			{
				return (double)((int)data[1] * 65536 + (int)data[2] * 256 + (int)data[3]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A18 RID: 10776 RVA: 0x001F1958 File Offset: 0x001EFB58
			internal double <GetPID01C2>b__0(byte[] data)
			{
				return (double)((int)data[4] * 65536 + (int)data[5] * 256 + (int)data[6]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A19 RID: 10777 RVA: 0x001F1987 File Offset: 0x001EFB87
			internal double <GetPID01C2>b__1(byte[] data)
			{
				return (double)((int)data[7] * 65536 + (int)data[8] * 256 + (int)data[9]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A1A RID: 10778 RVA: 0x001F19B7 File Offset: 0x001EFBB7
			internal double <GetPID01C2>b__2(byte[] data)
			{
				return (double)((int)data[10] * 65536 + (int)data[11] * 256 + (int)data[12]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A1B RID: 10779 RVA: 0x001F19E9 File Offset: 0x001EFBE9
			internal double <GetPID01C2>b__3(byte[] data)
			{
				return (double)((int)data[13] * 65536 + (int)data[14] * 256 + (int)data[15]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A1C RID: 10780 RVA: 0x001F1A1B File Offset: 0x001EFC1B
			internal double <GetPID01C2>b__4(byte[] data)
			{
				return (double)((int)data[16] * 65536 + (int)data[17] * 256 + (int)data[18]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A1D RID: 10781 RVA: 0x001F1A4D File Offset: 0x001EFC4D
			internal double <GetPID01C2>b__5(byte[] data)
			{
				return (double)((int)data[19] * 65536 + (int)data[20] * 256 + (int)data[21]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A1E RID: 10782 RVA: 0x001F1A7F File Offset: 0x001EFC7F
			internal double <GetPID01C2>b__6(byte[] data)
			{
				return (double)((int)data[22] * 65536 + (int)data[23] * 256 + (int)data[24]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x06002A1F RID: 10783 RVA: 0x001F1AB1 File Offset: 0x001EFCB1
			internal double <GetPID01C2>b__7(byte[] data)
			{
				return (double)((int)data[25] * 65536 + (int)data[26] * 256 + (int)data[27]) * this.multiplier / this.divider + this.offset;
			}

			// Token: 0x040017D0 RID: 6096
			public double multiplier;

			// Token: 0x040017D1 RID: 6097
			public double divider;

			// Token: 0x040017D2 RID: 6098
			public double offset;
		}
	}
}
