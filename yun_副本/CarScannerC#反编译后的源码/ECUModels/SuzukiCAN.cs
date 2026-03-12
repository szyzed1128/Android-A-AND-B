using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200051B RID: 1307
	internal class SuzukiCAN : CAN11bitECU
	{
		// Token: 0x060031C1 RID: 12737 RVA: 0x002257C8 File Offset: 0x002239C8
		public SuzukiCAN(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AF", "18000000", "18004000", "18008000", "1800C000", "1800FF00", "1800FFFF" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "140000", "14FF00", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x002258A8 File Offset: 0x00223AA8
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string, string>("7E1", "7E9", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string, string>("261", "661", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string, string>("273", "673", "SRS/Airbags"),
				new ValueTuple<string, string, string>("272", "672", "HVAC (Heater, ventilation, A/C)"),
				new ValueTuple<string, string, string>("263", "663", "Power steering"),
				new ValueTuple<string, string, string>("271", "671", "BCM"),
				new ValueTuple<string, string, string>("262", "662", "4WD")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1800FF00", "1802FF00", "18000000", "1902AF", "1902AC" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF", "140000" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				SuzukiCAN suzukiCAN = new SuzukiCAN(valueTuple.Item3, item, item2);
				list.Add(suzukiCAN);
			}
			return list;
		}

		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x00225AAB File Offset: 0x00223CAB
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return SuzukiCAN.BuildList();
			}
		}
	}
}
