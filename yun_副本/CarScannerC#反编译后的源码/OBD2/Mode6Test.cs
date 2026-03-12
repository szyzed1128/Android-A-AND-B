using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000326 RID: 806
	public class Mode6Test
	{
		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x0600248C RID: 9356 RVA: 0x001BEF94 File Offset: 0x001BD194
		public bool IsPro
		{
			get
			{
				return SharedSettings.Current.AdsProductPurchased;
			}
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x001BEFA0 File Offset: 0x001BD1A0
		public bool IsFree
		{
			get
			{
				return !SharedSettings.Current.AdsProductPurchased;
			}
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x001BEFAF File Offset: 0x001BD1AF
		// (set) Token: 0x0600248F RID: 9359 RVA: 0x001BEFB7 File Offset: 0x001BD1B7
		public bool TestPassed
		{
			get
			{
				return this._TestPassed;
			}
			private set
			{
				this._TestPassed = value;
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x001BEFC0 File Offset: 0x001BD1C0
		public bool TestNotPassed
		{
			get
			{
				return !this.TestPassed;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x001BEFCB File Offset: 0x001BD1CB
		public Color TestColor
		{
			get
			{
				if (!this.TestPassed)
				{
					return Color.Red;
				}
				return Color.Black;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x001BEFE0 File Offset: 0x001BD1E0
		// (set) Token: 0x06002493 RID: 9363 RVA: 0x001BEFE8 File Offset: 0x001BD1E8
		public string MonitorName
		{
			[CompilerGenerated]
			get
			{
				return this.<MonitorName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MonitorName>k__BackingField = value;
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x001BEFF1 File Offset: 0x001BD1F1
		// (set) Token: 0x06002495 RID: 9365 RVA: 0x001BEFF9 File Offset: 0x001BD1F9
		public string TestId
		{
			[CompilerGenerated]
			get
			{
				return this.<TestId>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TestId>k__BackingField = value;
			}
		}

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x001BF002 File Offset: 0x001BD202
		// (set) Token: 0x06002497 RID: 9367 RVA: 0x001BF00A File Offset: 0x001BD20A
		public double TestValue
		{
			[CompilerGenerated]
			get
			{
				return this.<TestValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TestValue>k__BackingField = value;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06002498 RID: 9368 RVA: 0x001BF013 File Offset: 0x001BD213
		// (set) Token: 0x06002499 RID: 9369 RVA: 0x001BF01B File Offset: 0x001BD21B
		public double MinValue
		{
			[CompilerGenerated]
			get
			{
				return this.<MinValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MinValue>k__BackingField = value;
			}
		}

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x001BF024 File Offset: 0x001BD224
		// (set) Token: 0x0600249B RID: 9371 RVA: 0x001BF02C File Offset: 0x001BD22C
		public double MaxValue
		{
			[CompilerGenerated]
			get
			{
				return this.<MaxValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MaxValue>k__BackingField = value;
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x001BF035 File Offset: 0x001BD235
		// (set) Token: 0x0600249D RID: 9373 RVA: 0x001BF03D File Offset: 0x001BD23D
		public int ValuePosition
		{
			[CompilerGenerated]
			get
			{
				return this.<ValuePosition>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ValuePosition>k__BackingField = value;
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x001BF046 File Offset: 0x001BD246
		// (set) Token: 0x0600249F RID: 9375 RVA: 0x001BF04E File Offset: 0x001BD24E
		public string Cmd
		{
			[CompilerGenerated]
			get
			{
				return this.<Cmd>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Cmd>k__BackingField = value;
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x001BF057 File Offset: 0x001BD257
		// (set) Token: 0x060024A1 RID: 9377 RVA: 0x001BF05F File Offset: 0x001BD25F
		public bool IsCan
		{
			[CompilerGenerated]
			get
			{
				return this.<IsCan>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsCan>k__BackingField = value;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x001BF068 File Offset: 0x001BD268
		// (set) Token: 0x060024A3 RID: 9379 RVA: 0x001BF070 File Offset: 0x001BD270
		public int TestType
		{
			[CompilerGenerated]
			get
			{
				return this.<TestType>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<TestType>k__BackingField = value;
			}
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x001BF079 File Offset: 0x001BD279
		// (set) Token: 0x060024A5 RID: 9381 RVA: 0x001BF081 File Offset: 0x001BD281
		public UnitsHelper.Units Units
		{
			[CompilerGenerated]
			get
			{
				return this.<Units>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Units>k__BackingField = value;
			}
		}

		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x060024A6 RID: 9382 RVA: 0x001BF08A File Offset: 0x001BD28A
		public string UnitsValue
		{
			get
			{
				return UnitsHelper.GetCaption(this.Units);
			}
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x001BF098 File Offset: 0x001BD298
		public void CheckTestPassed()
		{
			if (this.MinValue <= this.MaxValue)
			{
				if (this.TestValue >= this.MinValue && this.TestValue <= this.MaxValue)
				{
					this.TestPassed = true;
					return;
				}
				this.TestPassed = false;
				return;
			}
			else
			{
				if (this.TestValue >= this.MinValue)
				{
					this.TestPassed = true;
					return;
				}
				this.TestPassed = false;
				return;
			}
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x001BF0FC File Offset: 0x001BD2FC
		public static List<Mode6Test> DecodeFromBytes(string cmd, byte[] data, bool IsCan)
		{
			List<Mode6Test> list = new List<Mode6Test>();
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				if (IsCan)
				{
					while (memoryStream.Position + 9L <= memoryStream.Length)
					{
						Mode6Test mode6Test = Mode6Test.DecodeCANFromStream(memoryStream, cmd);
						list.Add(mode6Test);
					}
				}
				else
				{
					while (memoryStream.Position + 6L <= memoryStream.Length)
					{
						Mode6Test mode6Test2 = Mode6Test.DecodeSAEKWPFromStream(memoryStream, cmd);
						list.Add(mode6Test2);
					}
				}
			}
			return list;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x001BF17C File Offset: 0x001BD37C
		public static Mode6Test DecodeSAEKWPFromStream(Stream stream, string cmd)
		{
			Mode6Test mode6Test = new Mode6Test();
			mode6Test.Cmd = cmd;
			mode6Test.IsCan = false;
			int num = stream.ReadByte();
			int num2 = stream.ReadByte();
			byte[] array = new byte[2];
			mode6Test.ValuePosition = (int)stream.Position;
			stream.Read(array, 0, array.Length);
			byte[] array2 = new byte[2];
			stream.Read(array2, 0, array2.Length);
			int num3 = (int)BitHelpers.SetBit((byte)num2, 7, false);
			switch (num)
			{
			case 1:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 2:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 3:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 4:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 5:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 6:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 7:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 8:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 9:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 10:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 11:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 12:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			default:
				mode6Test.TestId = Translate.GetString("m6t_default");
				break;
			}
			Mode6Test mode6Test2 = mode6Test;
			mode6Test2.TestId = mode6Test2.TestId + Translate.GetString("m6t_MID") + cmd.Substring(2);
			mode6Test.MonitorName = mode6Test.TestId;
			mode6Test.TestId = Translate.GetString("m6t_ComponentId") + num3.ToString("X2", CultureInfo.InvariantCulture);
			if (num2 >= 128)
			{
				mode6Test.MaxValue = 0.0;
				mode6Test.MinValue = (double)((int)array2[0] * 256 + (int)array2[1]);
			}
			else
			{
				mode6Test.MinValue = 0.0;
				mode6Test.MaxValue = (double)((int)array2[0] * 256 + (int)array2[1]);
			}
			mode6Test.TestValue = (double)((int)array[0] * 256 + (int)array[1]);
			mode6Test.CheckTestPassed();
			return mode6Test;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x001BF47C File Offset: 0x001BD67C
		public static Mode6Test DecodeCANFromStream(Stream stream, string cmd)
		{
			Mode6Test mode6Test = new Mode6Test();
			mode6Test.Cmd = cmd;
			stream.ReadByte();
			int num = stream.ReadByte();
			int num2 = stream.ReadByte();
			byte[] array = new byte[2];
			mode6Test.ValuePosition = (int)stream.Position;
			stream.Read(array, 0, array.Length);
			byte[] array2 = new byte[2];
			stream.Read(array2, 0, array2.Length);
			byte[] array3 = new byte[2];
			stream.Read(array3, 0, array3.Length);
			mode6Test.IsCan = true;
			mode6Test.TestType = num2;
			mode6Test.MonitorName = Mode6Test.GetMonitorNameFromCommand(cmd);
			switch (num)
			{
			case 1:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 2:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 3:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 4:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 5:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 6:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 7:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 8:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 9:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 10:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 11:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			case 12:
				mode6Test.TestId = Translate.GetString("m6t_" + num.ToString());
				break;
			default:
				mode6Test.TestId = Translate.GetString("m6t_default");
				break;
			}
			Mode6Test mode6Test2 = mode6Test;
			mode6Test2.TestId = mode6Test2.TestId + Translate.GetString("m6t_TID") + num.ToString("X2", CultureInfo.InvariantCulture);
			if (num2 < 128)
			{
				mode6Test.TestValue = (double)((int)array[0] * 256 + (int)array[1]);
				mode6Test.MinValue = (double)((int)array2[0] * 256 + (int)array2[1]);
				mode6Test.MaxValue = (double)((int)array3[0] * 256 + (int)array3[1]);
				switch (num2)
				{
				case 1:
					mode6Test.Units = UnitsHelper.Units.None;
					break;
				case 2:
					mode6Test.Units = UnitsHelper.Units.None;
					mode6Test.TestValue /= 10.0;
					mode6Test.MinValue /= 10.0;
					mode6Test.MaxValue /= 10.0;
					break;
				case 3:
					mode6Test.Units = UnitsHelper.Units.None;
					mode6Test.TestValue /= 100.0;
					mode6Test.MinValue /= 100.0;
					mode6Test.MaxValue /= 100.0;
					break;
				case 4:
					mode6Test.Units = UnitsHelper.Units.None;
					mode6Test.TestValue /= 1000.0;
					mode6Test.MinValue /= 1000.0;
					mode6Test.MaxValue /= 1000.0;
					break;
				case 5:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num3 = 1.999;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num3);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num3);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num3);
					break;
				}
				case 6:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num4 = 19.988;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num4);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num4);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num4);
					break;
				}
				case 7:
				{
					mode6Test.Units = UnitsHelper.Units.rpm;
					double num5 = 16383.75;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num5);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num5);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num5);
					break;
				}
				case 8:
				{
					mode6Test.Units = UnitsHelper.Units.kmh;
					double num6 = 655.35;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num6);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num6);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num6);
					break;
				}
				case 9:
					mode6Test.Units = UnitsHelper.Units.kmh;
					break;
				case 10:
				{
					mode6Test.Units = UnitsHelper.Units.volts;
					double num7 = 7.995;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num7);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num7);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num7);
					break;
				}
				case 11:
				{
					mode6Test.Units = UnitsHelper.Units.volts;
					double num8 = 65.535;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num8);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num8);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num8);
					break;
				}
				case 12:
				{
					mode6Test.Units = UnitsHelper.Units.volts;
					double num9 = 655.35;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num9);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num9);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num9);
					break;
				}
				case 13:
				{
					mode6Test.Units = UnitsHelper.Units.mA;
					double num10 = 255.996;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num10);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num10);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num10);
					break;
				}
				case 14:
				{
					mode6Test.Units = UnitsHelper.Units.mA;
					double num11 = 65535.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num11);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num11);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num11);
					break;
				}
				case 15:
				{
					mode6Test.Units = UnitsHelper.Units.mA;
					double num12 = 655350.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num12);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num12);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num12);
					break;
				}
				case 16:
				{
					mode6Test.Units = UnitsHelper.Units.seconds;
					double num13 = 65.535;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num13);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num13);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num13);
					break;
				}
				case 17:
				{
					mode6Test.Units = UnitsHelper.Units.seconds;
					double num14 = 6553.5;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num14);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num14);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num14);
					break;
				}
				case 18:
				{
					mode6Test.Units = UnitsHelper.Units.seconds;
					double num15 = 65.535;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num15);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num15);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num15);
					break;
				}
				case 19:
				{
					mode6Test.Units = UnitsHelper.Units.Ohm;
					double num16 = 65.535;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num16);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num16);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num16);
					break;
				}
				case 20:
				{
					mode6Test.Units = UnitsHelper.Units.kOhm;
					double num17 = 65.535;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num17);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num17);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num17);
					break;
				}
				case 21:
				{
					mode6Test.Units = UnitsHelper.Units.MOhm;
					double num18 = 65.535;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num18);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num18);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num18);
					break;
				}
				case 22:
				{
					mode6Test.Units = UnitsHelper.Units.celicium;
					double num19 = 6553.5;
					double num20 = -40.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num19, num20);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num19, num20);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num19, num20);
					break;
				}
				case 23:
				{
					mode6Test.Units = UnitsHelper.Units.kPa;
					double num21 = 655.35;
					double num22 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num21, num22);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num21, num22);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num21, num22);
					break;
				}
				case 24:
				{
					mode6Test.Units = UnitsHelper.Units.kPa;
					double num23 = 766.7595;
					double num24 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num23, num24);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num23, num24);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num23, num24);
					break;
				}
				case 25:
				{
					mode6Test.Units = UnitsHelper.Units.kPa;
					double num25 = 5177.265;
					double num26 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num25, num26);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num25, num26);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num25, num26);
					break;
				}
				case 26:
				{
					mode6Test.Units = UnitsHelper.Units.kPa;
					double num27 = 65535.0;
					double num28 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num27, num28);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num27, num28);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num27, num28);
					break;
				}
				case 27:
				{
					mode6Test.Units = UnitsHelper.Units.kPa;
					double num29 = 655350.0;
					double num30 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num29, num30);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num29, num30);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num29, num30);
					break;
				}
				case 28:
				{
					mode6Test.Units = UnitsHelper.Units.grads;
					double num31 = 655.35;
					double num32 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num31, num32);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num31, num32);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num31, num32);
					break;
				}
				case 29:
				{
					mode6Test.Units = UnitsHelper.Units.grads;
					double num33 = 32767.5;
					double num34 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num33, num34);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num33, num34);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num33, num34);
					break;
				}
				case 30:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num35 = 1.999;
					double num36 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num35, num36);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num35, num36);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num35, num36);
					break;
				}
				case 31:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num37 = 32767.75;
					double num38 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num37, num38);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num37, num38);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num37, num38);
					break;
				}
				case 32:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num39 = 255.993;
					double num40 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num39, num40);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num39, num40);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num39, num40);
					break;
				}
				case 33:
				{
					mode6Test.Units = UnitsHelper.Units.Hz;
					double num41 = 65.535;
					double num42 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num41, num42);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num41, num42);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num41, num42);
					break;
				}
				case 34:
				{
					mode6Test.Units = UnitsHelper.Units.Hz;
					double num43 = 65535.0;
					double num44 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num43, num44);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num43, num44);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num43, num44);
					break;
				}
				case 35:
				{
					mode6Test.Units = UnitsHelper.Units.MHz;
					double num45 = 65.535;
					double num46 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num45, num46);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num45, num46);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num45, num46);
					break;
				}
				case 36:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num47 = 65535.0;
					double num48 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num47, num48);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num47, num48);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num47, num48);
					break;
				}
				case 37:
				{
					mode6Test.Units = UnitsHelper.Units.km;
					double num49 = 65535.0;
					double num50 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num49, num50);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num49, num50);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num49, num50);
					break;
				}
				case 38:
				{
					mode6Test.Units = UnitsHelper.Units.Vms;
					double num51 = 6.5535;
					double num52 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num51, num52);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num51, num52);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num51, num52);
					break;
				}
				case 39:
				{
					mode6Test.Units = UnitsHelper.Units.grams_sec;
					double num53 = 655.35;
					double num54 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num53, num54);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num53, num54);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num53, num54);
					break;
				}
				case 40:
				{
					mode6Test.Units = UnitsHelper.Units.grams_sec;
					double num55 = 65535.0;
					double num56 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num55, num56);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num55, num56);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num55, num56);
					break;
				}
				case 41:
				{
					mode6Test.Units = UnitsHelper.Units.Pa_sec;
					double num57 = 16384.0;
					double num58 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num57, num58);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num57, num58);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num57, num58);
					break;
				}
				case 42:
				{
					mode6Test.Units = UnitsHelper.Units.kg_h;
					double num59 = 65.535;
					double num60 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num59, num60);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num59, num60);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num59, num60);
					break;
				}
				case 43:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num61 = 65535.0;
					double num62 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num61, num62);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num61, num62);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num61, num62);
					break;
				}
				case 44:
				{
					mode6Test.Units = UnitsHelper.Units.g_cyl;
					double num63 = 655.35;
					double num64 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num63, num64);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num63, num64);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num63, num64);
					break;
				}
				case 45:
				{
					mode6Test.Units = UnitsHelper.Units.g_stroke;
					double num65 = 655.35;
					double num66 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num65, num66);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num65, num66);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num65, num66);
					break;
				}
				case 46:
					mode6Test.Units = UnitsHelper.Units.None;
					break;
				case 47:
				{
					mode6Test.Units = UnitsHelper.Units.percent;
					double num67 = 655.35;
					double num68 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num67, num68);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num67, num68);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num67, num68);
					break;
				}
				case 48:
				{
					mode6Test.Units = UnitsHelper.Units.percent;
					double num69 = 100.00641;
					double num70 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num69, num70);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num69, num70);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num69, num70);
					break;
				}
				case 49:
				{
					mode6Test.Units = UnitsHelper.Units.liters;
					double num71 = 65.535;
					double num72 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num71, num72);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num71, num72);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num71, num72);
					break;
				}
				case 50:
				{
					mode6Test.Units = UnitsHelper.Units.mm;
					double num73 = 50.77;
					double num74 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num73, num74);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num73, num74);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num73, num74);
					break;
				}
				case 51:
				{
					mode6Test.Units = UnitsHelper.Units.None;
					double num75 = 16.0;
					double num76 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num75, num76);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num75, num76);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num75, num76);
					break;
				}
				case 52:
				{
					mode6Test.Units = UnitsHelper.Units.minutes;
					double num77 = 65535.0;
					double num78 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num77, num78);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num77, num78);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num77, num78);
					break;
				}
				case 53:
				{
					mode6Test.Units = UnitsHelper.Units.seconds;
					double num79 = 655.35;
					double num80 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num79, num80);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num79, num80);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num79, num80);
					break;
				}
				case 54:
				{
					mode6Test.Units = UnitsHelper.Units.gramms;
					double num81 = 655.35;
					double num82 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num81, num82);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num81, num82);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num81, num82);
					break;
				}
				case 55:
				{
					mode6Test.Units = UnitsHelper.Units.gramms;
					double num83 = 6553.5;
					double num84 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num83, num84);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num83, num84);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num83, num84);
					break;
				}
				case 56:
				{
					mode6Test.Units = UnitsHelper.Units.gramms;
					double num85 = 65535.0;
					double num86 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num85, num86);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num85, num86);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num85, num86);
					break;
				}
				case 57:
				{
					mode6Test.Units = UnitsHelper.Units.percent;
					double num87 = 327.67;
					double num88 = 0.0;
					mode6Test.TestValue = Mode6Test.Calculate(mode6Test.TestValue, num87, num88);
					mode6Test.MinValue = Mode6Test.Calculate(mode6Test.MinValue, num87, num88);
					mode6Test.MaxValue = Mode6Test.Calculate(mode6Test.MaxValue, num87, num88);
					break;
				}
				default:
					mode6Test.Units = UnitsHelper.Units.None;
					break;
				}
			}
			else
			{
				mode6Test.TestValue = (double)Mode6Test.BytesToShort(array);
				mode6Test.MinValue = (double)Mode6Test.BytesToShort(array2);
				mode6Test.MaxValue = (double)Mode6Test.BytesToShort(array3);
				if (num2 <= 169)
				{
					switch (num2)
					{
					case 129:
						mode6Test.Units = UnitsHelper.Units.None;
						goto IL_1A64;
					case 130:
						mode6Test.Units = UnitsHelper.Units.None;
						Mode6Test.FixSigned(mode6Test, 0.1);
						goto IL_1A64;
					case 131:
						mode6Test.Units = UnitsHelper.Units.None;
						Mode6Test.FixSigned(mode6Test, 0.01);
						goto IL_1A64;
					case 132:
						mode6Test.Units = UnitsHelper.Units.None;
						Mode6Test.FixSigned(mode6Test, 0.001);
						goto IL_1A64;
					case 133:
						mode6Test.Units = UnitsHelper.Units.None;
						Mode6Test.FixSigned(mode6Test, 3.05E-05);
						goto IL_1A64;
					case 134:
						mode6Test.Units = UnitsHelper.Units.None;
						Mode6Test.FixSigned(mode6Test, 0.000305);
						goto IL_1A64;
					case 135:
					case 136:
					case 137:
					case 143:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 151:
					case 152:
					case 153:
					case 154:
					case 155:
						break;
					case 138:
						mode6Test.Units = UnitsHelper.Units.volts;
						Mode6Test.FixSigned(mode6Test, 122.0);
						goto IL_1A64;
					case 139:
						mode6Test.Units = UnitsHelper.Units.volts;
						Mode6Test.FixSigned(mode6Test, 0.001);
						goto IL_1A64;
					case 140:
						mode6Test.Units = UnitsHelper.Units.volts;
						Mode6Test.FixSigned(mode6Test, 0.01);
						goto IL_1A64;
					case 141:
						mode6Test.Units = UnitsHelper.Units.mA;
						Mode6Test.FixSigned(mode6Test, 0.00390625);
						goto IL_1A64;
					case 142:
						mode6Test.Units = UnitsHelper.Units.mA;
						Mode6Test.FixSigned(mode6Test, 1E-06);
						goto IL_1A64;
					case 144:
						mode6Test.Units = UnitsHelper.Units.ms;
						goto IL_1A64;
					case 150:
						mode6Test.Units = UnitsHelper.Units.celicium;
						Mode6Test.FixSigned(mode6Test, 0.1);
						goto IL_1A64;
					case 156:
						mode6Test.Units = UnitsHelper.Units.grads;
						Mode6Test.FixSigned(mode6Test, 0.01);
						goto IL_1A64;
					case 157:
						mode6Test.Units = UnitsHelper.Units.grads;
						Mode6Test.FixSigned(mode6Test, 0.5);
						goto IL_1A64;
					default:
						if (num2 == 168)
						{
							mode6Test.Units = UnitsHelper.Units.grams_sec;
							goto IL_1A64;
						}
						if (num2 == 169)
						{
							mode6Test.Units = UnitsHelper.Units.Pa_sec;
							Mode6Test.FixSigned(mode6Test, 0.25);
							goto IL_1A64;
						}
						break;
					}
				}
				else
				{
					switch (num2)
					{
					case 175:
						mode6Test.Units = UnitsHelper.Units.percent;
						Mode6Test.FixSigned(mode6Test, 0.01);
						goto IL_1A64;
					case 176:
						mode6Test.Units = UnitsHelper.Units.percent;
						Mode6Test.FixSigned(mode6Test, 0.003052);
						goto IL_1A64;
					case 177:
						mode6Test.Units = UnitsHelper.Units.mV_sec;
						Mode6Test.FixSigned(mode6Test, 2.0);
						goto IL_1A64;
					default:
						if (num2 == 253)
						{
							mode6Test.Units = UnitsHelper.Units.kPa;
							Mode6Test.FixSigned(mode6Test, 0.001);
							goto IL_1A64;
						}
						if (num2 == 254)
						{
							mode6Test.Units = UnitsHelper.Units.Pa;
							Mode6Test.FixSigned(mode6Test, 0.25);
							goto IL_1A64;
						}
						break;
					}
				}
				mode6Test.Units = UnitsHelper.Units.None;
			}
			IL_1A64:
			mode6Test.CheckTestPassed();
			return mode6Test;
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x001C0EF4 File Offset: 0x001BF0F4
		private static double Calculate(double value, double max)
		{
			return value * max / 65535.0;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x001C0F03 File Offset: 0x001BF103
		private static void FixSigned(Mode6Test m6t, double scale)
		{
			m6t.TestValue *= scale;
			m6t.MinValue *= scale;
			m6t.MaxValue *= scale;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x001C0F30 File Offset: 0x001BF130
		private static double Calculate(double value, double max, double min)
		{
			double num = Math.Abs(min) + Math.Abs(max);
			return value * num / 65535.0;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x001C0F58 File Offset: 0x001BF158
		private static void FillDictionary()
		{
			Mode6Test.MonitorNames.Add("01", Translate.GetString("m6t_dict_01"));
			Mode6Test.MonitorNames.Add("02", Translate.GetString("m6t_dict_02"));
			Mode6Test.MonitorNames.Add("03", Translate.GetString("m6t_dict_03"));
			Mode6Test.MonitorNames.Add("04", Translate.GetString("m6t_dict_04"));
			Mode6Test.MonitorNames.Add("05", Translate.GetString("m6t_dict_05"));
			Mode6Test.MonitorNames.Add("06", Translate.GetString("m6t_dict_06"));
			Mode6Test.MonitorNames.Add("07", Translate.GetString("m6t_dict_07"));
			Mode6Test.MonitorNames.Add("08", Translate.GetString("m6t_dict_08"));
			Mode6Test.MonitorNames.Add("09", Translate.GetString("m6t_dict_09"));
			Mode6Test.MonitorNames.Add("0A", Translate.GetString("m6t_dict_0A"));
			Mode6Test.MonitorNames.Add("0B", Translate.GetString("m6t_dict_0B"));
			Mode6Test.MonitorNames.Add("0C", Translate.GetString("m6t_dict_0C"));
			Mode6Test.MonitorNames.Add("0D", Translate.GetString("m6t_dict_0D"));
			Mode6Test.MonitorNames.Add("0E", Translate.GetString("m6t_dict_0E"));
			Mode6Test.MonitorNames.Add("0F", Translate.GetString("m6t_dict_0F"));
			Mode6Test.MonitorNames.Add("10", Translate.GetString("m6t_dict_10"));
			Mode6Test.MonitorNames.Add("21", Translate.GetString("m6t_dict_21"));
			Mode6Test.MonitorNames.Add("22", Translate.GetString("m6t_dict_22"));
			Mode6Test.MonitorNames.Add("23", Translate.GetString("m6t_dict_23"));
			Mode6Test.MonitorNames.Add("24", Translate.GetString("m6t_dict_24"));
			Mode6Test.MonitorNames.Add("31", Translate.GetString("m6t_dict_31"));
			Mode6Test.MonitorNames.Add("32", Translate.GetString("m6t_dict_32"));
			Mode6Test.MonitorNames.Add("33", Translate.GetString("m6t_dict_33"));
			Mode6Test.MonitorNames.Add("34", Translate.GetString("m6t_dict_34"));
			Mode6Test.MonitorNames.Add("35", Translate.GetString("m6t_dict_35"));
			Mode6Test.MonitorNames.Add("36", Translate.GetString("m6t_dict_36"));
			Mode6Test.MonitorNames.Add("37", Translate.GetString("m6t_dict_37"));
			Mode6Test.MonitorNames.Add("38", Translate.GetString("m6t_dict_38"));
			Mode6Test.MonitorNames.Add("39", Translate.GetString("m6t_dict_39"));
			Mode6Test.MonitorNames.Add("3A", Translate.GetString("m6t_dict_3A"));
			Mode6Test.MonitorNames.Add("3B", Translate.GetString("m6t_dict_3B"));
			Mode6Test.MonitorNames.Add("3C", Translate.GetString("m6t_dict_3C"));
			Mode6Test.MonitorNames.Add("3D", Translate.GetString("m6t_dict_3D"));
			Mode6Test.MonitorNames.Add("41", Translate.GetString("m6t_dict_41"));
			Mode6Test.MonitorNames.Add("42", Translate.GetString("m6t_dict_42"));
			Mode6Test.MonitorNames.Add("43", Translate.GetString("m6t_dict_43"));
			Mode6Test.MonitorNames.Add("44", Translate.GetString("m6t_dict_44"));
			Mode6Test.MonitorNames.Add("45", Translate.GetString("m6t_dict_45"));
			Mode6Test.MonitorNames.Add("46", Translate.GetString("m6t_dict_46"));
			Mode6Test.MonitorNames.Add("47", Translate.GetString("m6t_dict_47"));
			Mode6Test.MonitorNames.Add("48", Translate.GetString("m6t_dict_48"));
			Mode6Test.MonitorNames.Add("49", Translate.GetString("m6t_dict_49"));
			Mode6Test.MonitorNames.Add("4A", Translate.GetString("m6t_dict_4A"));
			Mode6Test.MonitorNames.Add("4B", Translate.GetString("m6t_dict_4B"));
			Mode6Test.MonitorNames.Add("4C", Translate.GetString("m6t_dict_4C"));
			Mode6Test.MonitorNames.Add("4D", Translate.GetString("m6t_dict_4D"));
			Mode6Test.MonitorNames.Add("4E", Translate.GetString("m6t_dict_4E"));
			Mode6Test.MonitorNames.Add("4F", Translate.GetString("m6t_dict_4F"));
			Mode6Test.MonitorNames.Add("50", Translate.GetString("m6t_dict_50"));
			Mode6Test.MonitorNames.Add("61", Translate.GetString("m6t_dict_61"));
			Mode6Test.MonitorNames.Add("62", Translate.GetString("m6t_dict_62"));
			Mode6Test.MonitorNames.Add("63", Translate.GetString("m6t_dict_63"));
			Mode6Test.MonitorNames.Add("64", Translate.GetString("m6t_dict_64"));
			Mode6Test.MonitorNames.Add("71", Translate.GetString("m6t_dict_71"));
			Mode6Test.MonitorNames.Add("72", Translate.GetString("m6t_dict_72"));
			Mode6Test.MonitorNames.Add("73", Translate.GetString("m6t_dict_73"));
			Mode6Test.MonitorNames.Add("74", Translate.GetString("m6t_dict_74"));
			Mode6Test.MonitorNames.Add("81", Translate.GetString("m6t_dict_81"));
			Mode6Test.MonitorNames.Add("82", Translate.GetString("m6t_dict_82"));
			Mode6Test.MonitorNames.Add("83", Translate.GetString("m6t_dict_83"));
			Mode6Test.MonitorNames.Add("84", Translate.GetString("m6t_dict_84"));
			Mode6Test.MonitorNames.Add("85", Translate.GetString("m6t_dict_85"));
			Mode6Test.MonitorNames.Add("86", Translate.GetString("m6t_dict_86"));
			Mode6Test.MonitorNames.Add("90", Translate.GetString("m6t_dict_90"));
			Mode6Test.MonitorNames.Add("91", Translate.GetString("m6t_dict_91"));
			Mode6Test.MonitorNames.Add("98", Translate.GetString("m6t_dict_98"));
			Mode6Test.MonitorNames.Add("99", Translate.GetString("m6t_dict_99"));
			Mode6Test.MonitorNames.Add("A1", Translate.GetString("m6t_dict_A1"));
			Mode6Test.MonitorNames.Add("A2", Translate.GetString("m6t_dict_A2"));
			Mode6Test.MonitorNames.Add("A3", Translate.GetString("m6t_dict_A3"));
			Mode6Test.MonitorNames.Add("A4", Translate.GetString("m6t_dict_A4"));
			Mode6Test.MonitorNames.Add("A5", Translate.GetString("m6t_dict_A5"));
			Mode6Test.MonitorNames.Add("A6", Translate.GetString("m6t_dict_A6"));
			Mode6Test.MonitorNames.Add("A7", Translate.GetString("m6t_dict_A7"));
			Mode6Test.MonitorNames.Add("A8", Translate.GetString("m6t_dict_A8"));
			Mode6Test.MonitorNames.Add("A9", Translate.GetString("m6t_dict_A9"));
			Mode6Test.MonitorNames.Add("AA", Translate.GetString("m6t_dict_AA"));
			Mode6Test.MonitorNames.Add("AB", Translate.GetString("m6t_dict_AB"));
			Mode6Test.MonitorNames.Add("AC", Translate.GetString("m6t_dict_AC"));
			Mode6Test.MonitorNames.Add("AD", Translate.GetString("m6t_dict_AD"));
			Mode6Test.MonitorNames.Add("B0", Translate.GetString("m6t_dict_B0"));
			Mode6Test.MonitorNames.Add("B1", Translate.GetString("m6t_dict_B1"));
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x001C1767 File Offset: 0x001BF967
		private static short BytesToShort(byte[] bytes)
		{
			return (short)((int)bytes[0] * 256 + (int)bytes[1]);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x001C1778 File Offset: 0x001BF978
		private static string GetMonitorNameFromCommand(string cmd)
		{
			if (Mode6Test.MonitorNames.Count == 0)
			{
				Mode6Test.FillDictionary();
			}
			string text = cmd.Substring(2);
			if (Mode6Test.MonitorNames.ContainsKey(text))
			{
				return Mode6Test.MonitorNames[text] + " - MID$" + text;
			}
			return "Manufacture defined monitor ID - $" + text;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x001C17D0 File Offset: 0x001BF9D0
		public static CustomPID CreateCustomPidFromMode6Test(Mode6Test m6t)
		{
			CustomPID customPID = new CustomPID(m6t.MonitorName, m6t.MonitorName, m6t.Cmd, "", "", m6t.Units, m6t.MinValue, m6t.MaxValue, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
			string letterFromPosition = CustomPID.GetLetterFromPosition(m6t.ValuePosition);
			string letterFromPosition2 = CustomPID.GetLetterFromPosition(m6t.ValuePosition + 1);
			string text = letterFromPosition + "*256+" + letterFromPosition2;
			if (m6t.IsCan)
			{
				customPID.Formula = Mode6Test.GetFormulaForCan(m6t, letterFromPosition, letterFromPosition2);
			}
			else
			{
				customPID.Formula = text;
			}
			return customPID;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x001C1888 File Offset: 0x001BFA88
		private static string GetFormulaForCan(Mode6Test m6t, string A, string B)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (m6t.TestType < 128)
			{
				text = A + "*256+" + B;
				switch (m6t.TestType)
				{
				case 1:
					m6t.Units = UnitsHelper.Units.None;
					text2 = text;
					break;
				case 2:
					m6t.Units = UnitsHelper.Units.None;
					text2 = text + "/10";
					break;
				case 3:
					m6t.Units = UnitsHelper.Units.None;
					text2 = text + "/100";
					break;
				case 4:
					m6t.Units = UnitsHelper.Units.None;
					text2 = text + "/100";
					break;
				case 5:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num = 1.999;
					text2 = Mode6Test.BuildCANFormula(text, num);
					break;
				}
				case 6:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num2 = 19.988;
					text2 = Mode6Test.BuildCANFormula(text, num2);
					break;
				}
				case 7:
				{
					m6t.Units = UnitsHelper.Units.rpm;
					double num3 = 16383.75;
					text2 = Mode6Test.BuildCANFormula(text, num3);
					break;
				}
				case 8:
				{
					m6t.Units = UnitsHelper.Units.kmh;
					double num4 = 655.35;
					text2 = Mode6Test.BuildCANFormula(text, num4);
					break;
				}
				case 9:
					text2 = text;
					m6t.Units = UnitsHelper.Units.kmh;
					break;
				case 10:
				{
					m6t.Units = UnitsHelper.Units.volts;
					double num5 = 7.995;
					text2 = Mode6Test.BuildCANFormula(text, num5);
					break;
				}
				case 11:
				{
					m6t.Units = UnitsHelper.Units.volts;
					double num6 = 65.535;
					text2 = Mode6Test.BuildCANFormula(text, num6);
					break;
				}
				case 12:
				{
					m6t.Units = UnitsHelper.Units.volts;
					double num7 = 655.35;
					text2 = Mode6Test.BuildCANFormula(text, num7);
					break;
				}
				case 13:
				{
					m6t.Units = UnitsHelper.Units.mA;
					double num8 = 255.996;
					text2 = Mode6Test.BuildCANFormula(text, num8);
					break;
				}
				case 14:
				{
					m6t.Units = UnitsHelper.Units.mA;
					double num9 = 65535.0;
					text2 = Mode6Test.BuildCANFormula(text, num9);
					break;
				}
				case 15:
				{
					m6t.Units = UnitsHelper.Units.mA;
					double num10 = 655350.0;
					text2 = Mode6Test.BuildCANFormula(text, num10);
					break;
				}
				case 16:
				{
					m6t.Units = UnitsHelper.Units.seconds;
					double num11 = 65.535;
					text2 = Mode6Test.BuildCANFormula(text, num11);
					break;
				}
				case 17:
				{
					m6t.Units = UnitsHelper.Units.seconds;
					double num12 = 6553.5;
					text2 = Mode6Test.BuildCANFormula(text, num12);
					break;
				}
				case 18:
				{
					m6t.Units = UnitsHelper.Units.seconds;
					double num13 = 65.535;
					text2 = Mode6Test.BuildCANFormula(text, num13);
					break;
				}
				case 19:
				{
					m6t.Units = UnitsHelper.Units.Ohm;
					double num14 = 65.535;
					text2 = Mode6Test.BuildCANFormula(text, num14);
					break;
				}
				case 20:
				{
					m6t.Units = UnitsHelper.Units.kOhm;
					double num15 = 65.535;
					text2 = Mode6Test.BuildCANFormula(text, num15);
					break;
				}
				case 21:
				{
					m6t.Units = UnitsHelper.Units.MOhm;
					double num16 = 65.535;
					text2 = Mode6Test.BuildCANFormula(text, num16);
					break;
				}
				case 22:
				{
					m6t.Units = UnitsHelper.Units.celicium;
					double num17 = 6553.5;
					double num18 = -40.0;
					text2 = Mode6Test.BuildCANFormula(text, num17, num18);
					break;
				}
				case 23:
				{
					m6t.Units = UnitsHelper.Units.kPa;
					double num19 = 655.35;
					double num20 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num19, num20);
					break;
				}
				case 24:
				{
					m6t.Units = UnitsHelper.Units.kPa;
					double num21 = 766.7595;
					double num22 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num21, num22);
					break;
				}
				case 25:
				{
					m6t.Units = UnitsHelper.Units.kPa;
					double num23 = 5177.265;
					double num24 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num23, num24);
					break;
				}
				case 26:
				{
					m6t.Units = UnitsHelper.Units.kPa;
					double num25 = 65535.0;
					double num26 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num25, num26);
					break;
				}
				case 27:
				{
					m6t.Units = UnitsHelper.Units.kPa;
					double num27 = 655350.0;
					double num28 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num27, num28);
					break;
				}
				case 28:
				{
					m6t.Units = UnitsHelper.Units.grads;
					double num29 = 655.35;
					double num30 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num29, num30);
					break;
				}
				case 29:
				{
					m6t.Units = UnitsHelper.Units.grads;
					double num31 = 32767.5;
					double num32 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num31, num32);
					break;
				}
				case 30:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num33 = 1.999;
					double num34 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num33, num34);
					break;
				}
				case 31:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num35 = 32767.75;
					double num36 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num35, num36);
					break;
				}
				case 32:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num37 = 255.993;
					double num38 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num37, num38);
					break;
				}
				case 33:
				{
					m6t.Units = UnitsHelper.Units.Hz;
					double num39 = 65.535;
					double num40 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num39, num40);
					break;
				}
				case 34:
				{
					m6t.Units = UnitsHelper.Units.Hz;
					double num41 = 65535.0;
					double num42 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num41, num42);
					break;
				}
				case 35:
				{
					m6t.Units = UnitsHelper.Units.MHz;
					double num43 = 65.535;
					double num44 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num43, num44);
					break;
				}
				case 36:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num45 = 65535.0;
					double num46 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num45, num46);
					break;
				}
				case 37:
				{
					m6t.Units = UnitsHelper.Units.km;
					double num47 = 65535.0;
					double num48 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num47, num48);
					break;
				}
				case 38:
				{
					m6t.Units = UnitsHelper.Units.Vms;
					double num49 = 6.5535;
					double num50 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num49, num50);
					break;
				}
				case 39:
				{
					m6t.Units = UnitsHelper.Units.grams_sec;
					double num51 = 655.35;
					double num52 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num51, num52);
					break;
				}
				case 40:
				{
					m6t.Units = UnitsHelper.Units.grams_sec;
					double num53 = 65535.0;
					double num54 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num53, num54);
					break;
				}
				case 41:
				{
					m6t.Units = UnitsHelper.Units.Pa_sec;
					double num55 = 16384.0;
					double num56 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num55, num56);
					break;
				}
				case 42:
				{
					m6t.Units = UnitsHelper.Units.kg_h;
					double num57 = 65.535;
					double num58 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num57, num58);
					break;
				}
				case 43:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num59 = 65535.0;
					double num60 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num59, num60);
					break;
				}
				case 44:
				{
					m6t.Units = UnitsHelper.Units.g_cyl;
					double num61 = 655.35;
					double num62 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num61, num62);
					break;
				}
				case 45:
				{
					m6t.Units = UnitsHelper.Units.g_stroke;
					double num63 = 655.35;
					double num64 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num63, num64);
					break;
				}
				case 46:
					m6t.Units = UnitsHelper.Units.None;
					text2 = text;
					break;
				case 47:
				{
					m6t.Units = UnitsHelper.Units.percent;
					double num65 = 655.35;
					double num66 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num65, num66);
					break;
				}
				case 48:
				{
					m6t.Units = UnitsHelper.Units.percent;
					double num67 = 100.00641;
					double num68 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num67, num68);
					break;
				}
				case 49:
				{
					m6t.Units = UnitsHelper.Units.liters;
					double num69 = 65.535;
					double num70 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num69, num70);
					break;
				}
				case 50:
				{
					m6t.Units = UnitsHelper.Units.mm;
					double num71 = 50.77;
					double num72 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num71, num72);
					break;
				}
				case 51:
				{
					m6t.Units = UnitsHelper.Units.None;
					double num73 = 16.0;
					double num74 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num73, num74);
					break;
				}
				case 52:
				{
					m6t.Units = UnitsHelper.Units.minutes;
					double num75 = 65535.0;
					double num76 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num75, num76);
					break;
				}
				case 53:
				{
					m6t.Units = UnitsHelper.Units.seconds;
					double num77 = 655.35;
					double num78 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num77, num78);
					break;
				}
				case 54:
				{
					m6t.Units = UnitsHelper.Units.gramms;
					double num79 = 655.35;
					double num80 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num79, num80);
					break;
				}
				case 55:
				{
					m6t.Units = UnitsHelper.Units.gramms;
					double num81 = 6553.5;
					double num82 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num81, num82);
					break;
				}
				case 56:
				{
					m6t.Units = UnitsHelper.Units.gramms;
					double num83 = 65535.0;
					double num84 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num83, num84);
					break;
				}
				case 57:
				{
					m6t.Units = UnitsHelper.Units.percent;
					double num85 = 327.67;
					double num86 = 0.0;
					text2 = Mode6Test.BuildCANFormula(text, num85, num86);
					break;
				}
				default:
					m6t.Units = UnitsHelper.Units.None;
					text2 = text;
					break;
				}
			}
			else
			{
				text = A + "," + B;
				int testType = m6t.TestType;
				if (testType <= 169)
				{
					switch (testType)
					{
					case 129:
						m6t.Units = UnitsHelper.Units.None;
						return Mode6Test.BuildCANFormulaSigned(text, 1.0);
					case 130:
						m6t.Units = UnitsHelper.Units.None;
						return Mode6Test.BuildCANFormulaSigned(text, 0.1);
					case 131:
						m6t.Units = UnitsHelper.Units.None;
						return Mode6Test.BuildCANFormulaSigned(text, 0.01);
					case 132:
						m6t.Units = UnitsHelper.Units.None;
						return Mode6Test.BuildCANFormulaSigned(text, 0.001);
					case 133:
						m6t.Units = UnitsHelper.Units.None;
						return Mode6Test.BuildCANFormulaSigned(text, 3.05E-05);
					case 134:
						m6t.Units = UnitsHelper.Units.None;
						return Mode6Test.BuildCANFormulaSigned(text, 0.000305);
					case 135:
					case 136:
					case 137:
					case 143:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 151:
					case 152:
					case 153:
					case 154:
					case 155:
						break;
					case 138:
						m6t.Units = UnitsHelper.Units.volts;
						return Mode6Test.BuildCANFormulaSigned(text, 122.0);
					case 139:
						m6t.Units = UnitsHelper.Units.volts;
						return Mode6Test.BuildCANFormulaSigned(text, 0.001);
					case 140:
						m6t.Units = UnitsHelper.Units.volts;
						return Mode6Test.BuildCANFormulaSigned(text, 0.01);
					case 141:
						m6t.Units = UnitsHelper.Units.mA;
						return Mode6Test.BuildCANFormulaSigned(text, 0.00390625);
					case 142:
						m6t.Units = UnitsHelper.Units.mA;
						return Mode6Test.BuildCANFormulaSigned(text, 1E-06);
					case 144:
						m6t.Units = UnitsHelper.Units.seconds;
						return Mode6Test.BuildCANFormulaSigned(text, 1.0);
					case 150:
						m6t.Units = UnitsHelper.Units.celicium;
						return Mode6Test.BuildCANFormulaSigned(text, 0.1);
					case 156:
						m6t.Units = UnitsHelper.Units.grads;
						return Mode6Test.BuildCANFormulaSigned(text, 0.01);
					case 157:
						m6t.Units = UnitsHelper.Units.grads;
						return Mode6Test.BuildCANFormulaSigned(text, 0.5);
					default:
						if (testType == 168)
						{
							m6t.Units = UnitsHelper.Units.grams_sec;
							return Mode6Test.BuildCANFormulaSigned(text, 1.0);
						}
						if (testType == 169)
						{
							m6t.Units = UnitsHelper.Units.Pa_sec;
							return Mode6Test.BuildCANFormulaSigned(text, 0.25);
						}
						break;
					}
				}
				else
				{
					switch (testType)
					{
					case 175:
						m6t.Units = UnitsHelper.Units.percent;
						return Mode6Test.BuildCANFormulaSigned(text, 0.01);
					case 176:
						m6t.Units = UnitsHelper.Units.percent;
						return Mode6Test.BuildCANFormulaSigned(text, 0.003052);
					case 177:
						m6t.Units = UnitsHelper.Units.mV_sec;
						return Mode6Test.BuildCANFormulaSigned(text, 2.0);
					default:
						if (testType == 253)
						{
							m6t.Units = UnitsHelper.Units.kPa;
							return Mode6Test.BuildCANFormulaSigned(text, 0.001);
						}
						if (testType == 254)
						{
							m6t.Units = UnitsHelper.Units.Pa;
							return Mode6Test.BuildCANFormulaSigned(text, 0.25);
						}
						break;
					}
				}
				m6t.Units = UnitsHelper.Units.None;
				text2 = Mode6Test.BuildCANFormulaSigned(text, 1.0);
			}
			return text2;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x001C25E9 File Offset: 0x001C07E9
		private static string BuildCANFormula(string AB, double test_max)
		{
			return string.Format("({0})*{1}/65535", AB, test_max.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x001C2604 File Offset: 0x001C0804
		private static string BuildCANFormula(string AB, double max, double min)
		{
			return string.Format("({0})*{1}/65535", AB, (Math.Abs(min) + Math.Abs(max)).ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x001C2636 File Offset: 0x001C0836
		private static string BuildCANFormulaSigned(string AB, double scaling)
		{
			return string.Format("ShortSigned({0})*{1}", AB, scaling.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00002050 File Offset: 0x00000250
		public Mode6Test()
		{
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x001C264F File Offset: 0x001C084F
		// Note: this type is marked as 'beforefieldinit'.
		static Mode6Test()
		{
		}

		// Token: 0x04001228 RID: 4648
		private bool _TestPassed;

		// Token: 0x04001229 RID: 4649
		[CompilerGenerated]
		private string <MonitorName>k__BackingField;

		// Token: 0x0400122A RID: 4650
		[CompilerGenerated]
		private string <TestId>k__BackingField;

		// Token: 0x0400122B RID: 4651
		[CompilerGenerated]
		private double <TestValue>k__BackingField;

		// Token: 0x0400122C RID: 4652
		[CompilerGenerated]
		private double <MinValue>k__BackingField;

		// Token: 0x0400122D RID: 4653
		[CompilerGenerated]
		private double <MaxValue>k__BackingField;

		// Token: 0x0400122E RID: 4654
		[CompilerGenerated]
		private int <ValuePosition>k__BackingField;

		// Token: 0x0400122F RID: 4655
		[CompilerGenerated]
		private string <Cmd>k__BackingField;

		// Token: 0x04001230 RID: 4656
		[CompilerGenerated]
		private bool <IsCan>k__BackingField;

		// Token: 0x04001231 RID: 4657
		[CompilerGenerated]
		private int <TestType>k__BackingField;

		// Token: 0x04001232 RID: 4658
		[CompilerGenerated]
		private UnitsHelper.Units <Units>k__BackingField;

		// Token: 0x04001233 RID: 4659
		private static Dictionary<string, string> MonitorNames = new Dictionary<string, string>(82);
	}
}
