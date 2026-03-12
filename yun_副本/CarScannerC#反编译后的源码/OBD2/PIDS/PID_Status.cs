using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200041E RID: 1054
	public class PID_Status : PID
	{
		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06002D19 RID: 11545 RVA: 0x001FE197 File Offset: 0x001FC397
		// (set) Token: 0x06002D1A RID: 11546 RVA: 0x001FE19F File Offset: 0x001FC39F
		public PID0101DataItem Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x001FE1A8 File Offset: 0x001FC3A8
		public PID_Status(string Name, string Command)
			: base(Name, Command)
		{
			this.Value = new PID0101DataItem();
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x001F2222 File Offset: 0x001F0422
		public override string ToString()
		{
			return base.Name;
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x001FE1C0 File Offset: 0x001FC3C0
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			new BitArrayReverse(data);
			this.Value = new PID0101DataItem();
			this.Value.MIL_ON = data[0] >> 7 == 1;
			byte b = (byte)(data[0] << 1);
			b = (byte)(b >> 1);
			this.Value.DTCs = (int)b;
			if (data.Length > 1)
			{
				bool bit_1_ = BitHelpers.GetBit_1_8(data[1], 4);
				this.Value.VehicleType = (bit_1_ ? PID0101DataItem.VehicleTypes.CompressionIgnition : PID0101DataItem.VehicleTypes.SparkIgnition);
				List<ECUTest> list = new List<ECUTest>(11);
				list.Add(new ECUTest(PID.GetResourceString("PID_Status_Misfire"))
				{
					Available = BitHelpers.GetBit_1_8(data[1], 1),
					Complete = !BitHelpers.GetBit_1_8(data[1], 5)
				});
				list.Add(new ECUTest(PID.GetResourceString("PID_Status_FuelSystem"))
				{
					Available = BitHelpers.GetBit_1_8(data[1], 2),
					Complete = !BitHelpers.GetBit_1_8(data[1], 6)
				});
				list.Add(new ECUTest(PID.GetResourceString("PID_Status_Components"))
				{
					Available = BitHelpers.GetBit_1_8(data[1], 3),
					Complete = !BitHelpers.GetBit_1_8(data[1], 7)
				});
				if (bit_1_)
				{
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_NMHC_Catalyst"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 1),
						Complete = !BitHelpers.GetBit_1_8(data[3], 1)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_NOxSCRMonitor"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 2),
						Complete = !BitHelpers.GetBit_1_8(data[3], 2)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_BoostPressure"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 4),
						Complete = !BitHelpers.GetBit_1_8(data[3], 4)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_ExhaustGasSensor"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 5),
						Complete = !BitHelpers.GetBit_1_8(data[3], 5)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_PMFilterMonitoring"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 6),
						Complete = !BitHelpers.GetBit_1_8(data[3], 6)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_EGRandorVTTSystem"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 7),
						Complete = !BitHelpers.GetBit_1_8(data[3], 7)
					});
				}
				else
				{
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_Catalyst"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 1),
						Complete = !BitHelpers.GetBit_1_8(data[3], 1)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_HeatedCatalyst"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 2),
						Complete = !BitHelpers.GetBit_1_8(data[3], 2)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_EvapSystem"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 3),
						Complete = !BitHelpers.GetBit_1_8(data[3], 3)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_SecondaryAirSystem"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 4),
						Complete = !BitHelpers.GetBit_1_8(data[3], 4)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_ACRefrigerant"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 5),
						Complete = !BitHelpers.GetBit_1_8(data[3], 5)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_OxygenSensor"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 6),
						Complete = !BitHelpers.GetBit_1_8(data[3], 6)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_OxygenSensorHeater"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 7),
						Complete = !BitHelpers.GetBit_1_8(data[3], 7)
					});
					list.Add(new ECUTest(PID.GetResourceString("PID_Status_EGRHeater"))
					{
						Available = BitHelpers.GetBit_1_8(data[2], 8),
						Complete = !BitHelpers.GetBit_1_8(data[3], 8)
					});
				}
				if (base.Id == 2)
				{
					using (List<ECUTest>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ECUTest ecutest = enumerator.Current;
							ecutest.Cycle = TestPidCycle.SinceDTCCleared;
						}
						goto IL_04A7;
					}
				}
				if (base.Id == 89)
				{
					foreach (ECUTest ecutest2 in list)
					{
						ecutest2.Cycle = TestPidCycle.SinceDTCCleared;
					}
				}
				IL_04A7:
				this.Value.ECUTests = list.ToArray();
			}
			else
			{
				this.Value.ECUTests = new ECUTest[0];
			}
			this.OnValueChanged();
		}

		// Token: 0x04001924 RID: 6436
		[CompilerGenerated]
		private PID0101DataItem <Value>k__BackingField;
	}
}
