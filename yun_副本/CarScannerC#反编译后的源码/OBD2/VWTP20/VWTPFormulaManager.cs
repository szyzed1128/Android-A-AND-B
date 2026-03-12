using System;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.OBD2.VWTP20
{
	// Token: 0x020003BF RID: 959
	internal static class VWTPFormulaManager
	{
		// Token: 0x06002791 RID: 10129 RVA: 0x001E4D06 File Offset: 0x001E2F06
		private static double ShortSigned(byte A, byte B)
		{
			return (double)((short)((int)A * 256 + (int)B));
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x001E4D14 File Offset: 0x001E2F14
		public static ValueTuple<double, UnitsHelper.Units> GetResult(byte formulaId, byte A, byte B)
		{
			switch (formulaId)
			{
			case 1:
				return new ValueTuple<double, UnitsHelper.Units>(0.2 * (double)A * (double)B, UnitsHelper.Units.rpm);
			case 2:
				return new ValueTuple<double, UnitsHelper.Units>((double)A * 0.002 * (double)B, UnitsHelper.Units.percent);
			case 3:
				return new ValueTuple<double, UnitsHelper.Units>(0.002 * (double)A * (double)B, UnitsHelper.Units.grads);
			case 4:
			{
				double num = (double)Math.Abs((int)(B - 127)) * 0.01 * (double)A;
				if (num > 127.0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(num, UnitsHelper.Units.ATDC);
				}
				return new ValueTuple<double, UnitsHelper.Units>(num, UnitsHelper.Units.BTDC);
			}
			case 5:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * (B - 100)) * 0.1, UnitsHelper.Units.celicium);
			case 6:
				return new ValueTuple<double, UnitsHelper.Units>(0.001 * (double)A * (double)B, UnitsHelper.Units.volts);
			case 7:
				return new ValueTuple<double, UnitsHelper.Units>(0.01 * (double)A * (double)B, UnitsHelper.Units.kmh);
			case 8:
				return new ValueTuple<double, UnitsHelper.Units>(0.1 * (double)A * (double)B, UnitsHelper.Units.None);
			case 9:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - 127) * 0.02 * (double)A, UnitsHelper.Units.grads);
			case 10:
				if (B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.None);
				}
				return new ValueTuple<double, UnitsHelper.Units>(1.0, UnitsHelper.Units.None);
			case 11:
				return new ValueTuple<double, UnitsHelper.Units>(0.0001 * (double)A * (double)(B - 128) + 1.0, UnitsHelper.Units.None);
			case 12:
				return new ValueTuple<double, UnitsHelper.Units>(0.001 * (double)A * (double)B, UnitsHelper.Units.Ohm);
			case 13:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - 127) * 0.001 * (double)A, UnitsHelper.Units.mm);
			case 14:
				return new ValueTuple<double, UnitsHelper.Units>(0.005 * (double)A * (double)B, UnitsHelper.Units.bar);
			case 15:
				return new ValueTuple<double, UnitsHelper.Units>(0.01 * (double)A * (double)B, UnitsHelper.Units.ms);
			case 16:
				return new ValueTuple<double, UnitsHelper.Units>((double)long.Parse(Convert.ToString(A, 2).PadLeft(8, '0') + Convert.ToString(B, 2).PadLeft(8, '0')), UnitsHelper.Units.None);
			case 17:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B), UnitsHelper.Units.None);
			case 18:
				return new ValueTuple<double, UnitsHelper.Units>(0.04 * (double)A * (double)B, UnitsHelper.Units.mbar);
			case 19:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.01, UnitsHelper.Units.liters);
			case 20:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * (B - 128) / 128), UnitsHelper.Units.percent);
			case 21:
				return new ValueTuple<double, UnitsHelper.Units>(0.001 * (double)A * (double)B, UnitsHelper.Units.volts);
			case 22:
				return new ValueTuple<double, UnitsHelper.Units>(0.001 * (double)A * (double)B, UnitsHelper.Units.ms);
			case 23:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)B / 256 * (int)A), UnitsHelper.Units.percent);
			case 24:
				return new ValueTuple<double, UnitsHelper.Units>(0.001 * (double)A * (double)B, UnitsHelper.Units.A);
			case 25:
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 1.421 + (double)(A / 182), UnitsHelper.Units.grams_sec);
			case 26:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - A), UnitsHelper.Units.celicium);
			case 27:
			{
				double num2 = (double)Math.Abs((int)(B - 128)) * 0.01 * (double)A;
				if (num2 < 128.0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(num2, UnitsHelper.Units.ATDC);
				}
				return new ValueTuple<double, UnitsHelper.Units>(num2, UnitsHelper.Units.BTDC);
			}
			case 28:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - A), UnitsHelper.Units.None);
			case 29:
				if (B < A)
				{
					return new ValueTuple<double, UnitsHelper.Units>(1.0, UnitsHelper.Units.None);
				}
				return new ValueTuple<double, UnitsHelper.Units>(2.0, UnitsHelper.Units.None);
			case 30:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B / 12 - A), UnitsHelper.Units.grads_CS);
			case 31:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)B / 2560 - (int)A), UnitsHelper.Units.celicium);
			case 32:
				if (B > 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((int)B - 256), UnitsHelper.Units.None);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)B, UnitsHelper.Units.None);
			case 33:
				if (A == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)(100 * B), UnitsHelper.Units.percent);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(100 * B / A), UnitsHelper.Units.percent);
			case 34:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - 128) * 0.01 * (double)A, UnitsHelper.Units.kW);
			case 35:
				return new ValueTuple<double, UnitsHelper.Units>(0.01 * (double)A * (double)B, UnitsHelper.Units.Lh);
			case 36:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 2560 + (int)(B * 10)), UnitsHelper.Units.km);
			case 37:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B), UnitsHelper.Units.None);
			case 38:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - 128) * 0.001 * (double)A, UnitsHelper.Units.grads_CS);
			case 39:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)B / 256 * (int)A), UnitsHelper.Units.mg_hour);
			case 40:
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 0.1 + 25.5 * (double)A - 400.0, UnitsHelper.Units.A);
			case 41:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B + A * byte.MaxValue), UnitsHelper.Units.Ah);
			case 42:
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 0.1 + 25.5 * (double)A - 400.0, UnitsHelper.Units.kW);
			case 43:
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 0.1 + 25.5 * (double)A, UnitsHelper.Units.volts);
			case 44:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * 60 + B), UnitsHelper.Units.minutes);
			case 45:
				return new ValueTuple<double, UnitsHelper.Units>(0.1 * (double)A * (double)B / 100.0, UnitsHelper.Units.None);
			case 46:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)(A * B) - 3200) * 0.0027, UnitsHelper.Units.grads_CS);
			case 47:
				return new ValueTuple<double, UnitsHelper.Units>((double)((B - 128) * A), UnitsHelper.Units.ms);
			case 48:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B + A * byte.MaxValue), UnitsHelper.Units.None);
			case 49:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B / 4) * 0.1 * (double)A, UnitsHelper.Units.mg_hour);
			case 50:
				if (A == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)(B - 128) / 0.01, UnitsHelper.Units.mbar);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - 128) / 0.01 * (double)A, UnitsHelper.Units.mbar);
			case 51:
				return new ValueTuple<double, UnitsHelper.Units>((double)((B - 128) / byte.MaxValue * A), UnitsHelper.Units.mg_hour);
			case 52:
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 0.02 * (double)A - (double)A, UnitsHelper.Units.Nm);
			case 53:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B - 128) * 1.4222 + 0.006 * (double)A, UnitsHelper.Units.grams_sec);
			case 54:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B), UnitsHelper.Units.None);
			case 55:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B / 200), UnitsHelper.Units.seconds);
			case 56:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B), UnitsHelper.Units.None);
			case 57:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B + 65536), UnitsHelper.Units.None);
			case 58:
				if (B > 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>(1.0225 * (double)(256 - (int)B), UnitsHelper.Units.per_second);
				}
				return new ValueTuple<double, UnitsHelper.Units>(1.0225 * (double)B, UnitsHelper.Units.per_second);
			case 59:
				return new ValueTuple<double, UnitsHelper.Units>((double)(((int)A * 256 + (int)B) / 32768), UnitsHelper.Units.None);
			case 60:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B) * 0.01, UnitsHelper.Units.seconds);
			case 61:
				if (A == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)(B - 128), UnitsHelper.Units.None);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)((B - 128) / A), UnitsHelper.Units.None);
			case 62:
				return new ValueTuple<double, UnitsHelper.Units>(0.256 * (double)A * (double)B, UnitsHelper.Units.per_second);
			case 63:
				return new ValueTuple<double, UnitsHelper.Units>((double)long.Parse(Convert.ToString(A, 2).PadLeft(8, '0') + Convert.ToString(B, 2).PadLeft(8, '0')), UnitsHelper.Units.None);
			case 64:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A + B), UnitsHelper.Units.Ohm);
			case 65:
				return new ValueTuple<double, UnitsHelper.Units>(0.01 * (double)A * (double)(B - 127), UnitsHelper.Units.mm);
			case 66:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) / 511.12, UnitsHelper.Units.volts);
			case 67:
				return new ValueTuple<double, UnitsHelper.Units>((double)(640 * (int)A) + (double)B * 2.5, UnitsHelper.Units.grads);
			case 68:
				return new ValueTuple<double, UnitsHelper.Units>((double)(256 * (int)A + (int)B) / 7.365, UnitsHelper.Units.grads_sec);
			case 69:
				return new ValueTuple<double, UnitsHelper.Units>((double)(256 * (int)A + (int)B) * 0.03254, UnitsHelper.Units.bar);
			case 70:
				return new ValueTuple<double, UnitsHelper.Units>((double)(256 * (int)A + (int)B) * 0.192, UnitsHelper.Units.m_sec2);
			case 71:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.cm);
			case 72:
				return new ValueTuple<double, UnitsHelper.Units>((double)A * 6.25 / 100.0 + (double)B * 5.17 / 100.0, UnitsHelper.Units.volts);
			case 73:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.01, UnitsHelper.Units.Ohm);
			case 74:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.1, UnitsHelper.Units.None);
			case 75:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.None);
			case 76:
				if (A > 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((int)(A + 1) * 256 + (int)B), UnitsHelper.Units.kOhm);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)B, UnitsHelper.Units.kOhm);
			case 78:
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 1.819, UnitsHelper.Units.grams_sec);
			case 79:
				return new ValueTuple<double, UnitsHelper.Units>((double)B, UnitsHelper.Units.None);
			case 80:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B) * 0.01, UnitsHelper.Units.kOhm);
			case 81:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B) * 0.04375, UnitsHelper.Units.grads);
			case 82:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B) * 98.1 / 10000.0, UnitsHelper.Units.m_sec2);
			case 83:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B) * 0.01, UnitsHelper.Units.bar);
			case 84:
				if ((A & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>(-24.909 + (double)B * 0.0973, UnitsHelper.Units.m_sec2);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 0.0973, UnitsHelper.Units.m_sec2);
			case 85:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B) * 0.00286, UnitsHelper.Units.per_second);
			case 86:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.1, UnitsHelper.Units.A);
			case 87:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.per_second);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.1, UnitsHelper.Units.per_second);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.1 - 12.8, UnitsHelper.Units.per_second);
			case 88:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.01, UnitsHelper.Units.kOhm);
			case 90:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.1, UnitsHelper.Units.kg_h);
			case 91:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.grads);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.1, UnitsHelper.Units.grads);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.1 - 12.8, UnitsHelper.Units.grads);
			case 92:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.km);
			case 93:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.Nm);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.001, UnitsHelper.Units.Nm);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.001 - 0.128, UnitsHelper.Units.Nm);
			case 94:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.Nm);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.1, UnitsHelper.Units.Nm);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.1 - 12.8, UnitsHelper.Units.Nm);
			case 95:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B), UnitsHelper.Units.None);
			case 96:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.mbar);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.1 + 12.8, UnitsHelper.Units.mbar);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.1, UnitsHelper.Units.mbar);
			case 97:
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * 5 - A * 5), UnitsHelper.Units.celicium);
			case 99:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B), UnitsHelper.Units.None);
			case 100:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.bar);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.1 + 12.8, UnitsHelper.Units.bar);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.1, UnitsHelper.Units.bar);
			case 101:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.l_per_mm);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.001 + 0.128, UnitsHelper.Units.l_per_mm);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.001, UnitsHelper.Units.l_per_mm);
			case 102:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.mm);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.1 + 12.8, UnitsHelper.Units.mm);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.1, UnitsHelper.Units.mm);
			case 103:
				return new ValueTuple<double, UnitsHelper.Units>((double)A + (double)B * 0.1, UnitsHelper.Units.volts);
			case 105:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.meters);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.01 - 1.28, UnitsHelper.Units.meters);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.01, UnitsHelper.Units.meters);
			case 106:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.kmh);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.1 - 12.8, UnitsHelper.Units.kmh);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.1, UnitsHelper.Units.kmh);
			case 107:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B), UnitsHelper.Units.None);
			case 111:
				return new ValueTuple<double, UnitsHelper.Units>((double)(72 + ((int)A * 256 + (int)B) * 256), UnitsHelper.Units.km);
			case 112:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.grads);
				}
				if ((B & 128) != 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.001 - 0.128, UnitsHelper.Units.grads);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.001, UnitsHelper.Units.grads);
			case 113:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.None);
				}
				if ((B & 128) != 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A) * 0.01 - 1.28, UnitsHelper.Units.None);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A) * 0.01, UnitsHelper.Units.None);
			case 114:
				if (A == 0 && B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.None);
				}
				if ((B & 128) != 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A - 128), UnitsHelper.Units.meters);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A), UnitsHelper.Units.None);
			case 115:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B), UnitsHelper.Units.W);
			case 116:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B), UnitsHelper.Units.per_minute);
			case 117:
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)A * 0.64, UnitsHelper.Units.celicium);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)A * -0.64, UnitsHelper.Units.celicium);
			case 119:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.01, UnitsHelper.Units.percent);
			case 120:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 1.412, UnitsHelper.Units.grads);
			case 121:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)B * 256 + (int)A) * 0.5, UnitsHelper.Units.None);
			case 122:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)B * 256 + (int)A) * 0.01 - 327.68, UnitsHelper.Units.None);
			case 124:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.1, UnitsHelper.Units.mA);
			case 125:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A - B), UnitsHelper.Units.db);
			case 126:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A - B), UnitsHelper.Units.None);
			case 128:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.per_minute);
			case 129:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.00391, UnitsHelper.Units.percent);
			case 130:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.0003906, UnitsHelper.Units.A);
			case 132:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.5, UnitsHelper.Units.grads);
			case 133:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 39.06 / 10000.0, UnitsHelper.Units.volts);
			case 134:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.kmh);
			case 135:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.None);
			case 136:
				return new ValueTuple<double, UnitsHelper.Units>((double)long.Parse(Convert.ToString(B, 2).PadLeft(8, '0')), UnitsHelper.Units.None);
			case 137:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.01, UnitsHelper.Units.ms);
			case 138:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.001, UnitsHelper.Units.volts);
			case 143:
				if (A == 0 || B == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.grads);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A / 100), UnitsHelper.Units.grads);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A / 100) - 12.8, UnitsHelper.Units.grads);
			case 144:
				if (B == 0 || A == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.Lh);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A / 100) + 12.8, UnitsHelper.Units.Lh);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A / 100), UnitsHelper.Units.Lh);
			case 145:
				if (B == 0 || A == 0)
				{
					return new ValueTuple<double, UnitsHelper.Units>(0.0, UnitsHelper.Units.liters100km);
				}
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 127) * A / 100) + 12.8, UnitsHelper.Units.liters100km);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(B * A / 100), UnitsHelper.Units.liters100km);
			case 147:
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)(byte.MaxValue - A), UnitsHelper.Units.percent);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(0 - A), UnitsHelper.Units.percent);
			case 148:
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)((B & 15) * 2), UnitsHelper.Units.None);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)(0 - A * 32), UnitsHelper.Units.None);
			case 149:
			{
				if ((B & 128) == 128)
				{
					int num3 = (int)(B & 15);
					return new ValueTuple<double, UnitsHelper.Units>((double)A * 2.8 + 0.4 * (double)num3, UnitsHelper.Units.celicium);
				}
				int num4 = (int)(B & 15);
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * -10 + num4), UnitsHelper.Units.celicium);
			}
			case 150:
			{
				if ((B & 128) == 128)
				{
					int num5 = (int)(B & 15);
					return new ValueTuple<double, UnitsHelper.Units>((double)A * 0.0056 + (double)num5 * 1.42, UnitsHelper.Units.grams_sec);
				}
				int num6 = (int)(B & 15);
				return new ValueTuple<double, UnitsHelper.Units>(182.04 + (double)A * 0.0056 + (double)num6 * 1.42, UnitsHelper.Units.grams_sec);
			}
			case 151:
			{
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.01, UnitsHelper.Units.grads_CS);
				}
				int num7 = (int)(B & 15);
				return new ValueTuple<double, UnitsHelper.Units>((double)A * 1.28 + (double)num7 * 0.02, UnitsHelper.Units.grads_CS);
			}
			case 152:
			{
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)B * 0.025 + (double)A * 3.2, UnitsHelper.Units.mg);
				}
				int num8 = (int)(B & 15);
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * num8) * 0.025, UnitsHelper.Units.mg);
			}
			case 153:
				if ((B & 128) == 128)
				{
					return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.004, UnitsHelper.Units.mg);
				}
				return new ValueTuple<double, UnitsHelper.Units>((double)A * -0.5 + (double)B * 0.0078, UnitsHelper.Units.mg);
			case 154:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.grads);
			case 155:
				return new ValueTuple<double, UnitsHelper.Units>((double)A * 2.55 - 90.0, UnitsHelper.Units.grads);
			case 156:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B), UnitsHelper.Units.cm);
			case 157:
				return new ValueTuple<double, UnitsHelper.Units>(VWTPFormulaManager.ShortSigned(A, B), UnitsHelper.Units.cm);
			case 158:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)(B * 10) / 1000), UnitsHelper.Units.kmh);
			case 159:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)B * 256 + (int)A) * 0.1 - 3251.2, UnitsHelper.Units.celicium);
			case 162:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.448, UnitsHelper.Units.grads);
			case 163:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * 60 + B), UnitsHelper.Units.minutes);
			case 164:
				return new ValueTuple<double, UnitsHelper.Units>((double)B, UnitsHelper.Units.percent);
			case 165:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)B * 256 + (int)A - 32768), UnitsHelper.Units.mA);
			case 166:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 640) + (double)B * 2.5, UnitsHelper.Units.grads_sec);
			case 167:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)(A * B * 10) / 10000), UnitsHelper.Units.mOhm);
			case 168:
				return new ValueTuple<double, UnitsHelper.Units>((double)B * 0.01 + (double)A * 2.56, UnitsHelper.Units.percent);
			case 169:
				return new ValueTuple<double, UnitsHelper.Units>((double)(200 + A * B), UnitsHelper.Units.mV);
			case 170:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 3.91 / 10000.0, UnitsHelper.Units.gramms);
			case 171:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 39.1 / 10000.0, UnitsHelper.Units.gramms);
			case 172:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B), UnitsHelper.Units.mg_km);
			case 173:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.1, UnitsHelper.Units.mg_sec);
			case 174:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.001, UnitsHelper.Units.liters100km);
			case 175:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.5 * 0.01, UnitsHelper.Units.bar);
			case 176:
				return new ValueTuple<double, UnitsHelper.Units>((double)(A * B) * 0.1, UnitsHelper.Units.ppm);
			case 177:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 256 + (int)B - 32768), UnitsHelper.Units.Nm);
			case 178:
				return new ValueTuple<double, UnitsHelper.Units>((double)A * 30.2 + (double)B * 0.118 - 3855.1, UnitsHelper.Units.grads_sec);
			case 179:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)A * 2560 + (int)(B * 10)), UnitsHelper.Units.None);
			case 180:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)(A * B * 156) / 40000), UnitsHelper.Units.kg_h);
			case 181:
				return new ValueTuple<double, UnitsHelper.Units>((double)((int)(A * B) / 1000), UnitsHelper.Units.mg_sec);
			}
			return new ValueTuple<double, UnitsHelper.Units>((double)long.Parse(formulaId.ToString("000") + A.ToString("000") + B.ToString("000")), UnitsHelper.Units.None);
		}
	}
}
