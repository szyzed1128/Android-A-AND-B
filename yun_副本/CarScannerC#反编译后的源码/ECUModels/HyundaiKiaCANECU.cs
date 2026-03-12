using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004DE RID: 1246
	internal class HyundaiKiaCANECU : CAN11bitECU
	{
		// Token: 0x060030B0 RID: 12464 RVA: 0x0021C0A0 File Offset: 0x0021A2A0
		public HyundaiKiaCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "1003", "1090", "1081" };
			base.ReadDTCCommands = new List<string> { "190208", "1902AC", "1902AF", "19020D", "1800FF00", "1802FF00" };
			base.ClearDTCCommands = new List<string> { "14", "14FF00", "14FFFF", "14FFFFFF", "144000", "146000", "142000" };
			base.CloseSessionCommands = new List<string> { "20" };
			base.IdentsASCII.TryAdd("22F180", "SW Boot ver.");
			base.IdentsASCII.TryAdd("22F181", "SW ver.");
			base.IdentsASCII.TryAdd("22F182", "SW data id.");
			base.IdentsASCII.TryAdd("22F18C", "ECU serial number");
			base.IdentsASCII.TryAdd("22F190", "VIN");
			base.IdentsASCII.TryAdd("22F191", "ECU part. num.");
			base.IdentsASCII.TryAdd("22F192", "Supplier part. number");
			base.IdentsASCII.TryAdd("22F195", "Supplier ECU");
			base.IdentsASCII.TryAdd("22F19C", "Calibration equipment software number");
			base.IdentsASCII.TryAdd("22F100", "Idents string");
			base.IdentsASCII.TryAdd("1A8C", "SW Boot ver.");
			base.IdentsASCII.TryAdd("1A8D", "SW ver.");
			base.IdentsASCII.TryAdd("1A8E", "Subsystem calibration version");
			base.IdentsASCII.TryAdd("1A80", "Idents string");
			base.IdentsASCII.TryAdd("1A91", "Part num.");
			this.AddUDSIdents();
			this.AddKWP2000Idents();
			this.CycleThroughOpenSessionCommands = true;
			if (base.RequestHeader == "7D2")
			{
				base.OpenSessionCommands = new List<string> { "1090", "1003", "1081" };
				base.RemoveOtherRequestsIfUDSReadResponded = true;
			}
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x0021C358 File Offset: 0x0021A558
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string[]>[] array = new ValueTuple<string, string, string[]>[]
			{
				new ValueTuple<string, string, string[]>("7E0", CAN11bitECU.ECU_ENGINE_NAME, new string[] { "190209", "19020D", "1800FF00" }),
				new ValueTuple<string, string, string[]>("7E7", CAN11bitECU.ECU_ENGINE_NAME + " #2", new string[] { "190209", "19020D", "1800FF00" }),
				new ValueTuple<string, string, string[]>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME, new string[] { "19020D", "1800FF00" }),
				new ValueTuple<string, string, string[]>("7E2", "Vehicle Motor control unit", null),
				new ValueTuple<string, string, string[]>("7E3", "Motor control unit", null),
				new ValueTuple<string, string, string[]>("7E4", "Battery management system", null),
				new ValueTuple<string, string, string[]>("7E5", "4WD #1 / OnBoard Charger (Hybrid/EV)", null),
				new ValueTuple<string, string, string[]>("7E6", "SBW Control unit", null),
				new ValueTuple<string, string, string[]>("723", "Seatbelt reminder / Lighting Module", new string[] { "190208" }),
				new ValueTuple<string, string, string[]>("725", "Wireless power charger", null),
				new ValueTuple<string, string, string[]>("73E", "Virtual Engine Sound system", null),
				new ValueTuple<string, string, string[]>("740", "4WD #2", new string[] { "1800FF00" }),
				new ValueTuple<string, string, string[]>("751", "Driver seat switch", null),
				new ValueTuple<string, string, string[]>("752", "Seat lumbar module", null),
				new ValueTuple<string, string, string[]>("755", "Blind spot detection right", null),
				new ValueTuple<string, string, string[]>("757", "Haptic steering Warning System", null),
				new ValueTuple<string, string, string[]>("760", "AFS", null),
				new ValueTuple<string, string, string[]>("770", "Integrated Gateway and power control module", null),
				new ValueTuple<string, string, string[]>("771", "Smart junction", null),
				new ValueTuple<string, string, string[]>("776", "Head Up Display", null),
				new ValueTuple<string, string, string[]>("777", "Power trunk module", null),
				new ValueTuple<string, string, string[]>("780", "Multimedia system", null),
				new ValueTuple<string, string, string[]>("783", "Amplifier", null),
				new ValueTuple<string, string, string[]>("791", "Active Hood system", new string[] { "1902FA", "1902AC", "18008000" }),
				new ValueTuple<string, string, string[]>("793", "Surround monitor view", null),
				new ValueTuple<string, string, string[]>("794", "OBC", null),
				new ValueTuple<string, string, string[]>("796", "Parking guide system/Rear view monitor", new string[] { "190208" }),
				new ValueTuple<string, string, string[]>("7A0", "BCM/TPMS #2", new string[] { "190208" }),
				new ValueTuple<string, string, string[]>("7A1", "Driver door", null),
				new ValueTuple<string, string, string[]>("7A2", "Passenger door", null),
				new ValueTuple<string, string, string[]>("7A3", "Electro seats", null),
				new ValueTuple<string, string, string[]>("7A4", "Steering wheel/column", null),
				new ValueTuple<string, string, string[]>("7A5", "Smart key", new string[] { "190208" }),
				new ValueTuple<string, string, string[]>("7A6", "Multi function switch", null),
				new ValueTuple<string, string, string[]>("7A7", "EPS(Electric power steering) #2", null),
				new ValueTuple<string, string, string[]>("7B1", "Parking assistance", null),
				new ValueTuple<string, string, string[]>("7B3", "HVAC", new string[] { "190208" }),
				new ValueTuple<string, string, string[]>("7B6", "E-Shifter", null),
				new ValueTuple<string, string, string[]>("7B7", "Blind spot detection", null),
				new ValueTuple<string, string, string[]>("7D0", "Adaptive cruise-control", new string[] { "190208" }),
				new ValueTuple<string, string, string[]>("7D1", CAN11bitECU.ECU_ABS_NAME, new string[] { "190208", "1902AC", "1800FF00", "18004000" }),
				new ValueTuple<string, string, string[]>("7E7", CAN11bitECU.ECU_ABS_NAME + " #2", null),
				new ValueTuple<string, string, string[]>("7D3", "Suspension", null),
				new ValueTuple<string, string, string[]>("7D2", "SRS/Airbag", new string[] { "190208", "1902AC", "1860FF00", "1820FF00", "1800FF00", "1802FF00" }),
				new ValueTuple<string, string, string[]>("7D4", "EPS (Electric power steering) #1", new string[] { "190208", "1902AC", "18004000" }),
				new ValueTuple<string, string, string[]>("7D5", "Electric parking brake", new string[] { "190208", "1902AC", "18004000" }),
				new ValueTuple<string, string, string[]>("7D6", "TPMS #1", null),
				new ValueTuple<string, string, string[]>("7C1", "Pre-safe seat belt", null),
				new ValueTuple<string, string, string[]>("7C4", "Lane assistance/Front view camera", null),
				new ValueTuple<string, string, string[]>("7C5", "LDC", null),
				new ValueTuple<string, string, string[]>("7C7", "ECALL/CUBIS", new string[] { "190229" }),
				new ValueTuple<string, string, string[]>("7C6", "Dashboard", new string[] { "190208" }),
				new ValueTuple<string, string, string[]>("730", "ADAS", null)
			};
			List<IECU> list = new List<IECU>(array.Length + 1);
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "1800FF00", "1800FF00",
					"1802FF00"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14", "14FF00", "14FFFF", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string, string[]> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string[] item3 = valueTuple.Item3;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Hyundai", null);
				HyundaiKiaCANECU hyundaiKiaCANECU = new HyundaiKiaCANECU(item2, item, possibleResponseHeader);
				if (item3 != null)
				{
					hyundaiKiaCANECU.ReadDTCCommands.Clear();
					hyundaiKiaCANECU.ReadDTCCommands.AddRange(item3);
				}
				if (hyundaiKiaCANECU.RequestHeader == "7E0" || hyundaiKiaCANECU.RequestHeader == "7E7")
				{
					hyundaiKiaCANECU.ClearDTCCommands.Insert(0, "04");
				}
				list.Add(hyundaiKiaCANECU);
			}
			IECU iecu = list.FirstOrDefault((IECU x) => x.RequestHeader == "7D1");
			if (iecu != null)
			{
				iecu.ReadDTCCommands.Add("18004000");
			}
			return list;
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x0021CBC4 File Offset: 0x0021ADC4
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return HyundaiKiaCANECU.BuildList();
			}
		}

		// Token: 0x020004DF RID: 1247
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060030B3 RID: 12467 RVA: 0x0021CBCB File Offset: 0x0021ADCB
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060030B4 RID: 12468 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060030B5 RID: 12469 RVA: 0x0021CBD7 File Offset: 0x0021ADD7
			internal bool <BuildList>b__1_0(IECU x)
			{
				return x.RequestHeader == "7D1";
			}

			// Token: 0x04001C38 RID: 7224
			public static readonly HyundaiKiaCANECU.<>c <>9 = new HyundaiKiaCANECU.<>c();

			// Token: 0x04001C39 RID: 7225
			public static Func<IECU, bool> <>9__1_0;
		}
	}
}
