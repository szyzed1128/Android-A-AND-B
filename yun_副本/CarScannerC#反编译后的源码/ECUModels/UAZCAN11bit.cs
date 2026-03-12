using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200051E RID: 1310
	internal class UAZCAN11bit : CAN11bitECU
	{
		// Token: 0x060031CA RID: 12746 RVA: 0x00226AE4 File Offset: 0x00224CE4
		public UAZCAN11bit(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "1902AF", "19020B", "1800FF00", "1802FF00", "18FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x00226BB0 File Offset: 0x00224DB0
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("7C7", "A/T Shifter (CAN)"),
				new ValueTuple<string, string>("7E3", "ABS (BOSCH 9.1/CAN)"),
				new ValueTuple<string, string>("7E5", "SRS/Takata (CAN)"),
				new ValueTuple<string, string>("7E4", "HVAC (CAN)")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "1800FF00", "1802FF00", "18FF00" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "UAZ", null);
				UAZCAN11bit uazcan11bit = new UAZCAN11bit(item2, item, possibleResponseHeader);
				list.Add(uazcan11bit);
			}
			return list;
		}

		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x060031CC RID: 12748 RVA: 0x00226D5B File Offset: 0x00224F5B
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return UAZCAN11bit.BuildList();
			}
		}
	}
}
